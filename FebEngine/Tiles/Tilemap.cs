using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using System;

namespace FebEngine.Tiles
{
  public class Tilemap
  {
    public TilemapLayer[] Layers { get; set; }

    public Tileset tileset;

    public string name;

    public int X;
    public int Y;

    public int width;
    public int height;

    public int tileWidth;
    public int tileHeight;

    public Rectangle bounds
    {
      get { return new Rectangle(X, Y, width, height); }
    }

    public int LayerCount { get { return Layers.Length; } }

    public bool showTileProperties = false;

    public Tilemap(int width, int height, int tileWidth, int tileHeight)
    {
      Layers = new TilemapLayer[0];

      this.width = width;
      this.height = height;

      this.tileWidth = tileWidth;
      this.tileHeight = tileHeight;
    }

    public void SetLayers(params string[] names)
    {
      Layers = new TilemapLayer[names.Length];

      for (int i = 0; i < names.Length; i++)
      {
        Layers[i] = new TilemapLayer(this, names[i]);
      }
    }

    public TilemapLayer GetLayer(int i)
    {
      return Layers[i];
    }

    public void Draw(SpriteBatch sb)
    {
      for (int layerID = 0; layerID < LayerCount; layerID++)
      {
        var layer = GetLayer(layerID);

        if (layer.IsVisible)
        {
          for (int tileIndex = 0; tileIndex < layer.tileArray.Length; tileIndex++)
          {
            int tileX = tileIndex % width;
            int tileY = tileIndex / width;
            var tile = layer.tileArray[tileX, tileY];

            tile.tint = Color.White;
            if (showTileProperties)
            {
              if (tile.properties[0] == TileType.None)
              {
                tile.tint = Color.DarkRed;
              }
              if (tile.properties[0] == TileType.Solid)
              {
                tile.tint = Color.LightGreen;
              }
              else if (tile.properties[0] == TileType.Breakable)
              {
                tile.tint = Color.Yellow;
              }
            }

            Vector2 framePosition = tileset.GetTilePositionFromIndex(tile.id);
            int x = (int)framePosition.X;
            int y = (int)framePosition.Y;

            var destinationRectangle = new Rectangle(tileX * tileWidth + layer.X, tileY * tileHeight + layer.Y, tileWidth, tileHeight);

            if (tile.id >= 0)
            {
              int frame = tile.ReturnFrame(layer, tileX, tileY);

              //Debug.Text(destinationRectangle.X / tileWidth);

              framePosition = tileset.GetTilePositionFromIndex(frame);
              x = (int)framePosition.X;
              y = (int)framePosition.Y;

              sb.Draw(
                tileset.Texture,
                destinationRectangle,
                new Rectangle(x * tileWidth, y * tileHeight, tileWidth, tileHeight),
                tile.tint
                );
            }
          }
        }
      }
    }
  }
}