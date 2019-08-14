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
    private List<UIButton> buttons;

    private int amountOfButtons;

    public UIPrompt(string title = "", string message = "", int amountOfButtons = 3)
    {
      this.title = title;
      this.message = message;

      this.amountOfButtons = amountOfButtons;
    }

    public override void Init()
    {
      buttons = new List<UIButton>();

      window = AddChild("PromptWindow", new UIWindow(title, isDraggable: true), 0, 0, 400, 200) as UIWindow;
      messageBox = window.AddChild("PromptMessage", new UITextBox(message), 5, 25, 400, 200) as UITextBox;

      int buttonWidth = window.Width / amountOfButtons;

      for (int i = 0; i < amountOfButtons; i++)
      {
        var button = window.AddChild("Button" + i, new UIButton(), buttonWidth * i, window.Height - 40, buttonWidth, 40) as UIButton;
        buttons.Add(button);
      }

      Disable();
    }

    public void Refresh(string title = "", string message = "", params UIButton[] buttonRefs)
    {
      window.title = title;
      messageBox.SetMessage(message);

      for (int i = 0; i < buttonRefs.Length; i++)
      {
        if (i > amountOfButtons)
        {
          break;
        }

        var button = buttons[i];

        button.title = buttonRefs[i].title;
        button.onClick = buttonRefs[i].onClick;
      }
    }
  }
}