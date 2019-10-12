using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebEngine
{
  public class RetroFont
  {
    public Vector2 position;

    private Texture2D texture;
    private string layout;
    private string message;

    private int charWidth;
    private int charHeight;

    private List<int> code = new List<int>();

    public RetroFont(Texture2D texture, Vector2 position, int charWidth, int charHeight, string message = "text", RetroFontLayout layout = RetroFontLayout.Basic)
    {
      this.texture = texture;
      this.position = position;

      this.charWidth = charWidth;
      this.charHeight = charHeight;

      //this.message = message;

      switch (layout)
      {
        case RetroFontLayout.Basic:
          this.layout = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
          break;

        case RetroFontLayout.Uppercase:
          this.layout = " ABCDEFGHIJKLMNOPQRSTUVWXYZ";
          break;

        case RetroFontLayout.Digits:
          this.layout = " 0123456789";
          break;
      }

      SetMessage(message);
    }

    public void SetMessage(object messageToSet)
    {
      code.Clear();

      message = messageToSet.ToString();
      //message = message.ToUpper();

      foreach (char letter in message)
      {
        for (int i = 0; i < layout.Length; i++)
        {
          if (letter == layout[i])
          {
            code.Add(i);
            break;
          }
        }
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      for (int i = 0; i < message.Length; i++)
      {
        spriteBatch.Draw(
          texture,
          new Rectangle(i * charWidth + (int)position.X, (int)position.Y, charWidth, charHeight),
          new Rectangle(code[i] * charWidth, 0, charWidth, charHeight),
          Color.White
          );
      }
    }
  }

  public enum RetroFontLayout
  {
    Basic, Uppercase, Digits
  }
}