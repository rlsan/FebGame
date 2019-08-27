using Microsoft.Xna.Framework;
using System;

namespace FebEngine.Tiles
{
  public class Tile
  {
    /// <summary>
    /// The layer this tile belongs to.
    /// </summary>
    public TilemapLayer Layer { get; }

    public int X { get; }
    public int Y { get; }

    /// <summary>
    /// The brush that defines how this tile will be drawn.
    /// </summary>
    public TileBrush Brush { get; set; }

    /// <summary>
    /// Properties for this tile.
    /// </summary>
    public TileProperties Properties { get; set; }

    public Color tint = Color.White;

    public Tile(TilemapLayer layer, int x, int y)
    {
      Layer = layer;
      X = x;
      Y = y;
    }

    public void SetBrush(TileBrush brush)
    {
      Brush = brush;
    }

    /// <summary>
    /// Sets the brush to null and removes any properties assigned to the tile.
    /// </summary>
    public void Reset()
    {
      Brush = null;
      Properties.Clear();
    }

    public struct TileProperties
    {
      private TileType[] properties;

      public void Clear()
      {
        Array.Clear(properties, 0, 0);
      }

      public override string ToString()
      {
        string s = "null";

        if (properties != null)
        {
          s = "";

          for (int i = 0; i < properties.Length; i++)
          {
            s += properties[i].ToString();
            if (i < properties.Length - 1) s += ", ";
          }
        }
        return s;
      }
    }
  }
}