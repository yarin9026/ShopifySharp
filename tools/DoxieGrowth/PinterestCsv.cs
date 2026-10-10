using System.Globalization;
using System.Text;

namespace DoxieGrowth;

public record Pin(string Title, string MediaUrl, string Board, string Description, string Link, DateTime PublishAt, string Keywords);

/// Builds the CSV accepted by Pinterest's "Create Pins in bulk" uploader.
public static class PinterestCsv
{
    // Pinterest's bulk uploader accepts at most 200 rows per file.
    public const int MaxRows = 200;

    // Posting window in UTC: 13:00 → 03:00 next day, i.e. US morning through US evening.
    private const double WindowStartHour = 13;
    private const double WindowLengthHours = 14;

    public static List<Pin> Build(Shop shop, IEnumerable<Product> products, string board, int perDay, DateTime startDay)
    {
        var active = products.Where(p => p.Status == "ACTIVE" && p.Images.Count > 0).ToList();
        if (active.Count == 0) return [];

        // One pin per image, interleaved across products so the same product isn't posted back to back.
        var picks = new List<(Product Product, ProductImage Image, int Variant)>();
        var maxImages = active.Max(p => p.Images.Count);
        for (var i = 0; i < maxImages && picks.Count < MaxRows; i++)
            foreach (var p in active.Where(p => i < p.Images.Count).Take(MaxRows - picks.Count))
                picks.Add((p, p.Images[i], i));

        return picks.Select((pick, i) =>
        {
            var slot = i % perDay;
            var publishAt = startDay.Date
                .AddDays(i / perDay)
                .AddMinutes(Math.Round((WindowStartHour + slot * WindowLengthHours / perDay) * 60));
            var link = (pick.Product.OnlineStoreUrl ?? $"{shop.PrimaryUrl}/products/{pick.Product.Handle}") +
                       $"?utm_source=pinterest&utm_medium=social&utm_campaign=bulk_pins&utm_content=pin{i + 1}";

            return new Pin(
                Copy.PinTitle(pick.Product, pick.Variant),
                pick.Image.Url,
                board,
                Copy.PinDescription(pick.Product, pick.Variant),
                link,
                publishAt,
                string.Join(", ", Copy.Keywords(pick.Product)));
        }).ToList();
    }

    public static string ToCsv(IEnumerable<Pin> pins)
    {
        var csv = new StringBuilder();
        csv.AppendLine("Title,Media URL,Pinterest board,Thumbnail,Description,Link,Publish date,Keywords");
        foreach (var pin in pins)
            csv.AppendLine(string.Join(",",
                Quote(pin.Title),
                Quote(pin.MediaUrl),
                Quote(pin.Board),
                "",
                Quote(pin.Description),
                Quote(pin.Link),
                pin.PublishAt.ToString("yyyy-MM-dd'T'HH:mm:ss", CultureInfo.InvariantCulture),
                Quote(pin.Keywords)));
        return csv.ToString();
    }

    private static string Quote(string value) => $"\"{value.Replace("\"", "\"\"")}\"";
}
