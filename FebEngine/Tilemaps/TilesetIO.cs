using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebEngine.Tilemaps
{
  internal class TilesetIO
  {
    /*
    private static Tileset ReadTileset(string document)
    {
      XDocument parsedTileset = XDocument.Parse(document);

      XElement root = parsedTileset.Root;

      string texturePath = root.Attribute("TexturePath").Value;
      Texture2D tilesetTexture = content.Load<Texture2D>(texturePath);

      var tileset = new Tileset(tilesetTexture, int.Parse(root.Attribute("TileWidth").Value), int.Parse(root.Attribute("TileHeight").Value));

      tileset.name = root.Attribute("Name").Value;

      IEnumerable<XElement> brushes = root.Elements("Tile");

      foreach (var brush in brushes)
      {
      }

      return tileset;
    }

    private static string WriteTileset(Tileset tileset)
    {
      XElement root = new XElement("Tileset",
        new XAttribute("Name", tileset.name),
        new XAttribute("TileWidth", tileset.TileWidth),
        new XAttribute("TileHeight", tileset.TileHeight),
        new XAttribute("TexturePath", tileset.Texture.Name)
        );
      XElement tilesElement = new XElement("Tiles");

      foreach (var brush in tileset.Brushes)
      {
        var tileElement = new XElement("Tile",
          new XAttribute("ID", brush.id),
        new XAttribute("Name", brush.Name),
        new XAttribute("Type", brush.GetType().Name)

        );

        if (brush.HasInputs)
        {
          var s = string.Join(", ", brush.Inputs.Select(i => i.id));

          tileElement.Add(s);
        }

        root.Add(tileElement);
      }

      root.Save(tileset.name + ".ats");

      return root.ToString();
    }
    */
  }
}