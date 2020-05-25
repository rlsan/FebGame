using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Fubar.GUI
{
  public class GUITextPrompt : GUIWindow
  {
    public GUITextField textField;

    public StringBuilder refString;

    public string Text
    {
      get { return textField.text; }
      set { textField.SetMessage(value); }
    }

    public override void Init()
    {
      Action a;
      base.Init();

      anchorPosition = AnchorPosition.Center;

      AddText("Rename to:");
      textField = AddTextField();

      var buttons = AddPanel();
      buttons.division = Division.Horizontal;
      buttons.AddButton("Rename", Rename).Padding = 8;
      a = () => { Disable(); Canvas.locked = false; };
      buttons.AddButton("Cancel", a).Padding = 8;
    }

    public void SetString(ref StringBuilder s)
    {
      Text = s.ToString();
      refString = s;
    }

    private void Rename()
    {
      refString.Clear();
      refString.Append(textField.text);

      Disable();
      Canvas.locked = false;
    }

    public override void Update(GameTime gameTime)
    {
      if (Canvas.KeyboardAccept)
      {
        Rename();
      }
      if (Canvas.KeyboardCancel)
      {
        Disable();
      }

      base.Update(gameTime);
    }
  }
}