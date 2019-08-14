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
    public Tilemap tilemap;

    public TileSet tileset;

    public TilemapXML(Tilemap tilemap, TileSet tileset)
    {
      this.tilemap = tilemap;
      this.tileset = tileset;
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

    public void WriteTileset()
    {
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

      Console.WriteLine(root);

      root.Save(tileset.name + ".ats");
    }

    public void WriteTilemap()
    {
      XElement root = new XElement("Tilemap",
        new XAttribute("Name", tilemap.name),
        new XAttribute("Width", tilemap.width),
        new XAttribute("Height", tilemap.height),
        new XAttribute("TileWidth", tilemap.tileWidth),
        new XAttribute("TileHeight", tilemap.tileHeight)
        );

      for (int i = 0; i < tilemap.LayerCount; i++)
      {
        var layer = tilemap.Layers[i];

        string indexString = "";

        for (int tileIndex = 0; tileIndex < layer.tileArray.Length; tileIndex++)
        {
          int tileX = tileIndex % tilemap.width;
          int tileY = tileIndex / tilemap.width;
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

      Console.WriteLine(root);

      root.Save(tilemap.name + ".atm");
    }
  }
}