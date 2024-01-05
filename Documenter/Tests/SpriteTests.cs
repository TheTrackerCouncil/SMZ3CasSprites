using System.Text.RegularExpressions;
using SpriteDocumenter;

namespace Tests;

public class Tests
{
    private string _spriteDirectory = "";
    private List<Sprite> _linkSprites = new();
    private List<Sprite> _samusSprites = new();
    private List<Sprite> _shipSprites = new();
    
    [SetUp]
    public void Setup()
    {
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());

        while (directory != null && !directory.GetDirectories(".git").Any())
        {
            directory = directory.Parent;
        }

        var path = Path.Combine(directory!.FullName, "Sprites");

        if (!Directory.Exists(path))
        {
            throw new InvalidOperationException("Sprite folder not found");
        }
            
        _spriteDirectory = path;

        _linkSprites = Sprite.GetRdcSprites(Path.Combine(_spriteDirectory, "Link"));
        _samusSprites = Sprite.GetRdcSprites(Path.Combine(_spriteDirectory, "Samus"));
        _shipSprites = Sprite.GetIpsSprites(Path.Combine(_spriteDirectory, "Ships"));
    }

    [Test]
    public void ValidateLinkSprites()
    {
        ValidateSprites(_linkSprites);
    }
    
    [Test]
    public void ValidateSamusSprites()
    {
        ValidateSprites(_samusSprites);
    }
    
    [Test]
    public void ValidateShipSprites()
    {
        ValidateSprites(_shipSprites);
    }
    
    [Test]
    public void UpdateLinkSpriteMarkdown()
    {
        Sprite.WriteSpriteMarkdown("Link", Path.Combine(_spriteDirectory, "Link"), _linkSprites);
    }
    
    [Test]
    public void UpdateSamusSpriteMarkdown()
    {
        Sprite.WriteSpriteMarkdown("Samus", Path.Combine(_spriteDirectory, "Samus"), _samusSprites);
    }
    
    [Test]
    public void UpdateShipSpriteMarkdown()
    {
        Sprite.WriteSpriteMarkdown("Ship", Path.Combine(_spriteDirectory, "Ships"), _shipSprites);
    }

    [Test]
    public void ValidateLinkSpriteSize()
    {
        ValidateSpriteSizes(_linkSprites, 64, 96);
    }
    
    [Test]
    public void ValidateSamusSpriteSize()
    {
        ValidateSpriteSizes(_samusSprites, 128, 212);
    }
    
    [Test]
    public void ValidateShipSpriteSize()
    {
        ValidateSpriteSizes(_shipSprites, 248, 92);
    }

    private void ValidateSprites(List<Sprite> sprites)
    {
        foreach (var sprite in sprites)
        {
            Assert.IsNotEmpty(sprite.Name, $"Expected sprite name for {sprite.SpritePath}");
            Assert.IsNotEmpty(sprite.Author, $"Expected sprite name for sprite {sprite.Name}");
            Assert.NotNull(sprite.PreviewImage, $"Expected preview image for sprite {sprite.Name} by {sprite.Author}");
            Assert.True(File.Exists(sprite.PreviewImage), $"Sprite {sprite.PreviewImage} was not found for {sprite.Name} by {sprite.Author}");
        }
    }

    private void ValidateMarkdown(string type, string path, List<Sprite> sprites)
    {
        path = Path.Combine(path, "README.md");
        var fileMarkdown = File.ReadAllText(path);
        fileMarkdown = Regex.Replace(fileMarkdown, @"\s*(\r?\n)+\s*", "\r\n").ToLower();

        var testMarkdown = Sprite.GetSpriteMarkdown(type, sprites);
        testMarkdown = Regex.Replace(testMarkdown, @"\s*(\r?\n)+\s*", "\r\n").ToLower();
        
        Assert.That(testMarkdown, Is.EqualTo(fileMarkdown), $"Markdown in {path} does not match expected markdown");
    }

    private void ValidateSpriteSizes(List<Sprite> sprites, int width, int height)
    {
        foreach (var sprite in sprites.Where(x => !string.IsNullOrEmpty(x.PreviewImage)))
        {
            using var stream = File.OpenRead(sprite.PreviewImage!);

            var fileHeader = SkiaSharp.SKBitmap.DecodeBounds(stream);
            if (fileHeader.IsEmpty) 
            {
                // Handle failed decoding here
            }

            Assert.That(fileHeader.Width, Is.EqualTo(width), $"Sprite preview png for {sprite.Name} by {sprite.Author} should have a width of {width} pixels");
            Assert.That(fileHeader.Height, Is.EqualTo(height), $"Sprite preview png for {sprite.Name} by {sprite.Author} should have a height of {height} pixels");
        }
    }
}