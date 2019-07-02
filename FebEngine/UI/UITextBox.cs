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
    public string message = "";

    public UITextBox(string message = "")
    {
      this.message = message;
    }

    public override void Draw(SpriteBatch sb)
    {
      Debug.Text(message, X + 2, Y + 2);

      base.Draw(sb);
    }
  }
}