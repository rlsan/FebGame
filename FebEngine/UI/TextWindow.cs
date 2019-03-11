using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebEngine.UI
{
  public class TextWindow : UIElement
  {
    public string[] lines;

    private int indent = 8;

    private int marginX = 6;
    private int marginY = 10;

    private bool enableBackground;

    public TextWindow(bool enableBackground = false, params string[] newLines)
    {
      lines = newLines;
      this.enableBackground = enableBackground;
    }

    public void SetLines(params string[] newLines)
    {
      lines = newLines;
    }

    public override void Draw(SpriteBatch sb)
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
        new Rectangle(bounds.X + 1, bounds.Y + 1, bounds.Width - 1, 16),
        new Rectangle(0, 0, 16, 16),
        Color.White
          );

      Debug.Text(label, X + 5, Y + 5);

      for (int i = 0; i < lines.Length; i++)
      {
        string line = lines[i];

        Debug.Text(line, indent + X, 18 + Y + i * marginY);
      }
    }
  }
}