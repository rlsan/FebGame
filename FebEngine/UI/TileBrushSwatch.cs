using Microsoft.Xna.Framework;
using FebEngine.Tiles;

namespace FebEngine.UI
{
  public class TileBrushSwatch : Button
  {
    public Tile tile;

    public TileBrushSwatch(Tile tile, Vector2 position, int size)
    {
      Bounds = new Rectangle(position.ToPoint(), new Point(size));
      this.tile = tile;
    }
  }
}