using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace FebEngine.UI
{
  public class UIPrompt : UIContainer
  {
    private string title;
    private string message;

    private UIWindow window;
    private UITextBox messageBox;

    private UIButton[] buttons;

    public UIPrompt(string title = "", string message = "", params UIButton[] buttons)
    {
      this.title = title;
      this.message = message;

      this.buttons = buttons;
    }

    public override void Init()
    {
      window = AddChild("PromptWindow", new UIWindow(title, isDraggable: true), 0, 0, 400, 200) as UIWindow;
      messageBox = window.AddChild("PromptMessage", new UITextBox(message), 5, 25, 400, 200) as UITextBox;

      if (buttons.Length > 0)
      {
        int buttonWidth = window.Width / buttons.Length;

        for (int i = 0; i < buttons.Length; i++)
        {
          window.AddChild("Button" + i, new UIButton(buttons[i].title, buttons[i].onClick), buttonWidth * i, window.Height - 40, buttonWidth, 40);
        }
      }

      Disable();
    }

    public void Refresh(string title = "", string message = "")
    {
      window.title = title;
      messageBox.message = message;
    }
  }
}