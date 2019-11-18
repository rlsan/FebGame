using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FebEngine.GUI
{
  public enum TextAlignment
  {
    TopLeft, Center
  }

  public class GUIText : GUIElement
  {
    public StringBuilder message;
    public TextAlignment alignment;

    public GUIText(params string[] lines)
    {
      SetMessage(lines);
    }

    public void SetMessage(params string[] lines)
    {
      var s = new StringBuilder();

      for (int i = 0; i < lines.Length; i++)
      {
        s.AppendLine(lines[i]);
      }
      message = s;
    }

    public override void Draw(SpriteBatch sb)
    {
      Vector2 size = Canvas.Font.MeasureString(message.ToString());
      Vector2 origin = size * 0.5f;

      if (alignment == TextAlignment.TopLeft)
      {
        sb.DrawString(Canvas.Font, message.ToString(), new Vector2(X, Y) + Vector2.One, Color.Black);
        sb.DrawString(Canvas.Font, message.ToString(), new Vector2(X, Y), Color.White);
      }
      else if (alignment == TextAlignment.Center)
      {
        sb.DrawString(Canvas.Font, message.ToString(), new Vector2(X + 1 + Width / 2, Y + 15 + Height / 2), Color.Black, 0, origin, 1, SpriteEffects.None, 0);
        sb.DrawString(Canvas.Font, message.ToString(), new Vector2(X + Width / 2, Y + 14 + Height / 2), Color.White, 0, origin, 1, SpriteEffects.None, 0);
      }

      base.Draw(sb);
    }
  }
}