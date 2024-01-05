# SMZ3 Cas' Sprites

This repository is for various in game and tracker UI sprites for the [SMZ3 Cas' Randomizer](https://github.com/Vivelin/SMZ3Randomizer). These sprites are both bundled with the application install and, if enabled, will be downloaded automatically on launch of the randomizer.

## Sprite Folders

- **Dungeons** - Dungeon abbreviation and reward sprites for the tracker UI.
- **Items** - Item sprites for the tracker UI.
- **Link** - In game rdc sprites for Link with preview images.
- **Maps** - Map images for displaying in the tracker map window
- **Marks** - Number sprites used for the tracker UI.
- **Samus** - In game rdc sprites for Samus with preview images.
- **Ships** - In game ips ship patches with preview images.

## Submitting Sprites

To submit sprites, create a fork/branch and create a pull request with the following file updates:

- Sprite file (rdc for Link/Samus sprites or ips patch for ship sprites)
  - rdc files should have the appropriate title and author metadata added
  - Ship sprite filenames should be labeled like "SpriteTitle by SpriteAuthor.ips" to be parsed correctly
- A png preview image needs to be included with the sprite with the following dimensions:
  - Link sprites: 64x96 pixels (with shadow included)
  - Samus sprites: 128x212 pixels
  - Ship sprites: 248x92 pixels

## Documenter App

The Documenter solution is a C# application built to generate the sprite README.md files and unit tests to validate all of the sprites. This will run automatically when a PR is created to validate that the sprites have all necessary metadata, match the correct resolution, and will then create an additional commit in the PR to update the documentation.

## Credits

Each folder includes a README.md file with all available sprites listed with their title and original creators.

- [Link Sprite Credits](Sprites/Link/README.md)
- [Samus Sprite Credits](Sprites/Samus/README.md)
- [Ship Sprite Credits](Sprites/Ships/README.md)