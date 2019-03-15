using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebEngine.UI
{
  public class UITextWindow : UIElement
  {
    public string[] lines;

    private int indent = 8;

    private int marginX = 6;
    private int marginY = 10;

    private bool enableBackground;

    public UITextWindow(int x, int y, int width, int height, bool enableBackground = false, params string[] newLines)
    {
      lines = newLines;
      bounds = new Rectangle(x, y, width, height);
      this.enableBackground = enableBackground;
    }

    public void SetLines(params string[] newLines)
    {
      lines = newLines;
    }

    public override void Draw(SpriteBatch sb)
    {
      if (isVisible)
      {
        if (enableBackground)
        {
          sb.Draw(canvas.ThemeTexture,
            bounds,
            new Rectangle(16, 0, 16, 16),
            Color.White
              );
        }

        sb.Draw(canvas.ThemeTexture,
          new Rectangle(bounds.X + 1, bounds.Y + 1, bounds.Width - 1, 10),
          new Rectangle(0, 0, 16, 16),
          Color.White
            );

        Debug.Text(label, X + 2, Y + 2);

        for (int i = 0; i < lines.Length; i++)
        {
          string line = lines[i];

          Debug.Text(line, indent + X, 18 + Y + i * marginY);
        }

        base.Draw(sb);
      }
    }
  }
}