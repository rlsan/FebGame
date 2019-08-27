using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FebEngine.Tiles
{
  public class Tileset
  {
    public string name = "Tileset";
    public Texture2D Texture { get; }

    private TileBrush[] rawTiles;
    public List<TileBrush> TileBrushes { get; }

    public int TileWidth { get; }

    public int TileHeight { get; }

    public readonly int rows;
    public readonly int columns;

    public int SwatchCount { get { return TileBrushes.Count; } }

    public Tileset(Texture2D texture, int tileWidth, int tileHeight)
    {
      Texture = texture;

      TileWidth = tileWidth;
      TileHeight = tileHeight;

      rows = Texture.Width / TileWidth;
      columns = Texture.Height / tileHeight;

      rawTiles = new TileBrush[rows * columns];
      TileBrushes = new List<TileBrush>();

      for (int i = 0; i < rawTiles.Length; i++)
      {
        AddBrush(new TileBrush("Tile" + i, i));
      }
    }

    public Vector2 GetTilePositionFromIndex(int index)
    {
      int tileX = index % rows;
      int tileY = index / rows;

      return new Vector2(tileX, tileY);
    }

    public TileBrush GetBrushFromIndex(int index)
    {
      if (index < SwatchCount)
      {
        return TileBrushes[index];
      }
      else
      {
        return TileBrushes[SwatchCount - 1];
      }
    }

    public TileBrush AddBrush(TileBrush brush, string name = null)
    {
      brush.id = TileBrushes.Count;

      TileBrushes.Add(brush);

      return brush;
    }
  }
}