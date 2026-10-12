namespace DoxieGrowth;

/// Generates SEO and Pinterest copy from the product data already in Shopify.
public static class Copy
{
    // Different angles so several pins for the same product don't look like duplicates to Pinterest.
    private static readonly string[] PinHooks =
    [
        "{0}",
        "The Perfect Gift for Dachshund Lovers: {0}",
        "{0} | Cute Wiener Dog Outfit Idea",
        "Doxie Parents Will Love This {1}",
        "{0} – Sausage Dog Style",
        "Funny Dachshund {1} Gift Idea"
    ];

    private static readonly string[] PinCallsToAction =
    [
        "Tap to shop and pick your size & color.",
        "Save this for your next gift idea!",
        "Order now — printed on demand just for you.",
        "Every doxie parent needs one. Shop the link!"
    ];

    // Printify's default tags describe the blank, not the design — useless as search keywords.
    private static readonly HashSet<string> GenericTags = ["unisex", "cotton", "t-shirts", "sweatshirts", "hoodies"];

    /// "Dachshund Mama Retro T-Shirt - Comfort Colors ..." → "Dachshund Mama Retro T-Shirt"
    public static string ShortName(Product product) =>
        product.Title.Split(" - ", 2)[0].Trim();

    private static string Kind(Product product) =>
        string.IsNullOrWhiteSpace(product.ProductType) ? "Shirt" : product.ProductType;

    public static string SeoTitle(Product product, Shop shop) =>
        Truncate($"{ShortName(product)} | {shop.Name}", 70);

    public static string SeoDescription(Product product)
    {
        var summary = Summary(product.DescriptionText, 80);
        var text = $"{summary} Shop the {ShortName(product)} from {product.Currency} {product.MinPrice:0.00}. " +
                   "Soft, printed on demand, many sizes & colors.";
        return Truncate(text, 160);
    }

    public static string AltText(Product product, int imageIndex) =>
        Truncate(imageIndex == 1
            ? $"{ShortName(product)} — dachshund {Kind(product).ToLowerInvariant()} for wiener dog lovers"
            : $"{ShortName(product)} — view {imageIndex}", 125);

    public static string PinTitle(Product product, int variant) =>
        Truncate(string.Format(PinHooks[variant % PinHooks.Length], ShortName(product), Kind(product)), 100);

    public static string PinDescription(Product product, int variant)
    {
        var hashtags = string.Join(" ", Keywords(product).Take(5).Select(k => "#" + k.Replace(" ", "")));
        var text = $"{Summary(product.DescriptionText, 320)} {PinCallsToAction[variant % PinCallsToAction.Length]} {hashtags}";
        return Truncate(text, 500);
    }

    public static IEnumerable<string> Keywords(Product product) =>
        product.Tags
            .Select(t => t.ToLowerInvariant())
            .Where(t => t.Length > 3 && !GenericTags.Contains(t))
            .Distinct()
            .Take(10);

    /// Leading whole sentences of the product description, up to roughly <paramref name="max"/> characters.
    private static string Summary(string text, int max)
    {
        text = text.Trim();
        if (text.Length == 0 || text.StartsWith("XS S M L")) return "";

        var summary = "";
        var position = 0;
        while (position < text.Length)
        {
            var end = text.IndexOfAny(['.', '!', '?'], position);
            var sentence = end < 0 ? text[position..] : text[position..(end + 1)];
            if (summary.Length > 0 && summary.Length + sentence.Length > max) break;
            summary += sentence;
            position += sentence.Length;
            if (end < 0) break;
        }

        return Truncate(summary, max);
    }

    private static string Truncate(string text, int max)
    {
        text = text.Trim();
        if (text.Length <= max) return text;
        var cut = text[..(max - 1)];
        var space = cut.LastIndexOf(' ');
        return (space > max / 2 ? cut[..space] : cut).TrimEnd(',', ' ', '-', '|') + "…";
    }
}
