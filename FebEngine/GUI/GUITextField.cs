using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebEngine.GUI
{
  public class GUITextField : GUIButton
  {
    public bool active = false;
    public string text = "";

    private Keys[] lastKeys;
    public double timer;
    private KeyboardState lastKeyboardState;

    private bool turbo = false;
    private bool shift = false;

    public GUITextField(string text = "")
    {
      this.text = text;
    }

    public override void Init()
    {
      base.Init();

      //Padding = 16;
    }

    public void SetMessage(object message)
    {
      text = message.ToString();
    }

    public override void OnPress(Point mousePos)
    {
      active = true;

      base.OnPress(mousePos);
    }

    public override void OnRelease()
    {
      base.OnRelease();
    }

    public override void Update(GameTime gameTime)
    {
      if (active)
      {
        //Get the current keyboard state and keys that are pressed
        KeyboardState keyboardState = Canvas.keyboard;
        Keys[] keys = Canvas.keyboard.GetPressedKeys();

        shift = keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift);

        foreach (Keys currentKey in keys)
        {
          if (currentKey != Keys.None)
          {
            //If we have pressed the same key twice, wait atleast 125ms before adding it again
            if (lastKeys.Contains(currentKey))
            {
              if (turbo == true)
              {
                HandleKey(gameTime, currentKey);
              }
              else
              {
                if (gameTime.TotalGameTime.TotalMilliseconds - timer > 300)
                {
                  turbo = true;
                  HandleKey(gameTime, currentKey);
                }
              }
            }
            //If we press a new key, add it
            else if (!lastKeys.Contains(currentKey))
            {
              HandleKey(gameTime, currentKey);
              turbo = false;
            }
          }
        }

        //Save the last keys and pressed keys array
        lastKeyboardState = keyboardState;
        lastKeys = keys;
      }

      base.Update(gameTime);
    }

    public override void Draw(SpriteBatch sb)
    {
      sb.Draw(Canvas.Theme,
        new Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height),
        new Rectangle(0, 16, 16, 16),
        Color.White
          );

      if (active)
      {
        sb.DrawString(Canvas.Font, text + "_", new Vector2(X + 4, Y + 4), Color.White);
      }
      else
      {
        sb.DrawString(Canvas.Font, text, new Vector2(X + 4, Y + 4), Color.White);
      }
    }

    private void HandleKey(GameTime gameTime, Keys currentKey)
    {
      string keyString = currentKey.ToString();

      timer = gameTime.TotalGameTime.TotalMilliseconds;

      if ((currentKey == Keys.Back || currentKey == Keys.Delete) && text.Length > 0)
        text = text.Remove(text.Length - 1);

      switch (currentKey)
      {
        case Keys.None:
          break;

        case Keys.Back:
          break;

        case Keys.Tab:
          break;

        case Keys.Enter:
          break;

        case Keys.CapsLock:
          break;

        case Keys.Escape:
          break;

        case Keys.Space:
          text += " ";
          break;

        case Keys.PageUp:
          break;

        case Keys.PageDown:
          break;

        case Keys.End:
          break;

        case Keys.Home:
          break;

        case Keys.Left:
          break;

        case Keys.Up:
          break;

        case Keys.Right:
          break;

        case Keys.Down:
          break;

        case Keys.Select:
          break;

        case Keys.Print:
          break;

        case Keys.Execute:
          break;

        case Keys.PrintScreen:
          break;

        case Keys.Insert:
          break;

        case Keys.Delete:
          break;

        case Keys.Help:
          break;

        case Keys.D0:
          text += "0";
          break;

        case Keys.D1:
          text += "1";
          break;

        case Keys.D2:
          text += "2";
          break;

        case Keys.D3:
          text += "3";
          break;

        case Keys.D4:
          text += "4";
          break;

        case Keys.D5:
          text += "5";
          break;

        case Keys.D6:
          text += "6";
          break;

        case Keys.D7:
          text += "7";
          break;

        case Keys.D8:
          text += "8";
          break;

        case Keys.D9:
          text += "9";
          break;

        case Keys.LeftWindows:
          break;

        case Keys.RightWindows:
          break;

        case Keys.Apps:
          break;

        case Keys.Sleep:
          break;

        case Keys.NumPad0:
          break;

        case Keys.NumPad1:
          break;

        case Keys.NumPad2:
          break;

        case Keys.NumPad3:
          break;

        case Keys.NumPad4:
          break;

        case Keys.NumPad5:
          break;

        case Keys.NumPad6:
          break;

        case Keys.NumPad7:
          break;

        case Keys.NumPad8:
          break;

        case Keys.NumPad9:
          break;

        case Keys.Multiply:
          break;

        case Keys.Add:
          break;

        case Keys.Separator:
          break;

        case Keys.Subtract:
          break;

        case Keys.Decimal:
          break;

        case Keys.Divide:
          break;

        case Keys.F1:
          break;

        case Keys.F2:
          break;

        case Keys.F3:
          break;

        case Keys.F4:
          break;

        case Keys.F5:
          break;

        case Keys.F6:
          break;

        case Keys.F7:
          break;

        case Keys.F8:
          break;

        case Keys.F9:
          break;

        case Keys.F10:
          break;

        case Keys.F11:
          break;

        case Keys.F12:
          break;

        case Keys.F13:
          break;

        case Keys.F14:
          break;

        case Keys.F15:
          break;

        case Keys.F16:
          break;

        case Keys.F17:
          break;

        case Keys.F18:
          break;

        case Keys.F19:
          break;

        case Keys.F20:
          break;

        case Keys.F21:
          break;

        case Keys.F22:
          break;

        case Keys.F23:
          break;

        case Keys.F24:
          break;

        case Keys.NumLock:
          break;

        case Keys.Scroll:
          break;

        case Keys.LeftShift:
          break;

        case Keys.RightShift:
          break;

        case Keys.LeftControl:
          break;

        case Keys.RightControl:
          break;

        case Keys.LeftAlt:
          break;

        case Keys.RightAlt:
          break;

        case Keys.BrowserBack:
          break;

        case Keys.BrowserForward:
          break;

        case Keys.BrowserRefresh:
          break;

        case Keys.BrowserStop:
          break;

        case Keys.BrowserSearch:
          break;

        case Keys.BrowserFavorites:
          break;

        case Keys.BrowserHome:
          break;

        case Keys.VolumeMute:
          break;

        case Keys.VolumeDown:
          break;

        case Keys.VolumeUp:
          break;

        case Keys.MediaNextTrack:
          break;

        case Keys.MediaPreviousTrack:
          break;

        case Keys.MediaStop:
          break;

        case Keys.MediaPlayPause:
          break;

        case Keys.LaunchMail:
          break;

        case Keys.SelectMedia:
          break;

        case Keys.LaunchApplication1:
          break;

        case Keys.LaunchApplication2:
          break;

        case Keys.OemSemicolon:
          if (shift) text += ":";
          else text += ";";
          break;

        case Keys.OemPlus:
          if (shift) text += "+";
          else text += "=";
          break;

        case Keys.OemComma:
          if (shift) text += "<";
          else text += ",";
          break;

        case Keys.OemMinus:
          if (shift) text += "_";
          else text += "-";
          break;

        case Keys.OemPeriod:
          if (shift) text += ">";
          else text += ".";
          break;

        case Keys.OemQuestion:
          if (shift) text += "?";
          else text += "/";
          break;

        case Keys.OemTilde:
          break;

        case Keys.OemOpenBrackets:
          break;

        case Keys.OemPipe:
          break;

        case Keys.OemCloseBrackets:
          break;

        case Keys.OemQuotes:
          if (shift) text += "|";
          else text += "\\";
          break;

        case Keys.Oem8:
          text += "$";
          break;

        case Keys.OemBackslash:
          break;

        case Keys.ProcessKey:
          break;

        case Keys.Attn:
          break;

        case Keys.Crsel:
          break;

        case Keys.Exsel:
          break;

        case Keys.EraseEof:
          break;

        case Keys.Play:
          break;

        case Keys.Zoom:
          break;

        case Keys.Pa1:
          break;

        case Keys.OemClear:
          break;

        case Keys.ChatPadGreen:
          break;

        case Keys.ChatPadOrange:
          break;

        case Keys.Pause:
          break;

        case Keys.ImeConvert:
          break;

        case Keys.ImeNoConvert:
          break;

        case Keys.Kana:
          break;

        case Keys.Kanji:
          break;

        case Keys.OemAuto:
          break;

        case Keys.OemCopy:
          break;

        case Keys.OemEnlW:
          break;

        default:
          if (shift) text += keyString;
          else text += keyString.ToLower();

          break;
      }
    }
  }
}