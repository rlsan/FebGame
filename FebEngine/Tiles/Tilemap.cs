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

    public bool isShowingTileProperties = false;

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

    /// <summary>
    /// // Debug for visualizing the property of each tile.
    /// This isn't going to work for tiles with 2+ properties.
    /// </summary>
    /// <param name="t">The tile to get the color from.</param>
    public Color GetTilePropertyTint(Tile t)
    {
      switch (t.properties[0])
      {
        case TileType.None:
          return Color.White;

        case TileType.Solid:
          return Color.Red;

        case TileType.OneWayUp:
          return Color.LightGreen;

        case TileType.Half:
          return Color.Pink;

        case TileType.Ladder:
          return Color.DarkGray;

        case TileType.LadderTop:
          return Color.Gray;

        case TileType.Water:
          return Color.Cyan;

        case TileType.Breakable:
          return Color.Yellow;

        default:
          return Color.White;
      }
    }

    public void DrawTile(SpriteBatch sb, Tile tile, TilemapLayer layer)
    {
      // Debug for coloring the tile based on its property.
      if (isShowingTileProperties) tile.tint = GetTilePropertyTint(tile);

      // Cut out the appropriate tile from the tileset.
      Vector2 tilesetPosition = tileset.GetTilePositionFromIndex(tile.id);
      int tilesetX = (int)tilesetPosition.X;
      int tilesetY = (int)tilesetPosition.Y;

      var destinationRectangle = new Rectangle(
        tile.X * tileWidth + layer.X,
        tile.Y * tileHeight + layer.Y,
        tileWidth,
        tileHeight);

      int frame = tile.ReturnFrame(layer, tile.X, tile.Y);

      tilesetPosition = tileset.GetTilePositionFromIndex(frame);
      tilesetX = (int)tilesetPosition.X;
      tilesetY = (int)tilesetPosition.Y;

      sb.Draw(
        tileset.Texture,
        destinationRectangle,
        new Rectangle(tilesetX * tileWidth, tilesetY * tileHeight, tileWidth, tileHeight),
        tile.tint
        );
    }

    public void Draw(SpriteBatch sb)
    {
      // Iterate through each layer in the map
      for (int layerID = 0; layerID < LayerCount; layerID++)
      {
        var layer = GetLayer(layerID);

        // Only draw this layer if it's visible
        if (layer.IsVisible)
        {
          // Iterate through each tile in this layer
          for (int tileIndex = 0; tileIndex < layer.tileArray.Length; tileIndex++)
          {
            int tileX = tileIndex % width;
            int tileY = tileIndex / width;
            var tile = layer.tileArray[tileX, tileY];

            // Tiles with ID -1 and less are ignored.
            if (tile.id >= 0)
            {
              // Debug for coloring the tile based on its property.
              if (isShowingTileProperties) tile.tint = GetTilePropertyTint(tile);

              // Cut out the appropriate tile from the tileset.
              Vector2 tilesetPosition = tileset.GetTilePositionFromIndex(tile.id);
              int tilesetX = (int)tilesetPosition.X;
              int tilesetY = (int)tilesetPosition.Y;

              var destinationRectangle = new Rectangle(
                tileX * tileWidth + layer.X,
                tileY * tileHeight + layer.Y,
                tileWidth,
                tileHeight);

              int frame = tile.ReturnFrame(layer, tileX, tileY);

              tilesetPosition = tileset.GetTilePositionFromIndex(frame);
              tilesetX = (int)tilesetPosition.X;
              tilesetY = (int)tilesetPosition.Y;

              sb.Draw(
                tileset.Texture,
                destinationRectangle,
                new Rectangle(tilesetX * tileWidth, tilesetY * tileHeight, tileWidth, tileHeight),
                tile.tint
                );
            }
          }
        }
      }
    }
  }
}