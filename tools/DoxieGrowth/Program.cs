using System.Globalization;
using System.Text;
using DoxieGrowth;
using ShopifySharp.Utilities;

// DoxieGrowth — free growth tooling for a Shopify print-on-demand store.
//
//   audit                     Report what is hurting the store's search & social traffic.
//   fix-seo [--apply]         Fill missing SEO titles/descriptions and image alt text (dry run unless --apply).
//   pinterest [options]       Write a Pinterest bulk-upload CSV with scheduled pins for every active product.
//       --board <name>        Pinterest board the pins go to (must already exist). Default: "Dachshund Shirts & Gifts"
//       --per-day <n>         Pins scheduled per day. Default: 3
//       --start <yyyy-MM-dd>  First publish day (UTC). Default: tomorrow
//       --out <file>          Output file. Default: pinterest-pins.csv
//
// Environment: SHOPIFY_SHOP (e.g. my-store.myshopify.com) plus either SHOPIFY_CLIENT_ID + SHOPIFY_CLIENT_SECRET
// (custom app client credentials) or SHOPIFY_TOKEN (an Admin API access token).

var command = args.FirstOrDefault()?.ToLowerInvariant();
if (command is null or "help" or "--help" or "-h")
{
    PrintUsage();
    return 0;
}

if (command is not ("audit" or "fix-seo" or "pinterest"))
{
    Console.Error.WriteLine($"Unknown command '{command}'.");
    PrintUsage();
    return 1;
}

var shopDomain = Env("SHOPIFY_SHOP");
var token = Env("SHOPIFY_TOKEN");
var clientId = Env("SHOPIFY_CLIENT_ID");
var clientSecret = Env("SHOPIFY_CLIENT_SECRET");

if (shopDomain is null || (token is null && (clientId is null || clientSecret is null)))
{
    Console.Error.WriteLine("Set SHOPIFY_SHOP plus either SHOPIFY_CLIENT_ID + SHOPIFY_CLIENT_SECRET or SHOPIFY_TOKEN. See tools/DoxieGrowth/README.md.");
    return 1;
}

// Custom apps created since 2026 authenticate with client credentials, which mint a 24-hour access token.
token ??= (await new ShopifyOauthUtility().GetClientCredentialsAccessTokenAsync(new ClientCredentialsAccessTokenOptions
{
    ClientId = clientId!,
    ClientSecret = clientSecret!,
    ShopDomain = shopDomain
})).AccessToken;

var client = new ShopifyClient(shopDomain, token);
var shop = await client.GetShopAsync();
var products = await client.GetProductsAsync();

return command switch
{
    "audit" => Audit(shop, products),
    "fix-seo" => await FixSeo(client, shop, products, args.Contains("--apply")),
    _ => Pinterest(shop, products, args)
};

static void PrintUsage() => Console.WriteLine("""
    DoxieGrowth — free growth tooling for your Shopify store

    Usage:
      dotnet run -- audit
      dotnet run -- fix-seo            (preview)
      dotnet run -- fix-seo --apply    (write changes to Shopify)
      dotnet run -- pinterest [--board "Board Name"] [--per-day 3] [--start 2026-10-12] [--out pins.csv]

    Requires SHOPIFY_SHOP plus SHOPIFY_CLIENT_ID + SHOPIFY_CLIENT_SECRET (or SHOPIFY_TOKEN).
    """);

