using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FebEngine.Utility;
using Microsoft.Xna.Framework.Graphics;

namespace FebEngine.UI
{
  public class UITextBox : UIElement
  {
    public string[] message;

    public UITextBox(params string[] lines)
    {
      message = lines;
    }

    public void SetMessage(params string[] lines)
    {
      message = lines;
    }

    public override void Draw(SpriteBatch sb)
    {
      for (int i = 0; i < message.Length; i++)
      {
        Debug.Text(message[i], X + 2, Y + (i * 20) + 2);
      }

      base.Draw(sb);
    }
  }
}