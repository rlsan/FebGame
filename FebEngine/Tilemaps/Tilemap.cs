using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TexturePackerLoader;

namespace Fubar
{
  public class Tilemap : Entity
  {
    public Tileset Tileset { get; set; }

    public ObjectLayer ObjectLayer { get; }

    public List<TilemapLayer> Layers { get; } = new List<TilemapLayer>();

    public int LayerCount { get { return Layers.Count; } }

    public int X;
    public int Y;

    public List<SideWarp> sideWarps;

    /// <summary>
    /// The map's width in tiles.
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// The map's height in tiles.
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// The width of each tile.
    /// </summary>
    public int TileWidth { get; }

    /// <summary>
    /// The height of each tile.
    /// </summary>
    public int TileHeight { get; }

    /// <summary>
    /// The bounding box for the map.
    /// </summary>
    public Rectangle Bounds
    {
      get { return new Rectangle(X, Y, Width, Height); }
    }

    public void ClearLayers()
    {
      foreach (var layer in Layers)
      {
        layer.Clear();
      }
    }

    public Dictionary<WarpDirection, string> ConnectedMaps { get; set; } = new Dictionary<WarpDirection, string>();

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="tileset">The tileset that the tilemap should draw.</param>
    public Tilemap(int width, int height, int tileWidth, int tileHeight)
    {
      Width = width;
      Height = height;

      TileWidth = tileWidth;
      TileHeight = tileHeight;

      ObjectLayer = new ObjectLayer();

      sideWarps = new List<SideWarp>();
    }

    public void ExtendUp(int amount)
    {
      foreach (var layer in Layers) layer.ExtendUp(amount);
    }

    public void ExtendDown(int amount)
    {
      foreach (var layer in Layers) layer.ExtendDown(amount);
    }

    public void ExtendLeft(int amount)
    {
      foreach (var layer in Layers) layer.ExtendLeft(amount);
    }

    public void ExtendRight(int amount)
    {
      foreach (var layer in Layers) layer.ExtendRight(amount);
    }

    /// <summary>
    /// Replaces the layers with fresh ones.
    /// Used for initialization, should never really be used twice.
    /// </summary>
    /// <param name="names">What to name each layer in order.</param>
    public void SetLayers(params string[] names)
    {
      Layers.Clear();

      for (int i = 0; i < names.Length; i++)
      {
        Layers.Add(new TilemapLayer(this, names[i]));
      }
    }

    /// <summary>
    /// Adds a new layer to the map.
    /// </summary>
    /// <param name="name">The name for the new layer.</param>
    /// <returns>The added layer.</returns>
    public TilemapLayer AddLayer(string name)
    {
      TilemapLayer addedLayer = new TilemapLayer(this, name);

      Layers.Add(addedLayer);

      return addedLayer;
    }

    /// <summary>
    /// Handy method for getting a layer from index.
    /// </summary>
    /// <param name="i">Index of the layer you want.</param>
    /// <returns></returns>
    public TilemapLayer GetLayer(int i)
    {
      return Layers[i];
    }

    public void Refresh()
    {
      foreach (var layer in Layers)
      {
        //layer.Refresh();
      }
    }

    public override void Draw(RenderManager renderer, GameTime gameTime)
    {
      // Don't draw if there's no tileset.
      if (Tileset == null) return;

      // Iterate through each layer in the map
      for (int layerID = 0; layerID < LayerCount; layerID++)
      {
        var layer = GetLayer(layerID);

        // Only draw this layer if it's visible
        if (!layer.IsVisible) return;

        // Iterate through each tile in this layer
        for (int tileIndex = 0; tileIndex < layer.Tiles.Count; tileIndex++)
        {
          int tileX = tileIndex % Width;
          int tileY = tileIndex / Width;
          var tile = layer.GetTile(tileX, tileY);

          // Tiles with ID -1 and less are ignored.
          if (tile == -1)
          {
            continue;
          }

          // Gets the destination rectangle by the tile's raw position,
          // multiplying and offsetting it by the map's bounds and the layer's offset.
          Rectangle destination = new Rectangle(
            tileX * TileWidth + layer.X,
            tileY * TileHeight + layer.Y,
            TileWidth,
            TileHeight);

          // Uses the index to locate the correct region of the tileset's texture.

          var tileyes = new Tile(layer, tileX, tileY);

          var foo = Tileset.GetBrushFromIndex(tile);
          //int frame = foo.GetFirstFrame();
          int frame = foo.GetFrame(tileyes);

          int frameId = tile;
          Vector2 tilesetPosition = Tileset.GetTilePositionFromIndex(frame);

          // Divides the position into separate X and Y components, and multiply them by the tileset's grid unit width and height.
          int tilesetX = (int)tilesetPosition.X * Tileset.TileWidth;
          int tilesetY = (int)tilesetPosition.Y * Tileset.TileHeight;

          Rectangle source = new Rectangle(
            tilesetX,
            tilesetY,
            Tileset.TileWidth,
            Tileset.TileHeight);

          // Draw the tile.
          renderer.SpriteBatch.Draw(
            Tileset.Texture,
            destination,
            source,
            Color.White
            );
        }
      }
    }

    public enum WarpDirection
    {
      Up, Left, Right, Down, Free
    }

    public struct MapMusic
    {
    }
  }
}