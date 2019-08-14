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
    public TileSet ts;

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
        int.Parse(root.Attribute("Width").Value),
        int.Parse(root.Attribute("Height").Value),
        int.Parse(root.Attribute("TileWidth").Value),
        int.Parse(root.Attribute("TileHeight").Value)
        );

      tilemap.name = root.Attribute("Name").Value;

      tilemap.tileset = ts;

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

          Tile t = new Tile();

          addedLayer.tileArray[i % tilemap.width, i / tilemap.height].id = index;
        }
      }

      return tilemap;
    }

    public string WriteTilemap(Tilemap tilemapToExport)
    {
      XElement root = new XElement("Tilemap",
        new XAttribute("Name", tilemapToExport.name),
        new XAttribute("Width", tilemapToExport.width),
        new XAttribute("Height", tilemapToExport.height),
        new XAttribute("TileWidth", tilemapToExport.tileWidth),
        new XAttribute("TileHeight", tilemapToExport.tileHeight)
        );

      for (int i = 0; i < tilemapToExport.LayerCount; i++)
      {
        var layer = tilemapToExport.Layers[i];

        string indexString = "";

        for (int tileIndex = 0; tileIndex < layer.tileArray.Length; tileIndex++)
        {
          int tileX = tileIndex % tilemapToExport.width;
          int tileY = tileIndex / tilemapToExport.width;
          var tile = layer.tileArray[tileX, tileY];

          indexString += tile.id;

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

      root.Save(tilemapToExport.name + ".atm");

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

    public void printNode(Tile tile, XElement container, bool first = true)
    {
      XElement newContainer;

      if (!first)
      {
        newContainer = new XElement("Tile",
          new XAttribute("Name", tile.Name),
          new XAttribute("Type", tile.GetType().Name),
          new XAttribute("ID", tile.id));

        container.Add(newContainer);
      }
      else
      {
        newContainer = container;
      }

      for (int i = 0; i < tile.children.Length; i++)
      {
        printNode(tile.children[i], newContainer, false);
      }
    }
  }
}