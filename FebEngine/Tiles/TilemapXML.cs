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

    public TilemapXML(Tilemap tilemap)
    {
      this.tilemap = tilemap;
    }

    public void Test()
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

      root.Save(tilemap.name + ".xml");
    }
  }
}