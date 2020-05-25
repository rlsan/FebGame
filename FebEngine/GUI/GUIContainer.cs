using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fubar.GUI
{
  public class GUIContainer : GUIElement
  {
    public bool drawBackground = false;

    public override void Draw(SpriteBatch sb)
    {
      if (drawBackground)
      {
        var color = Color.White;
        int bgOffsetX = 0;
        int bgOffsetY = 96;

        // Fill
        sb.Draw(Canvas.Theme,
          new Rectangle(X + 8, Y + 8, Width - 8, Height - 8),
          new Rectangle(bgOffsetX + 8, bgOffsetY + 8, 32, 32),
          color
            );

        // Sides
        sb.Draw(Canvas.Theme,
          new Rectangle(bounds.X + 8, bounds.Y, bounds.Width - 8 * 2, 8),
          new Rectangle(bgOffsetX + 8, bgOffsetY + 0, 32, 8),
          color
            );
        sb.Draw(Canvas.Theme,
          new Rectangle(bounds.X, bounds.Y + 8, 8, bounds.Height - 8 * 2),
          new Rectangle(bgOffsetX + 0, bgOffsetY + 8, 8, 32),
          color
            );
        sb.Draw(Canvas.Theme,
          new Rectangle(bounds.X + 8, Y + bounds.Height - 8, bounds.Width - 8 * 2, 8),
          new Rectangle(bgOffsetX + 8, bgOffsetY + 32 + 8, 32, 8),
          color
            );
        sb.Draw(Canvas.Theme,
          new Rectangle(X + Width - 8, bounds.Y + 8, 8, bounds.Height - 8 * 2),
          new Rectangle(bgOffsetX + 32 + 8, bgOffsetY + 8, 8, 32),
          color
            );

        // Corners
        sb.Draw(Canvas.Theme,
          new Rectangle(bounds.X, bounds.Y, 8, 8),
          new Rectangle(bgOffsetX + 0, bgOffsetY + 0, 8, 8),
          color
            );
        sb.Draw(Canvas.Theme,
          new Rectangle(X + bounds.Width - 8, bounds.Y, 8, 8),
          new Rectangle(bgOffsetX + 40, bgOffsetY + 0, 8, 8),
          color
            );
        sb.Draw(Canvas.Theme,
          new Rectangle(bounds.X, Y + bounds.Height - 8, 8, 8),
          new Rectangle(bgOffsetX + 0, bgOffsetY + 40, 8, 8),
          color
            );
        sb.Draw(Canvas.Theme,
          new Rectangle(X + bounds.Width - 8, Y + bounds.Height - 8, 8, 8),
          new Rectangle(bgOffsetX + 40, bgOffsetY + 40, 8, 8),
          color
            );
      }

      base.Draw(sb);
    }
  }
}