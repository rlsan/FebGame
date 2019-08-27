using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FebEngine.Utility;

namespace FebEngine.Tiles
{
  public class MapGroup
  {
    public List<Tilemap> tilemaps = new List<Tilemap>();

    public int scale = 6;

    public Tilemap currentMap;

    public Tilemap AddMap(Tilemap tilemap)
    {
      //var tilemap = new Tilemap(width, height);
      tilemaps.Add(tilemap);

      return tilemap;
    }

    /// <summary>
    /// Changes the current map to one of the given name.
    /// </summary>
    /// <param name="name"></param>
    public void ChangeMap(string name)
    {
      foreach (var map in tilemaps)
      {
        if (map.Name == name)
        {
          currentMap = map;

          break;
        }
      }
    }

    /// <summary>
    /// Changes the current map to one of the given index.
    /// </summary>
    /// <param name="id"></param>
    public void ChangeMap(int id)
    {
      currentMap = tilemaps[id];
    }

    public void Draw(SpriteBatch sb)
    {
      foreach (var tilemap in tilemaps)
      {
        Debug.DrawRect(new Rectangle(
          (int)tilemap.Position.X * scale,
          (int)tilemap.Position.Y * scale,
          tilemap.Width * scale,
          tilemap.Height * scale),
          Color.Gray);

        for (int x = 0; x < tilemap.GetLayer(1).tileArray.GetLength(0); x++)
        {
          for (int y = 0; y < tilemap.GetLayer(1).tileArray.GetLength(1); y++)
          {
            var t = tilemap.GetLayer(1).tileArray[x, y];

            if (t.Brush.id != -1)
            {
              Debug.DrawRect(new Rectangle(((int)tilemap.Position.X * scale) + (x * scale), ((int)tilemap.Position.Y * scale) + (y * scale), scale, scale));
            }
          }
        }

        Debug.Text(tilemap.Name, new Vector2(tilemap.Position.X * scale, tilemap.Position.Y * scale));
      }
    }
  }
}