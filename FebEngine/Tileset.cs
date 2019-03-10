using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FebEngine
{
  public class Tileset
  {
    public Texture2D Texture { get; }

    private Tile[] rawTiles;
    public List<TileBrush> brushPalette { get; }

    public int TileWidth { get; }

    public int TileHeight { get; }

    private readonly int rows;
    private readonly int columns;

    public int TotalRawTiles { get { return rawTiles.Length; } }
    public int Brushes { get { return brushPalette.Count; } }

    public Tileset(Texture2D texture, int tileWidth, int tileHeight)
    {
      Texture = texture;

      TileWidth = tileWidth;
      TileHeight = tileHeight;

      rows = Texture.Width / TileWidth;
      columns = Texture.Height / tileHeight;

      rawTiles = new Tile[rows * columns];
      brushPalette = new List<TileBrush>();

      for (int i = 0; i < rawTiles.Length; i++)
      {
        //rawTiles[i].Reset();
        AddBrush(new int[] { i });
      }
    }

    public Vector2 GetTileFromIndex(int index)
    {
      int tileX = index % rows;
      int tileY = index / rows;

      return new Vector2(tileX, tileY);
    }

    public TileBrush AddBrush(int[] frames)
    {
      var tb = new TileBrush
      {
        id = Brushes + 0,
        frames = frames,
        properties = new TileType[] { TileType.Solid },
      };

      brushPalette.Add(tb);

      return tb;
    }

    public TileBrush AddBrush(int frame)
    {
      var tb = new TileBrush
      {
        id = Brushes + 0,
        frames = new int[] { frame },
        properties = new TileType[] { TileType.Solid },
      };

      brushPalette.Add(tb);

      return tb;
    }

    public TileBrush AddBrush(int[] frames, TileType[] properties)
    {
      var tb = new TileBrush
      {
        id = Brushes + 0,
        frames = frames,
        properties = properties,
      };

      brushPalette.Add(tb);

      return tb;
    }

    public TileBrush AddBrush(int frame, TileType property)
    {
      var tb = new TileBrush
      {
        id = Brushes + 0,
        frames = new int[] { frame },
        properties = new TileType[] { property },
      };

      brushPalette.Add(tb);

      return tb;
    }

    public TileBrush AddBrush(int[] frames, TileType property)
    {
      var tb = new TileBrush
      {
        id = Brushes + 1,
        frames = frames,
        properties = new TileType[] { property },
      };

      brushPalette.Add(tb);

      return tb;
    }
  }
}