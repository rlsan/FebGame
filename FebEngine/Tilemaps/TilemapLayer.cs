using Microsoft.Xna.Framework;
using System.Collections.Generic;
using FebEngine.Utility;

namespace FebEngine.Tiles
{
  public class TilemapLayer
  {
    /// <summary>
    /// The map this layer belongs to.
    /// </summary>
    public Tilemap Tilemap { get; }

    public string Name { get; }

    public int X { get; set; }
    public int Y { get; set; }

    public Tile[,] tileArray; // Make this private.

    public bool IsVisible { get; set; }

    public TilemapLayer(Tilemap tilemap, string name)
    {
      Tilemap = tilemap;
      Name = name;

      tileArray = new Tile[Tilemap.Width, Tilemap.Height];

      IsVisible = true;

      //Populate the tile array

      for (int x = 0; x < tileArray.GetLength(0); x++)
      {
        for (int y = 0; y < tileArray.GetLength(1); y++)
        {
          tileArray[x, y] = new Tile(this, x, y);
        }
      }
    }

    public void PutTile(TileBrush brush, int x, int y)
    {
      // Check if the position is outside the bounds of the map.
      if (x > Tilemap.Width - 1 || x < 0 || y > Tilemap.Height - 1 || y < 0)
      {
        return;
      }

      tileArray[x, y].SetBrush(brush);
    }

    public void EraseTile(int x, int y)
    {
      // Check if the position is outside the bounds of the map.
      if (x > Tilemap.Width - 1 || x < 0 || y > Tilemap.Height - 1 || y < 0)
      {
        return;
      }

      tileArray[x, y].Reset();
    }

    public void Clear()
    {
      for (int x = 0; x < tileArray.GetLength(0); x++)
      {
        for (int y = 0; y < tileArray.GetLength(1); y++)
        {
          tileArray[x, y].Reset();
        }
      }
    }

    public Tile GetTile(Vector2 position)
    {
      int x = (int)position.X / Tilemap.TileWidth;
      int y = (int)position.Y / Tilemap.TileHeight;

      if (x > Tilemap.Width - 1 || x < 0 || y > Tilemap.Height - 1 || y < 0)
      {
        return null;
      }

      return tileArray[x, y];
    }

    public Tile GetTile(int x, int y)
    {
      if (x > Tilemap.Width - 1 || x < 0 || y > Tilemap.Height - 1 || y < 0)
      {
        return null;
      }

      return tileArray[x, y];
    }

    public int GetTileIndexXY(int x, int y)
    {
      if (x > Tilemap.Width - 1 || x < 0 || y > Tilemap.Height - 1 || y < 0)
      {
        return -1;
      }
      if (tileArray[x, y].Brush != null)
      {
        return tileArray[x, y].Brush.id;
      }

      return -1;
    }

    public void ShowTileIndices(bool showEmptyTiles = false)
    {
      for (int x = 0; x < tileArray.GetLength(0); x++)
      {
        for (int y = 0; y < tileArray.GetLength(1); y++)
        {
          Tile tile = tileArray[x, y];

          if (showEmptyTiles || tile.Brush.id >= 0)
          {
            Debug.Text(tile.Brush.id.ToString(), x * Tilemap.Tileset.TileWidth, y * Tilemap.Tileset.TileHeight);
            //Debug.Text(hashArray[x, y], x * Tilemap.tileWidth, y * Tilemap.tileHeight);
          }
        }
      }
    }

    /*
    public void RandomizeTiles()
    {
      for (int x = 0; x < tileArray.GetLength(0); x++)
      {
        for (int y = 0; y < tileArray.GetLength(1); y++)
        {
          if (hashArray[x, y] % 2 == 0)
          {
            PutTile(new Tile { id = 0 }, x, y);
          }
        }
      }
    }
    */
  }
}