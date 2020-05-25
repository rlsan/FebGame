using System.Collections.Generic;
using Fubar.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fubar
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
    }

    public Vector2 GetTilePositionFromIndex(int index)
    {
      int tileX = index % Rows;
      int tileY = index / Rows;

      return new Vector2(tileX, tileY);
    }

    public TileBrush GetBrushFromIndex(int index)
    {
      foreach (var brush in Brushes)
      {
        if (brush.id == index) return brush;
      }

      return Brushes[BrushCount - 1];
    }

    public TileBrush GetBrushFromName(string name)
    {
      foreach (var brush in Brushes)
      {
        if (brush.Name.ToString() == name) return brush;
      }

      return Brushes[BrushCount - 1];
    }

    public TileBrush AddBrush(TileBrush brush, string name = null, bool isHidden = false)
    {
      brush.tileset = this;
      brush.id = Brushes.Count;
      brush.isHidden = isHidden;

      Brushes.Add(brush);

      return brush;
    }

    public void RemoveBrush(int id)
    {
      foreach (var item in Brushes.ToArray())
      {
        if (item.id == id)
        {
          Brushes.Remove(item);
        }
      }
    }
  }
}