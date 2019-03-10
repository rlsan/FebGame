using Microsoft.Xna.Framework.Graphics;
using System.Collections;

namespace FebEngine
{
  public class Tilemap
  {
    private SortedList Layers;

    public Texture2D texture;

    public int width;
    public int height;

    public int tileWidth;
    public int tileHeight;

    public int LayerCount { get { return Layers.Count; } }

    public Tilemap(int width, int height, int tileWidth, int tileHeight)
    {
      Layers = new SortedList();

      this.width = width;
      this.height = height;

      this.tileWidth = tileWidth;
      this.tileHeight = tileHeight;
    }

    public void AddLayer(string name)
    {
      Layers.Add(name, new TilemapLayer(this, name));
    }

    public TilemapLayer GetLayer(int i)
    {
      return Layers.GetByIndex(i) as TilemapLayer;
    }
  }
}