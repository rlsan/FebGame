using Microsoft.Xna.Framework;
using FebEngine.Tiles;
using Microsoft.Xna.Framework.Graphics;

namespace FebEngine.UI
{
  public class TileBrushSwatch : UIButton
  {
    public Tile tile;

    public TileBrushSwatch(Tile tile, Vector2 position, int size)
    {
      bounds = new Rectangle(position.ToPoint(), new Point(size));
      this.tile = tile;
    }
  }
}