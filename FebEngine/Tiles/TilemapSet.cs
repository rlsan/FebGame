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
  public class TilemapSet
  {
    public List<Tilemap> tilemaps = new List<Tilemap>();

    public int scale = 6;

    public void Add(Tilemap tilemap)
    {
      tilemaps.Add(tilemap);
    }

    public void Draw(SpriteBatch sb)
    {
      foreach (var tilemap in tilemaps)
      {
        Debug.DrawRect(new Rectangle(
          tilemap.X * scale,
          tilemap.Y * scale,
          tilemap.width * scale,
          tilemap.height * scale),
          Color.Gray);

        for (int x = 0; x < tilemap.GetLayer(1).tileArray.GetLength(0); x++)
        {
          for (int y = 0; y < tilemap.GetLayer(1).tileArray.GetLength(1); y++)
          {
            var t = tilemap.GetLayer(1).tileArray[x, y];

            if (t.id != -1)
            {
              Debug.DrawRect(new Rectangle((tilemap.X * scale) + (x * scale), (tilemap.Y * scale) + (y * scale), scale, scale));
            }
          }
        }

        Debug.Text(tilemap.name, new Vector2(tilemap.X * scale, tilemap.Y * scale));
      }
    }
  }
}