static int Audit(Shop shop, List<Product> products)
{
    Console.WriteLine($"Audit for {shop.Name} ({shop.PrimaryUrl}) — {products.Count} products\n");

    var active = products.Where(p => p.Status == "ACTIVE").ToList();
    var drafts = products.Where(p => p.Status == "DRAFT").ToList();
    var issues = 0;

    void Issue(string message)
    {
        issues++;
        Console.WriteLine($"  - {message}");
    }

    if (drafts.Count > 0)
    {
        Console.WriteLine($"[!] {drafts.Count} draft products are invisible to customers and search engines:");
        foreach (var p in drafts) Issue($"DRAFT: {p.Title}");
        Console.WriteLine();
    }

    foreach (var p in active)
    {
        var problems = new List<string>();
        if (p.OnlineStoreUrl is null) problems.Add("not published to the Online Store sales channel");
        if (p.SeoTitle is null) problems.Add("no SEO title (Google uses the raw product title)");
        if (p.SeoDescription is null) problems.Add("no SEO meta description");
        if (p.DescriptionText.Length < 300) problems.Add($"short description ({p.DescriptionText.Length} chars; aim for 300+)");
        if (p.DescriptionText.TrimStart().StartsWith("XS S M L")) problems.Add("description starts with a size chart instead of selling copy");
        var missingAlt = p.Images.Count(i => i.Alt is null);
        if (missingAlt > 0) problems.Add($"{missingAlt}/{p.Images.Count} images without alt text (hurts Google Images)");
        if (p.Images.Count < 4) problems.Add($"only {p.Images.Count} images (use Printify mockups to reach 4+)");

        if (problems.Count == 0) continue;
        Console.WriteLine($"[{p.Title}]");
        foreach (var problem in problems) Issue(problem);
        Console.WriteLine();
    }

    Console.WriteLine(issues == 0
        ? "No issues found."
        : $"{issues} issues found. Run 'fix-seo --apply' to fix SEO titles, meta descriptions and alt text automatically.");
    return 0;
}

static async Task<int> FixSeo(ShopifyClient client, Shop shop, List<Product> products, bool apply)
{
    Console.WriteLine(apply ? "Applying SEO fixes...\n" : "Preview (nothing is written; add --apply to save):\n");
    var changes = 0;

    foreach (var p in products.Where(p => p.Status != "ARCHIVED"))
    {
        if (p.SeoTitle is null || p.SeoDescription is null)
        {
            var title = p.SeoTitle ?? Copy.SeoTitle(p, shop);
            var description = p.SeoDescription ?? Copy.SeoDescription(p);
            Console.WriteLine($"[{p.Title}]\n  SEO title:       {title}\n  SEO description: {description}");
            if (apply) await client.UpdateSeoAsync(p.Id, title, description);
            changes++;
        }

        var index = 0;
        foreach (var image in p.Images)
        {
            index++;
            if (image.Alt is not null) continue;
            var alt = Copy.AltText(p, index);
            Console.WriteLine($"  image {index} alt:     {alt}");
            if (apply) await client.UpdateImageAltAsync(image.MediaId, alt);
            changes++;
        }
    }

    Console.WriteLine(changes == 0
        ? "Nothing to fix."
        : apply ? $"\nDone — {changes} changes saved." : $"\n{changes} changes pending. Re-run with --apply to save them.");
    return 0;
}

static int Pinterest(Shop shop, List<Product> products, string[] args)
{
    var board = Option(args, "--board") ?? "Dachshund Shirts & Gifts";
    var perDay = int.TryParse(Option(args, "--per-day"), out var n) && n > 0 ? n : 3;
    var start = DateTime.TryParseExact(Option(args, "--start"), "yyyy-MM-dd", CultureInfo.InvariantCulture,
        DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var s)
        ? s.Date
        : DateTime.UtcNow.Date.AddDays(1);
    var output = Option(args, "--out") ?? "pinterest-pins.csv";

    var pins = PinterestCsv.Build(shop, products, board, perDay, start);
    if (pins.Count == 0)
    {
        Console.Error.WriteLine("No active products with images — publish some products first.");
        return 1;
    }

    File.WriteAllText(output, PinterestCsv.ToCsv(pins), new UTF8Encoding(false));
    Console.WriteLine($"Wrote {pins.Count} pins for {pins.Select(p => p.Link.Split('?')[0]).Distinct().Count()} products to {Path.GetFullPath(output)}");
    Console.WriteLine($"Schedule: {perDay}/day from {pins[0].PublishAt:yyyy-MM-dd} to {pins[^1].PublishAt:yyyy-MM-dd} (UTC), board \"{board}\".");
    Console.WriteLine("Upload it at pinterest.com → Create → Create Pins in bulk → Upload .csv file.");
    return 0;
}

static string? Env(string name) =>
    Environment.GetEnvironmentVariable(name) is { Length: > 0 } value ? value.Trim() : null;

static string? Option(string[] args, string name)
{
    var index = Array.IndexOf(args, name);
    return index >= 0 && index + 1 < args.Length ? args[index + 1] : null;
}
