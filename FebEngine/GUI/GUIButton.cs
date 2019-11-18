using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FebEngine.GUI
{
  public class GUIButton : GUIElement
  {
    public string title;
    public Action onClick;

    private bool isPressed;

    public GUIButton(string title = "", Action onClick = null)
    {
      this.title = title;
      this.onClick = onClick;
    }

    public override void Init()
    {
      base.Init();

      AddText(title);
    }

    public override void OnPress(Point mousePos)
    {
      isPressed = true;
    }

    public override void OnRelease()
    {
      if (isPressed)
      {
        if (onClick != null)
        {
          onClick.DynamicInvoke();
        }
      }
      isPressed = false;
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
    }

    public override void Draw(SpriteBatch sb)
    {
      //Debug.Text(title, X + 2, Y + 2);

      Color color = Color.White;
      if (isPressed)
      {
        color = Color.Gray;
      }

      // Fill
      sb.Draw(Canvas.Theme,
        new Rectangle(X + 8, Y + 8, Width - 8, Height - 8),
        new Rectangle(8, 8, 32, 32),
        color
          );

      // Sides
      sb.Draw(Canvas.Theme,
        new Rectangle(bounds.X + 8, bounds.Y, bounds.Width - 8 * 2, 8),
        new Rectangle(8, 0, 32, 8),
        color
          );
      sb.Draw(Canvas.Theme,
        new Rectangle(bounds.X, bounds.Y + 8, 8, bounds.Height - 8 * 2),
        new Rectangle(0, 8, 8, 32),
        color
          );
      sb.Draw(Canvas.Theme,
        new Rectangle(bounds.X + 8, Y + bounds.Height - 8, bounds.Width - 8 * 2, 8),
        new Rectangle(8, 32 + 8, 32, 8),
        color
          );
      sb.Draw(Canvas.Theme,
        new Rectangle(X + Width - 8, bounds.Y + 8, 8, bounds.Height - 8 * 2),
        new Rectangle(32 + 8, 8, 8, 32),
        color
          );

      // Corners
      sb.Draw(Canvas.Theme,
        new Rectangle(bounds.X, bounds.Y, 8, 8),
        new Rectangle(0, 0, 8, 8),
        color
          );
      sb.Draw(Canvas.Theme,
        new Rectangle(X + bounds.Width - 8, bounds.Y, 8, 8),
        new Rectangle(40, 0, 8, 8),
        color
          );
      sb.Draw(Canvas.Theme,
        new Rectangle(bounds.X, Y + bounds.Height - 8, 8, 8),
        new Rectangle(0, 40, 8, 8),
        color
          );
      sb.Draw(Canvas.Theme,
        new Rectangle(X + bounds.Width - 8, Y + bounds.Height - 8, 8, 8),
        new Rectangle(40, 40, 8, 8),
        color
          );

      base.Draw(sb);
    }
  }
}