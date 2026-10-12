using System.Text.Json;
using ShopifySharp;
using ShopifySharp.Services.Graph;

namespace DoxieGrowth;

public record ProductImage(string MediaId, string Url, string? Alt);

public record Product(
    string Id,
    string Title,
    string Handle,
    string Status,
    string ProductType,
    string DescriptionText,
    string? SeoTitle,
    string? SeoDescription,
    string? OnlineStoreUrl,
    decimal MinPrice,
    string Currency,
    IReadOnlyList<string> Tags,
    IReadOnlyList<ProductImage> Images);

public record Shop(string Name, string PrimaryUrl);

/// Thin wrapper around ShopifySharp's GraphService for the few queries this tool needs.
public class ShopifyClient(string shopDomain, string accessToken)
{
    private readonly GraphService _graph = new(shopDomain, accessToken);

    private async Task<JsonElement> RunAsync(string query, Dictionary<string, object>? variables = null)
    {
        using var result = await _graph.PostAsync(new GraphRequest
        {
            Query = query,
            Variables = variables,
            UserErrorHandling = GraphRequestUserErrorHandling.Throw
        });

        using var doc = JsonDocument.Parse(result.Json.GetRawText());
        var root = doc.RootElement.Clone();
        return root.TryGetProperty("data", out var data) ? data : root;
    }

    public async Task<Shop> GetShopAsync()
    {
        var data = await RunAsync("{ shop { name primaryDomain { url } } }");
        var shop = data.GetProperty("shop");
        return new Shop(
            shop.GetProperty("name").GetString()!,
            shop.GetProperty("primaryDomain").GetProperty("url").GetString()!.TrimEnd('/'));
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        const string query = """
            query Products($after: String) {
              products(first: 50, after: $after) {
                pageInfo { hasNextPage endCursor }
                nodes {
                  id title handle status productType description onlineStoreUrl tags
                  seo { title description }
                  priceRangeV2 { minVariantPrice { amount currencyCode } }
                  media(first: 20) {
                    nodes { id alt ... on MediaImage { image { url } } }
                  }
                }
              }
            }
            """;

        var products = new List<Product>();
        string? after = null;

        do
        {
            var data = await RunAsync(query, new Dictionary<string, object> { ["after"] = after! });
            var connection = data.GetProperty("products");

            foreach (var node in connection.GetProperty("nodes").EnumerateArray())
                products.Add(ParseProduct(node));

            var pageInfo = connection.GetProperty("pageInfo");
            after = pageInfo.GetProperty("hasNextPage").GetBoolean()
                ? pageInfo.GetProperty("endCursor").GetString()
                : null;
        } while (after is not null);

        return products;
    }

    public static Product ParseProduct(JsonElement node)
    {
        var seo = node.GetProperty("seo");
        var price = node.GetProperty("priceRangeV2").GetProperty("minVariantPrice");
        var images = new List<ProductImage>();

        foreach (var media in node.GetProperty("media").GetProperty("nodes").EnumerateArray())
        {
            if (!media.TryGetProperty("image", out var image) || image.ValueKind != JsonValueKind.Object)
                continue;

            images.Add(new ProductImage(
                media.GetProperty("id").GetString()!,
                image.GetProperty("url").GetString()!,
                StringOrNull(media, "alt")));
        }

        return new Product(
            node.GetProperty("id").GetString()!,
            node.GetProperty("title").GetString()!,
            node.GetProperty("handle").GetString()!,
            node.GetProperty("status").GetString()!,
            node.GetProperty("productType").GetString() ?? "",
            node.GetProperty("description").GetString() ?? "",
            StringOrNull(seo, "title"),
            StringOrNull(seo, "description"),
            StringOrNull(node, "onlineStoreUrl"),
            decimal.Parse(price.GetProperty("amount").GetString()!, System.Globalization.CultureInfo.InvariantCulture),
            price.GetProperty("currencyCode").GetString()!,
            node.GetProperty("tags").EnumerateArray().Select(t => t.GetString()!).ToList(),
            images);
    }

    private static string? StringOrNull(JsonElement element, string property) =>
        element.TryGetProperty(property, out var value) && value.ValueKind == JsonValueKind.String
            ? (string.IsNullOrWhiteSpace(value.GetString()) ? null : value.GetString())
            : null;

    public Task UpdateSeoAsync(string productId, string seoTitle, string seoDescription) =>
        RunAsync("""
            mutation UpdateSeo($product: ProductUpdateInput!) {
              productUpdate(product: $product) { product { id } userErrors { field message } }
            }
            """,
            new Dictionary<string, object>
            {
                ["product"] = new Dictionary<string, object>
                {
                    ["id"] = productId,
                    ["seo"] = new Dictionary<string, object> { ["title"] = seoTitle, ["description"] = seoDescription }
                }
            });

    public Task UpdateImageAltAsync(string mediaId, string alt) =>
        RunAsync("""
            mutation UpdateAlt($files: [FileUpdateInput!]!) {
              fileUpdate(files: $files) { files { id } userErrors { field message } }
            }
            """,
            new Dictionary<string, object>
            {
                ["files"] = new[] { new Dictionary<string, object> { ["id"] = mediaId, ["alt"] = alt } }
            });
}
