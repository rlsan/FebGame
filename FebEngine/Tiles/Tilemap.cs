using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using System;

namespace FebEngine.Tiles
{
  public class Tilemap
  {
    private TilemapLayer[] Layers;

    public Tileset tileset;

    public int width;
    public int height;

    public int tileWidth;
    public int tileHeight;

    public int LayerCount { get { return Layers.Length; } }

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

            //int timeFrame = (int)(gameTime.TotalGameTime.TotalSeconds * 12) % tile.frames.Length;
            //var frame = tile.frames[timeFrame];

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
                Color.White
                );
              /*
              if (tile.Name == "Random")
              {
                var rand = (RandomTile)tile;

                var pickedTile = rand.tiles[layer.hashArray[tileX, tileY] % rand.tiles.Length];

                framePosition = tileset.GetTilePositionFromIndex(pickedTile.id);
                x = (int)framePosition.X;
                y = (int)framePosition.Y;

                // Don't go straight to drawing.
                sb.Draw(
                  tileset.Texture,
                  destinationRectangle,
                  new Rectangle(x * tileWidth, y * tileHeight, tileWidth, tileHeight),
                  Color.White
                  );
              }
              else if (tile.Name == "Column")
              {
                var col = (ColumnTile)tile;

                //var tiles = rand.tiles[layer.hashArray[tileX, tileY] % rand.tiles.Length];

                if (layer.GetTileIndexXY(tileX, tileY - 1) != tile.id)
                {
                  framePosition = tileset.GetTilePositionFromIndex(col.top.id);
                }
                else if (layer.GetTileIndexXY(tileX, tileY + 1) != tile.id)
                {
                  framePosition = tileset.GetTilePositionFromIndex(col.bottom.id);
                }
                else
                {
                  framePosition = tileset.GetTilePositionFromIndex(col.middle.id);
                }

              x = (int)framePosition.X;
              y = (int)framePosition.Y;

              sb.Draw(
                tileset.Texture,
                destinationRectangle,
                new Rectangle(x * tileWidth, y * tileHeight, tileWidth, tileHeight),
                Color.White
                );
            }
            else
            */
              {
                sb.Draw(
                  tileset.Texture,
                  destinationRectangle,
                  new Rectangle(x * tileWidth, y * tileHeight, tileWidth, tileHeight),
                  Color.White
                  );
              }
            }
          }
        }
      }
    }
  }
}