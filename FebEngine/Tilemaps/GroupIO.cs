using FebEngine.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace FebEngine
{
  public static class GroupIO
  {
    /// <summary>
    /// Imports the map group.
    /// </summary>
    public static List<Tilemap> Import(string path)
    {
      string currentDir = Directory.GetCurrentDirectory();
      string localPath = currentDir + "\\" + path;

      var group = new MapGroup();

      var document = XElement.Load(path);

      group.Name = new StringBuilder(document.Attribute("Name").Value);

      foreach (var mapElement in document.Elements("Map"))
      {
        string name = mapElement.Attribute("Name").Value;

        Tilemap map = MapIO.Import(mapElement);
        group.AddMap(map);
      }

      return group.Tilemaps;
    }

    /// <summary>
    /// Exports the map group.
    /// </summary>
    public static string Export(MapGroup group)
    {
      XElement root = new XElement("Group",
        new XAttribute("Name", group.Name)
        );

      foreach (var map in group.Tilemaps)
      {
        /*
        var mapElement = new XElement("Map",
        new XAttribute("Name", map.Name)
        );

        root.Add(mapElement);
        */

        var mapElement = MapIO.Export(map);
        root.Add(mapElement);
      }

      //root.Save(group.Name + ".amg");

      Console.WriteLine("Saved group to " + Directory.GetCurrentDirectory() + "\\" + group.Name + ".amg");

      return root.ToString();
    }
  }
}