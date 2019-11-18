using System;
using Microsoft.Xna.Framework;

namespace FebEngine.GUI
{
  public class GUIPrompt : GUIWindow
  {
    private GUIText textBox;
    public Action action;

    public string Message
    {
      get { return textBox.message.ToString(); }
      set { textBox.SetMessage(value); }
    }

    public override void Init()
    {
      base.Init();

      anchorPosition = AnchorPosition.Center;

      textBox = AddText();

      var buttons = AddPanel();
      buttons.division = Division.Horizontal;
      buttons.AddButton("Yes", Execute).Padding = 8;
      buttons.AddButton("No", Disable).Padding = 8;
    }

    private void Execute()
    {
      if (action != null)
      {
        action.DynamicInvoke();
      }
      Disable();
    }

    public override void Update(GameTime gameTime)
    {
      if (Canvas.KeyboardAccept)
      {
        if (action != null)
        {
          action.DynamicInvoke();
        }
        Disable();
      }
      if (Canvas.KeyboardCancel)
      {
        Disable();
      }

      base.Update(gameTime);
    }
  }
}