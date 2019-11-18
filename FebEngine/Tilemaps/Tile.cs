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
    //public TileBrush Brush { get; set; }

    /// <summary>
    /// The ID that the tileset will use to draw.
    /// </summary>
    public int Id { get; set; }

    public Color tint;

    public int Hash { get; set; }

    public Tile(TilemapLayer layer, int x, int y, int id = -1)
    {
      Layer = layer;
      X = x;
      Y = y;

      Id = id;

      tint = Color.White;

      RefreshHash();
      //Hash = RNG.RandInt();
    }

    public void SetBrush(TileBrush brush)
    {
      //Brush = brush;
    }

    public void SetIndex(int id)
    {
      Id = id;
    }

    /// <summary>
    /// Sets the brush to null and removes any properties assigned to the tile.
    /// </summary>
    public void Reset()
    {
      //Brush = null;
      Id = -1;
    }

    public void RefreshHash()
    {
      Hash = RNG.RandInt(X + Y);
    }

    public override string ToString()
    {
      return Id.ToString();
    }
  }
}