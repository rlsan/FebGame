using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FebEngine.UI
{
  public abstract class Button
  {
    public Rectangle Bounds { get; set; }

    public bool Hover
    {
      get
      {
        var m = Mouse.GetState();

        return Bounds.Contains(m.Position.ToVector2() / 2f);
      }
    }

    public bool Pressed
    {
      get
      {
        var m = Mouse.GetState();

        return Hover && m.LeftButton == ButtonState.Pressed || Hover && m.RightButton == ButtonState.Pressed;
      }
    }
  }
}