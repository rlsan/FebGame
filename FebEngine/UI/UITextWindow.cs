using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FebEngine.Utility;

namespace FebEngine.UI
{
  public class UITextWindow : UIElement
  {
    public string title;
    public string[] lines;

    private int indent = 8;

    private int marginX = 12;
    private int marginY = 20;

    private bool enableBackground;

    public UITextWindow(string title, int x, int y, int width, int height, bool enableBackground = false, params string[] newLines)
    {
      this.title = title;
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
          sb.Draw(Canvas.ThemeTexture,
            bounds,
            new Rectangle(16, 0, 16, 16),
            Color.White
              );
        }

        sb.Draw(Canvas.ThemeTexture,
          new Rectangle(bounds.X + 0, bounds.Y + 0, bounds.Width - 0, 20),
          new Rectangle(0, 0, 16, 16),
          Color.White
            );

        Debug.Text(title, X + 2, Y + 2);

        for (int i = 0; i < lines.Length; i++)
        {
          string line = lines[i];

          Debug.Text(line, indent + X, 26 + Y + i * marginY);
        }

        base.Draw(sb);
      }
    }
  }
}