using System.Diagnostics;
using NetVips;

var inputBytes = await File.ReadAllBytesAsync("Rectangles.png");

// This is the original 10000x10000 image
using var image = Image.NewFromBuffer(inputBytes);

// Use a more manageable size as a base for the other thumbnails
// and force libvips to fully build it in memory
using var preThumb = image.ThumbnailImage(2880, size: Enums.Size.Down);
var preThumbBytes = preThumb.RawsaveBuffer();
using var preThumbImage = Image.NewFromMemory(
    preThumbBytes,
    preThumb.Width,
    preThumb.Height,
    preThumb.Bands,
    preThumb.Format
);

foreach (var width in new[] { 2880, 1440, 1000, 720, 480, 320, 260, 130 })
{
    var stopwatch = Stopwatch.StartNew();

    using var thumb = preThumbImage.ThumbnailImage(width, size: Enums.Size.Down);
    var bytes = thumb.JpegsaveBuffer(background: [255]);
    await File.WriteAllBytesAsync($"out/Rectangles_{width}.jpeg", bytes);

    Console.WriteLine($"Created thumbnail with width {width} pixels in {stopwatch.Elapsed}");
}
