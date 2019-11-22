using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace FebEngine
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

    public Grid<int> Tiles { get; set; }

    public bool IsVisible { get; set; }

    public TilemapLayer(Tilemap tilemap, string name)
    {
      Tilemap = tilemap;
      Name = name;

      IsVisible = true;

      // Populate the tile array.
      Tiles = new Grid<int>(tilemap.Width, tilemap.Height, -1);
    }

    public void ExtendUp(int amount)
    {
      Tiles.ExtendUp(amount, -1);
    }

    public void ExtendDown(int amount)
    {
      Tiles.ExtendDown(amount, -1);
    }

    public void ExtendLeft(int amount)
    {
      Tiles.ExtendLeft(amount, -1);
    }

    public void ExtendRight(int amount)
    {
      Tiles.ExtendRight(amount, -1);
    }

    public void PutTile(int id, Vector2 position)
    {
      int x = (int)(position.X / Tilemap.TileWidth);
      int y = (int)(position.Y / Tilemap.TileHeight);

      // Check if the position is outside the bounds of the map.
      if (x > Tilemap.Width - 1 || x < 0 || y > Tilemap.Height - 1 || y < 0)
      {
        return;
      }

      var tile = Tiles.Get(x, y);
      Tiles.Place(x, y, id);
      //tile.RefreshHash();
      //tile.SetIndex(id);
    }

    public void PutTile(int id, int x, int y)
    {
      // Check if the position is outside the bounds of the map.
      if (x > Tilemap.Width - 1 || x < 0 || y > Tilemap.Height - 1 || y < 0)
      {
        return;
      }

      var tile = Tiles.Get(x, y);

      Tiles.Place(x, y, id);

      //tile.RefreshHash();
      //tile.SetIndex(id);
    }

    public void EraseTile(int x, int y)
    {
      // Check if the position is outside the bounds of the map.
      if (x > Tilemap.Width - 1 || x < 0 || y > Tilemap.Height - 1 || y < 0)
      {
        return;
      }

      //var tile = Tiles.Get(x, y);
      Tiles.Place(x, y, -1);
      //tile = -1;
    }

    public void EraseTile(Vector2 position)
    {
      int x = (int)(position.X / Tilemap.TileWidth);
      int y = (int)(position.Y / Tilemap.TileHeight);

      // Check if the position is outside the bounds of the map.
      if (x > Tilemap.Width - 1 || x < 0 || y > Tilemap.Height - 1 || y < 0)
      {
        return;
      }

      Tiles.Place(x, y, -1);
      //var tile = Tiles.Get(x, y);
      //tile = -1;
    }

    public void Clear()
    {
      Tiles.Fill(-1);
    }

    public int GetTile(Vector2 position)
    {
      int x = (int)position.X / Tilemap.TileWidth;
      int y = (int)position.Y / Tilemap.TileHeight;

      if (x > Tilemap.Width - 1 || x < 0 || y > Tilemap.Height - 1 || y < 0)
      {
        return -1;
      }

      return Tiles.Get(x, y);
    }

    public int GetTile(int x, int y)
    {
      if (x > Tilemap.Width - 1 || x < 0 || y > Tilemap.Height - 1 || y < 0)
      {
        return -1;
      }

      return Tiles.Get(x, y);
    }

    public int GetTileIndexXY(int x, int y)
    {
      if (x > Tilemap.Width - 1 || x < 0 || y > Tilemap.Height - 1 || y < 0)
      {
        return -1;
      }

      return Tiles.Get(x, y);
    }

    /*
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