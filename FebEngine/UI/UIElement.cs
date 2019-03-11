using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FebEngine.UI
{
  public abstract class UIElement
  {
    public string label;
    public UICanvas canvas;

    public Rectangle bounds;

    public int X { get { return bounds.X; } set { bounds.X = value; } }
    public int Y { get { return bounds.Y; } set { bounds.Y = value; } }

    public abstract void Draw(SpriteBatch sb);
  }
}