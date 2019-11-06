using FebEngine.Tiles;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FebEngine.Tilemaps
{
  public static class TilesetIO
  {
    public static Tileset Import(string path)
    {
      string currentDir = Directory.GetCurrentDirectory();
      string localPath = currentDir + "\\" + path;

      XDocument document = XDocument.Load(path);

      XElement root = document.Root;

      string texturePath = root.Attribute("TexturePath").Value;
      Texture2D tilesetTexture = RenderManager.instance.Content.Load<Texture2D>(texturePath);

      var tileset = new Tileset(tilesetTexture, int.Parse(root.Attribute("TileWidth").Value), int.Parse(root.Attribute("TileHeight").Value));

      tileset.name = root.Attribute("Name").Value;

      IEnumerable<XElement> tileElements = root.Elements("Tile");

      int tileCount = int.Parse(root.Attribute("TileCount").Value);

      List<int> addedIDs = new List<int>();

      // Keep looping through the tiles until all tiles have been added.
      while (addedIDs.Count < tileCount)
      {
        foreach (var tileElement in tileElements)
        {
          int index = int.Parse(tileElement.Attribute("ID").Value);
          string name = tileElement.Attribute("Name").Value;
          string tileType = tileElement.Attribute("Type").Value;

          // If this tile has already been processed...
          if (addedIDs.Contains(index))
          {
            continue;
          }

          // If the tile has no inputs...
          if (tileElement.Value == string.Empty)
          {
            var t = new TileBrush(name, index);
            t.id = index;
            tileset.AddBrush(t);

            // Record the ID.
            addedIDs.Add(index);

            continue;
          }

          int[] subtileIndexes = Array.ConvertAll(tileElement.Value.Split(','), int.Parse);

          // Loop through the subtiles but stop if it finds a subtile that hasn't been added yet.
          bool works = true;
          foreach (var subtileIndex in subtileIndexes)
          {
            // If the ID has already been processed...
            if (addedIDs.Contains(subtileIndex))
            {
              continue;
            }
            else
            {
              works = false;
              break;
            }
          }

          // If all its subtiles already exist, create the tile.
          if (works)
          {
            if (tileType == "RandomBrush")
            {
              var t = new RandomBrush(name);
              t.id = index;

              foreach (var subtileIndex in subtileIndexes)
              {
                t.AddInput(tileset.GetBrushFromIndex(subtileIndex));
              }

              tileset.AddBrush(t);
            }
            else if (tileType == "RowBrush")
            {
              var t = new RowBrush(name,
                tileset.GetBrushFromIndex(subtileIndexes[0]),
                tileset.GetBrushFromIndex(subtileIndexes[1]),
                tileset.GetBrushFromIndex(subtileIndexes[2]),
                tileset.GetBrushFromIndex(subtileIndexes[3]));

              t.id = index;

              tileset.AddBrush(t);
            }

            // Record the ID.
            addedIDs.Add(index);
          }
        }
      }

      return tileset;
    }

    public static string Export(Tileset tileset)
    {
      XElement root = new XElement("Tileset",
        new XAttribute("Name", tileset.name),
        new XAttribute("TileCount", tileset.BrushCount),
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
  }
}