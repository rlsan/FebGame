using FebEngine.Tiles;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FebEngine
{
  public static class TilesetIO
  {
    public static Tileset Import(string path, ContentManager content)
    {
      string currentDir = Directory.GetCurrentDirectory();
      string localPath = currentDir + "\\" + path;

      XDocument document = XDocument.Load(path);

      XElement root = document.Root;

      string texturePath = root.Attribute("TexturePath").Value;
      //Console.WriteLine(RenderManager.instance.Content);
      Texture2D tilesetTexture = content.Load<Texture2D>(texturePath);

      var tileset = new Tileset(tilesetTexture, int.Parse(root.Attribute("TileWidth").Value), int.Parse(root.Attribute("TileHeight").Value));
      //tileset.Texture = tilesetTexture;

      tileset.name = root.Attribute("Name").Value;

      IEnumerable<XElement> tileElements = root.Elements("Tile");

      int tileCount = int.Parse(root.Attribute("TileCount").Value);

      List<int> addedIDs = new List<int>();

      TileBrush[] tilesToAdd = new TileBrush[tileCount];

      // Keep looping through the tiles until all tiles have been added.
      int iterations = 0;
      int maxBailout = 10000;
      while (addedIDs.Count < tileCount && iterations <= maxBailout)
      {
        for (int i = 0; i < tileCount; i++)
        {
          var tileElement = tileElements.ElementAt(i);

          // Retrieve the tile element's properties.
          int index = int.Parse(tileElement.Attribute("ID").Value);
          StringBuilder name = new StringBuilder(tileElement.Attribute("Name").Value);
          string tileType = tileElement.Attribute("Type").Value;
          string subtileString = tileElement.Element("Inputs").Value;
          bool isBaseTile = bool.Parse(tileElement.Attribute("Locked").Value);

          // If this tile has already been processed...
          if (addedIDs.Contains(index))
          {
            continue;
          }

          // If the tile has no inputs...
          if (subtileString == string.Empty)
          {
            var t = new TileBrush(name.ToString(), index);
            t.id = index;
            tilesToAdd[i] = t;

            // Record the ID.
            addedIDs.Add(index);

            continue;
          }

          int[] subtileIndexes = Array.ConvertAll(subtileString.Split(','), int.Parse);

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
            var t = new TileBrush();
            if (tileType == "RandomBrush")
            {
              t = new RandomBrush(name.ToString());
              t.Name = name;
              t.id = index;

              foreach (var subtileIndex in subtileIndexes)
              {
                t.AddInput(tilesToAdd[subtileIndex]);
              }
            }
            if (tileType == "AnimatedBrush")
            {
              t = new AnimatedBrush(12, true);
              t.Name = name;
              t.id = index;

              foreach (var subtileIndex in subtileIndexes)
              {
                t.AddInput(tilesToAdd[subtileIndex]);
              }
            }
            else if (tileType == "RowBrush")
            {
              t = new RowBrush(name.ToString(),
                tilesToAdd[subtileIndexes[0]],
                tilesToAdd[subtileIndexes[1]],
                tilesToAdd[subtileIndexes[2]],
                tilesToAdd[subtileIndexes[3]]);

              t.Name = name;
              t.id = index;
            }

            t.isLocked = isBaseTile;
            tilesToAdd[i] = t;

            // Record the ID.
            addedIDs.Add(index);
          }
        }

        iterations++;
      }

      for (int i = 0; i < tilesToAdd.Length; i++)
      {
        tileset.AddBrush(tilesToAdd[i]);
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
        new XAttribute("Type", brush.GetType().Name),
        new XAttribute("Locked", brush.isLocked)
        );

        var inputs = new XElement("Inputs");
        var properties = new XElement("Properties");

        if (brush.HasInputs)
        {
          var inputList = string.Join(", ", brush.Inputs.Select(i => i.id));
          inputs.Add(inputList);
        }

        var propertyList = string.Join(", ", brush.Properties.Select(i => i));
        properties.Add(propertyList);

        tileElement.Add(inputs);
        tileElement.Add(properties);

        root.Add(tileElement);
      }

      //root.Save(tileset.name + ".ats");

      return root.ToString();
    }
  }
}