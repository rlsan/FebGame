using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FebEngine.Tiles
{
  public class Tileset
  {
    public string name = "Tileset";
    public Texture2D Texture { get; }

    public List<TileBrush> Brushes { get; }

    public int TileWidth { get; }
    public int TileHeight { get; }

    public int Rows { get; }
    public int Columns { get; }

    public int BrushCount { get { return Brushes.Count; } }

    public Tileset(Texture2D texture, int tileWidth, int tileHeight)
    {
      Texture = texture;

      TileWidth = tileWidth;
      TileHeight = tileHeight;

      Rows = Texture.Width / TileWidth;
      Columns = Texture.Height / tileHeight;

      Brushes = new List<TileBrush>();

      for (int i = 0; i < Rows * Columns; i++)
      {
        AddBrush(new TileBrush("Tile" + i, i));
      }
    }

    public Vector2 GetTilePositionFromIndex(int index)
    {
      int tileX = index % Rows;
      int tileY = index / Rows;

      return new Vector2(tileX, tileY);
    }

    public TileBrush GetBrushFromIndex(int index)
    {
      if (index < BrushCount)
      {
        return Brushes[index];
      }
      else
      {
        return Brushes[BrushCount - 1];
      }
    }

    public TileBrush AddBrush(TileBrush brush, string name = null, bool isHidden = false)
    {
      brush.tileset = this;
      brush.id = Brushes.Count;
      brush.isHidden = isHidden;

      Brushes.Add(brush);

      return brush;
    }
  }
}