using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FebEngine.Tiles
{
  public static class TilemapXML
  {
    public static ContentManager content;

    public static string ExportMap(Tilemap tilemapToExport)
    {
      return WriteTilemap(tilemapToExport);
    }

    public static Tilemap ImportMap(string tilemapToImport)
    {
      return ReadTilemap(tilemapToImport);
    }

    public static string ExportTileset(Tileset tilesetToExport)
    {
      return WriteTileset(tilesetToExport);
    }

    public static Tileset ImportTileset(string tilesetToImport)
    {
      return ReadTileset(tilesetToImport);
    }

    private static Tilemap ReadTilemap(string document)
    {
      XDocument parsedTilemap = XDocument.Parse(document);

      XElement root = parsedTilemap.Root;

      Tilemap tilemap = new Tilemap(
        int.Parse(root.Attribute("Width").Value),
        int.Parse(root.Attribute("Height").Value),
        int.Parse(root.Attribute("TileWidth").Value),
        int.Parse(root.Attribute("TileHeight").Value)
        );

      tilemap.Name = root.Attribute("Name").Value;

      IEnumerable<XElement> layers = root.Elements("Layer");

      foreach (var layer in layers)
      {
        string layerName = layer.Attribute("Name").Value;

        TilemapLayer addedLayer = tilemap.AddLayer(layerName);

        string unsplitIndices = layer.Element("Tiles").Value;

        string[] indices = unsplitIndices.Split(',');

        for (int i = 0; i < indices.Length; i++)
        {
          int index = int.Parse(indices[i]);

          var brush = addedLayer.tileArray[i % tilemap.Width, i / tilemap.Height].Brush;

          if (brush != null)
          {
            brush.id = index;
          }
          else
          {
          }
        }
      }

      return tilemap;
    }

    private static string WriteTilemap(Tilemap tilemapToExport)
    {
      XElement root = new XElement("Tilemap",
        new XAttribute("Name", tilemapToExport.Name),
        new XAttribute("Width", tilemapToExport.Width),
        new XAttribute("Height", tilemapToExport.Height),
        new XAttribute("TileWidth", tilemapToExport.TileWidth),
        new XAttribute("TileHeight", tilemapToExport.TileHeight)
        );

      for (int i = 0; i < tilemapToExport.LayerCount; i++)
      {
        var layer = tilemapToExport.Layers[i];

        string indexString = "";

        for (int tileIndex = 0; tileIndex < layer.tileArray.Length; tileIndex++)
        {
          int tileX = tileIndex % tilemapToExport.Width;
          int tileY = tileIndex / tilemapToExport.Width;
          var tile = layer.tileArray[tileX, tileY];

          indexString += tile.Brush.id;

          if (tileIndex < layer.tileArray.Length - 1)
          {
            indexString += ",";
          }
        }

        var layerElement = new XElement("Layer",
          new XAttribute("Name", layer.Name));

        layerElement.Add(new XElement("Tiles", indexString));

        root.Add(layerElement);
      }

      root.Save(tilemapToExport.Name + ".atm");

      return root.ToString();
    }

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
  }
}