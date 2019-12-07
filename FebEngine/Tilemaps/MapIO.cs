using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace FebEngine
{
  public static class MapIO
  {
    public static Tilemap Import(XElement map)
    {
      // Retrieve its root element.
      var root = map;

      // Create a new tilemap using the root element's attributes.
      var tilemap = new Tilemap(
        int.Parse(root.Attribute("Width").Value),
        int.Parse(root.Attribute("Height").Value),
        int.Parse(root.Attribute("TileWidth").Value),
        int.Parse(root.Attribute("TileHeight").Value)
        );

      //Console.WriteLine(root.Attribute("Tileset").Value);

      tilemap.name = new StringBuilder(root.Attribute("Name").Value);
      tilemap.X = int.Parse(root.Attribute("X").Value);
      tilemap.Y = int.Parse(root.Attribute("Y").Value);

      // Iterate through each warp of the document.
      foreach (var warpElement in root.Element("Warps").Elements("SideWarp"))
      {
        var w = new SideWarp(
          warpElement.Attribute("Dest").Value,
          int.Parse(warpElement.Attribute("Min").Value),
          int.Parse(warpElement.Attribute("Max").Value),
          (WarpDirection)Enum.Parse(typeof(WarpDirection), warpElement.Attribute("Dir").Value)
          );
        tilemap.sideWarps.Add(w);
      }

      // Iterate through each object of the object list.

      foreach (var objectElement in root.Element("Objects").Elements("Object"))
      {
        var x = float.Parse(objectElement.Attribute("X").Value);
        var y = float.Parse(objectElement.Attribute("Y").Value);
        string objectName = objectElement.Attribute("Name").Value;

        var position = new Vector2(x, y);

        tilemap.ObjectLayer.Add(position, objectName);
      }

      // Iterate through each layer of the document.
      foreach (var layer in root.Elements("Layer"))
      {
        string layerName = layer.Attribute("Name").Value;

        var addedLayer = tilemap.AddLayer(layerName);

        string unsplitIndices = layer.Element("Tiles").Value;

        string[] indexes = unsplitIndices.Split(',');

        for (int i = 0; i < addedLayer.Tiles.Count; i++)
        {
          addedLayer.Tiles.Insert(i, int.Parse(indexes[i]));
        }
      }

      return tilemap;
    }

    public static XElement Export(Tilemap tilemapToExport)
    {
      // Create the root element.
      XElement root = new XElement("Map",
        new XAttribute("Name", tilemapToExport.name),
        new XAttribute("X", tilemapToExport.X),
        new XAttribute("Y", tilemapToExport.Y),
        new XAttribute("Width", tilemapToExport.Width),
        new XAttribute("Height", tilemapToExport.Height),
        //new XAttribute("Tileset", tilemapToExport.Tileset.name),
        new XAttribute("TileWidth", tilemapToExport.TileWidth),
        new XAttribute("TileHeight", tilemapToExport.TileHeight)
        );

      //Console.WriteLine(tilemapToExport.Tileset.name);

      var warpElement = new XElement("Warps");

      foreach (var sideWarp in tilemapToExport.sideWarps)
      {
        warpElement.Add(new XElement("SideWarp",
          new XAttribute("Dest", sideWarp.DestinationMapName),
          new XAttribute("Dir", sideWarp.Direction),
          new XAttribute("Min", sideWarp.RangeMin),
          new XAttribute("Max", sideWarp.RangeMax)
          ));
      }

      root.Add(warpElement);

      var objectsElement = new XElement("Objects");

      foreach (var gameObject in tilemapToExport.ObjectLayer.Objects)
      {
        objectsElement.Add(new XElement("Object",
          new XAttribute("Name", gameObject.name),
          new XAttribute("X", gameObject.position.X),
          new XAttribute("Y", gameObject.position.Y)
          ));
      }

      root.Add(objectsElement);

      // Iterate through each layer of the map.
      foreach (var layer in tilemapToExport.Layers)
      {
        // Create an element for this layer.
        var layerElement = new XElement("Layer", new XAttribute("Name", layer.Name));

        // Create a comma-delimited list of all tile IDs in the layer.
        var indexes = string.Join(", ", from item in layer.Tiles select item);

        // Add the index list to the layer element.
        layerElement.Add(new XElement("Tiles", indexes));

        // Add the layer element to the root.
        root.Add(layerElement);
      }

      return root;
    }
  }
}