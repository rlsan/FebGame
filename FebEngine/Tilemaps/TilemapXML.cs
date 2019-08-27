using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace FebEngine.Tiles
{
  public class TilemapXML
  {
    public Tileset ts;

    public string Export(Tilemap tilemapToExport)
    {
      return WriteTilemap(tilemapToExport);
    }

    public Tilemap Import(string tilemapToImport)
    {
      return ReadTilemap(tilemapToImport);
    }

    public Tilemap ReadTilemap(string document)
    {
      XDocument parsedTilemap = XDocument.Parse(document);

      XElement root = parsedTilemap.Root;

      Tilemap tilemap = new Tilemap(
        ts,
        int.Parse(root.Attribute("Width").Value),
        int.Parse(root.Attribute("Height").Value)
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

          addedLayer.tileArray[i % tilemap.Width, i / tilemap.Height].Brush.id = index;
        }
      }

      return tilemap;
    }

    public string WriteTilemap(Tilemap tilemapToExport)
    {
      XElement root = new XElement("Tilemap",
        new XAttribute("Name", tilemapToExport.Name),
        new XAttribute("Width", tilemapToExport.Width),
        new XAttribute("Height", tilemapToExport.Height),
        new XAttribute("TileWidth", tilemapToExport.Tileset.TileWidth),
        new XAttribute("TileHeight", tilemapToExport.Tileset.TileHeight)
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

      //Console.WriteLine(root);

      root.Save(tilemapToExport.Name + ".atm");

      return root.ToString();
    }

    public void WriteTileset()
    {
      /*
      if (tileset == null) return;

      XElement root = new XElement("Tileset",
        new XAttribute("Name", tileset.name),
        new XAttribute("TileWidth", tileset.TileWidth),
        new XAttribute("TileHeight", tileset.TileHeight)
        );
      XElement tilesElement = new XElement("Tiles");

      for (int i = 0; i < tileset.TilePalette.Count; i++)
      {
        var tile = tileset.TilePalette[i];

        var tileElement = new XElement("Tile",
        new XAttribute("Name", tile.Name),
        new XAttribute("Type", tile.GetType().Name),
        new XAttribute("ID", tile.id),
        new XAttribute("Properties", tile.properties[0]));

        if (tile.children.Length > 0)
        {
          printNode(tile, tileElement);
        }

        root.Add(tileElement);
        //Console.WriteLine();
      }

      //Console.WriteLine(root);

      root.Save(tileset.name + ".ats");
      */
    }

    public void printNode(TileBrush brush, XElement container, bool first = true)
    {
      XElement newContainer;

      if (!first)
      {
        newContainer = new XElement("Tile",
          new XAttribute("Name", brush.Name),
          new XAttribute("Type", brush.GetType().Name),
          new XAttribute("ID", brush.FrameId));

        container.Add(newContainer);
      }
      else
      {
        newContainer = container;
      }

      for (int i = 0; i < brush.Children.Length; i++)
      {
        printNode(brush.Children[i], newContainer, false);
      }
    }
  }
}