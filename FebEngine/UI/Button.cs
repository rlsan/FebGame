using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace FebEngine.UI
{
  public class Button : UIElement
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

    public override void Draw(SpriteBatch sb)
    {
    }
  }
}