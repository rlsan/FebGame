using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using FebEngine.Utility;

namespace FebEngine.UI
{
  public class UIButton : UIElement
  {
    public string title;
    public Action onClick;

    private bool isPressed;

    public UIButton(string title = "", Action onClick = null)
    {
      this.title = title;
      this.onClick = onClick;
    }

    public override void OnPress()
    {
      isPressed = true;
    }

    public override void OnRelease()
    {
      isPressed = false;

      if (onClick != null)
      {
        onClick.DynamicInvoke();
      }
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
    }

    public override void Draw(SpriteBatch sb)
    {
      Debug.Text(title, X + 2, Y + 2);

      Color color = Color.White;
      if (isPressed)
      {
        color = Color.Gray;
      }

      sb.Draw(Canvas.ThemeTexture,
        new Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height),
        new Rectangle(0, 0, 16, 16),
        color
          );

      base.Draw(sb);
    }
  }
}