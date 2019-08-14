using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FebEngine.Tiles
{
  public class TileSet
  {
    public string name = "Tileset";
    public Texture2D Texture { get; }

    private Tile[] rawTiles;
    public List<Tile> TilePalette { get; }

    public int TileWidth { get; }

    public int TileHeight { get; }

    public readonly int rows;
    public readonly int columns;

    public int Tiles { get { return TilePalette.Count; } }

    public TileSet(Texture2D texture, int tileWidth, int tileHeight)
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
        AddTile(new Tile { id = i, frame = i });
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
      if (index <= Tiles)
      {
        return TilePalette[index];
      }
      else
      {
        return TilePalette[Tiles - 1];
      }
    }

    public Tile AddTile(Tile tile, string name = null)
    {
      var t = tile;
      t.id = Tiles;

      System.Console.WriteLine();

      if (name != null)
      {
        t.Name = name;
      }
      else
      {
        t.Name = t.GetType().Name + Tiles;
      }

      TilePalette.Add(t);

      return t;
    }
  }
}