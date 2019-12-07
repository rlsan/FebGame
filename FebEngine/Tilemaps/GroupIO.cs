using FebEngine.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.IO.Compression;

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
      string localPath = currentDir + "\\Data\\Chapters\\" + path + ".amg";

      var group = new MapGroup();

      var documentString = File.ReadAllText(localPath);
      var uncompressed = Unzip(documentString);

      //var root = XElement.Load(path);
      var root = XDocument.Parse(uncompressed).Root;

      group.name = new StringBuilder(root.Attribute("Name").Value);

      foreach (var mapElement in root.Elements("Map"))
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
        new XAttribute("Name", group.name)
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

      Console.WriteLine("Saved group to " + Directory.GetCurrentDirectory() + "\\" + group.name + ".amg");

      string compressed = Zip(root.ToString());

      var sb = new StringBuilder();

      foreach (var item in compressed)
      {
        sb.Append(item.ToString());
      }

      return compressed;
    }

    public static void CopyTo(Stream src, Stream dest)
    {
      byte[] bytes = new byte[4096];

      int cnt;

      while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
      {
        dest.Write(bytes, 0, cnt);
      }
    }

    public static string Zip(string str)
    {
      var bytes = Encoding.UTF8.GetBytes(str);

      using (var msi = new MemoryStream(bytes))
      using (var mso = new MemoryStream())
      {
        using (var gs = new GZipStream(mso, CompressionMode.Compress))
        {
          CopyTo(msi, gs);
        }

        var outputBytes = mso.ToArray();
        var outputbase64 = Convert.ToBase64String(outputBytes);

        return outputbase64;
      }
    }

    public static string Unzip(string input)
    {
      var bytes = Convert.FromBase64String(input);

      using (var msi = new MemoryStream(bytes))
      using (var mso = new MemoryStream())
      {
        using (var gs = new GZipStream(msi, CompressionMode.Decompress))
        {
          CopyTo(gs, mso);
        }

        return Encoding.UTF8.GetString(mso.ToArray());
      }
    }
  }
}