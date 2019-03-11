using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FebEngine.Tiles
{
  public class Tileset
  {
    public Texture2D Texture { get; }

    private Tile[] rawTiles;
    public List<Tile> TilePalette { get; }

    public int TileWidth { get; }

    public int TileHeight { get; }

    private readonly int rows;
    private readonly int columns;

    public int Tiles { get { return TilePalette.Count; } }

    public Tileset(Texture2D texture, int tileWidth, int tileHeight)
    {
      Texture = texture;

      TileWidth = tileWidth;
      TileHeight = tileHeight;

      rows = Texture.Width / TileWidth;
      columns = Texture.Height / tileHeight;

      rawTiles = new Tile[rows * columns];
      TilePalette = new List<Tile>();

      for (int i = 0; i < rawTiles.Length; i++)
      {
        AddTile(new Tile { id = i });
      }
    }

    public Vector2 GetTilePositionFromIndex(int index)
    {
      int tileX = index % rows;
      int tileY = index / rows;

      return new Vector2(tileX, tileY);
    }

    public Tile GetTileFromIndex(int index)
    {
      return TilePalette[index];
    }

    public Tile AddTile(Tile tile)
    {
      var t = tile;
      t.id = Tiles + 0;

      TilePalette.Add(t);

      return t;
    }
  }
}