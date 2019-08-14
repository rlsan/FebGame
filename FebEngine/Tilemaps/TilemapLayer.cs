using Microsoft.Xna.Framework;
using System.Collections.Generic;
using FebEngine.Utility;

namespace FebEngine.Tiles
{
  public class TilemapLayer
  {
    public Tilemap Tilemap { get; }
    public string Name { get; }

    public int X { get; set; }
    public int Y { get; set; }

    public Dictionary<int, Tile> tiles;
    public Tile[,] tileArray;
    public int[,] hashArray;

    public Color tint = Color.White;

    public bool IsVisible { get; set; }
    public bool IsDirty { get; set; }

    public TilemapLayer(Tilemap tilemap, string name)
    {
      Tilemap = tilemap;
      Name = name;

      tiles = new Dictionary<int, Tile>();
      tileArray = new Tile[Tilemap.width, Tilemap.height];
      hashArray = new int[Tilemap.width, Tilemap.height];

      IsVisible = true;

      //Populate the tile array

      for (int x = 0; x < tileArray.GetLength(0); x++)
      {
        for (int y = 0; y < tileArray.GetLength(1); y++)
        {
          var t = new Tile
          {
            properties = new TileType[] { TileType.None },
            X = x,
            Y = y,
          };

          tileArray[x, y] = t;
          hashArray[x, y] = RNG.RandIntRange(0, 99);
        }
      }
    }

    public void PutTile(Tile tile, int x, int y)
    {
      if (x > Tilemap.width - 1 || x < 0 || y > Tilemap.height - 1 || y < 0)
      {
        return;
      }

      hashArray[x, y] = RNG.RandIntRange(0, 99);

      if (tileArray[x, y].id == tile.id)
      {
        return;
      }

      //Tile t = tile;
      Tile t = GetTileXY(x, y);
      //t.X = x;
      //t.Y = y;

      t.id = tile.id;

      tileArray[x, y] = t;
      IsDirty = true;
    }

    public void EraseTile(int x, int y)
    {
      if (x > Tilemap.width - 1 || x < 0 || y > Tilemap.height - 1 || y < 0)
      {
        return;
      }

      tileArray[x, y] = new Tile();

      IsDirty = true;
    }

    public void Clear()
    {
      for (int x = 0; x < tileArray.GetLength(0); x++)
      {
        for (int y = 0; y < tileArray.GetLength(1); y++)
        {
          tileArray[x, y] = new Tile();
        }
      }

      IsDirty = true;
    }

    public Tile GetTile(Vector2 position)
    {
      Tile t = new Tile();

      int x = (int)position.X / 16;
      int y = (int)position.Y / 16;

      if (x > Tilemap.width - 1 || x < 0 || y > Tilemap.height - 1 || y < 0)
      {
        return t;
      }

      return tileArray[x, y];
    }

    public Tile GetTileXY(int x, int y)
    {
      Tile t = new Tile
      {
        id = -1
      };

      if (x > Tilemap.width - 1 || x < 0 || y > Tilemap.height - 1 || y < 0)
      {
        return t;
      }

      return tileArray[x, y];
    }

    public int GetTileIndexXY(int x, int y)
    {
      if (x > Tilemap.width - 1 || x < 0 || y > Tilemap.height - 1 || y < 0)
      {
        return -1;
      }

      return tileArray[x, y].id;
    }

    public void ShowTileIndices(bool showEmptyTiles = false)
    {
      for (int x = 0; x < tileArray.GetLength(0); x++)
      {
        for (int y = 0; y < tileArray.GetLength(1); y++)
        {
          Tile tile = tileArray[x, y];

          if (showEmptyTiles || tile.id >= 0)
          {
            //EUREKA
            Debug.Text(tile.id.ToString(), x * Tilemap.tileWidth, y * Tilemap.tileHeight);
            //Debug.Text(hashArray[x, y], x * Tilemap.tileWidth, y * Tilemap.tileHeight);
          }
        }
      }
    }

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
  }
}