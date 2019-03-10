using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebEngine
{
  public class TilemapLayer
  {
    private Tilemap Tilemap { get; }
    public string Name { get; }

    public Dictionary<int, Tile> tiles;
    public Tile[,] tileArray;

    public TilemapLayer(Tilemap tilemap, string Name)
    {
      this.Tilemap = tilemap;
      this.Name = Name;

      tiles = new Dictionary<int, Tile>();
      tileArray = new Tile[this.Tilemap.width, this.Tilemap.height];

      //Populate the tile array

      for (int i = 0; i < tileArray.GetLength(0); i++)
      {
        for (int j = 0; j < tileArray.GetLength(1); j++)
        {
          var t = new Tile
          {
            id = -1,
            frames = new int[] { -1 },
            properties = new TileType[] { TileType.None },
          };

          tileArray[i, j] = t;
        }
      }
    }

    public void PutTile(TileBrush brush, int x, int y)
    {
      if (x > Tilemap.width - 1 || x < 0 || y > Tilemap.height - 1 || y < 0)
      {
        return;
      }

      tileArray[x, y].id = brush.id;
      tileArray[x, y].frames = brush.frames;
      tileArray[x, y].properties = brush.properties;
    }

    public void EraseTile(int x, int y)
    {
      if (x > Tilemap.width - 1 || x < 0 || y > Tilemap.height - 1 || y < 0)
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
          tileArray[x, y].id = -1;
        }
      }
    }

    public Tile GetTile(Vector2 position)
    {
      Tile t = new Tile
      {
        id = -1
      };

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

    public void ShowTileIndices()
    {
      for (int x = 0; x < tileArray.GetLength(0); x++)
      {
        for (int y = 0; y < tileArray.GetLength(1); y++)
        {
          Tile tile = tileArray[x, y];
          if (tile.id >= 0)
          {
            Debug.Text(tile.id.ToString(), x * Tilemap.tileWidth, y * Tilemap.tileHeight);
          }
        }
      }
    }
  }
}