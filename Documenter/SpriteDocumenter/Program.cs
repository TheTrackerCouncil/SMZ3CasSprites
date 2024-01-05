using Serilog;

namespace SpriteDocumenter;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Debug()
            .WriteTo.Console()
            .CreateLogger();

        var spritePath = GetSpritePath;
        Log.Information("Sprite path: {Path}", spritePath);

        var linkSprites = Sprite.GetRdcSprites(Path.Combine(spritePath, "Link"));
        var samusSprites = Sprite.GetRdcSprites(Path.Combine(spritePath, "Samus"));
        var shipSprites = Sprite.GetIpsSprites(Path.Combine(spritePath, "Ships"));
        
        Log.Information("{Count} Link sprites found", linkSprites.Count);
        Log.Information("{Count} Samus sprites found", samusSprites.Count);
        Log.Information("{Count} ship sprites found", shipSprites.Count);

        Sprite.WriteSpriteMarkdown("Link", Path.Combine(spritePath, "Link"), linkSprites);
        Log.Information("Wrote {Type} README.md file {Path}", "Link", Path.Combine(spritePath, "Link"));
        
        Sprite.WriteSpriteMarkdown("Samus", Path.Combine(spritePath, "Samus"), samusSprites);
        Log.Information("Wrote {Type} README.md file {Path}", "Samus", Path.Combine(spritePath, "Samus"));
        
        Sprite.WriteSpriteMarkdown("Ship", Path.Combine(spritePath, "Ships"), shipSprites);
        Log.Information("Wrote {Type} README.md file {Path}", "Ship", Path.Combine(spritePath, "Ships"));
    }

    private static string GetSpritePath
    {
        get
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
            
            return path;
        }
    }
}