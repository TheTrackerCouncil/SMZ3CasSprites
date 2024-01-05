using System.Text;
using System.Web;

namespace SpriteDocumenter;

public class Sprite
{
    public required string SpritePath { get; set; }
    public required string Name { get; set; }
    public required string Author { get; set; }
    public string? PreviewImage { get; set; }
    public string? EncodedImageName { get; set; }
    
    public static List<Sprite> GetRdcSprites(string path)
    {
        var toReturn = new List<Sprite>();
        
        foreach (var rdcFilePath in Directory.EnumerateFiles(path, "*.rdc"))
        {
            using var stream = File.OpenRead(rdcFilePath);
            var rdc = Rdc.Parse(stream);
            
            var name = Path.GetFileName(rdcFilePath);
            var author = rdc.Author;
            if (rdc.TryParse<MetaDataBlock>(stream, out var block))
            {
                var title = block?.Content?.Value<string>("title");
                if (!string.IsNullOrEmpty(title))
                    name = title;

                var author2 = block?.Content?.Value<string>("author");
                if (string.IsNullOrEmpty(author) && !string.IsNullOrEmpty(author2))
                    author = author2;
            }
            
            var file = new FileInfo(rdcFilePath);
            var previewPath = file.FullName.Replace(file.Extension, ".png");
            string? encodedImagePath = null;
            if (!File.Exists(previewPath))
            {
                previewPath = null;
            }
            else
            {
                encodedImagePath = HttpUtility.UrlEncode(Path.GetFileName(previewPath));
            }
            
            toReturn.Add(new Sprite()
            {
                SpritePath = rdcFilePath,
                Name = name,
                Author = author,
                PreviewImage = previewPath,
                EncodedImageName = encodedImagePath
            });
        }

        return toReturn;
    }
    
    public static List<Sprite> GetIpsSprites(string path)
    {
        var toReturn = new List<Sprite>();
        
        foreach (var ipsFilePath in Directory.EnumerateFiles(path, "*.ips"))
        {
            var filename = Path.GetFileNameWithoutExtension(ipsFilePath);
            var name = filename;
            var author = "";

            if (name.Contains(" by "))
            {
                var parts = name.Split(" by ");
                name = parts.First();
                author = parts.Last();
            }
            
            var file = new FileInfo(ipsFilePath);
            var previewPath = file.FullName.Replace(file.Extension, ".png");
            string? encodedImagePath = null;
            if (!File.Exists(previewPath))
            {
                previewPath = null;
            }
            else
            {
                encodedImagePath = Uri.EscapeDataString(Path.GetFileName(previewPath));
            }
            
            toReturn.Add(new Sprite()
            {
                SpritePath = ipsFilePath,
                Name = name,
                Author = author,
                PreviewImage = previewPath,
                EncodedImageName = encodedImagePath
            });
        }

        return toReturn;
    }
    
    public static void WriteSpriteMarkdown(string type, string path, List<Sprite> sprites)
    {
        path = Path.Combine(path, "README.md");
        File.WriteAllText(path, GetSpriteMarkdown(type, sprites));
    }

    public static string GetSpriteMarkdown(string type, List<Sprite> sprites)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"# {type} Sprites");
        stringBuilder.AppendLine();
        foreach (var sprite in sprites.OrderBy(x => x.Name).ThenBy(x => x.Author).ThenBy(x => x.SpritePath))
        {
            stringBuilder.AppendLine($"### {sprite.Name} by {sprite.Author}");
            stringBuilder.AppendLine($"![image]({sprite.EncodedImageName})");
            stringBuilder.AppendLine();
        }
        return stringBuilder.ToString();
    }
}