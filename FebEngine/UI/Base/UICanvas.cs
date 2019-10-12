using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebEngine.UI
{
  public class UICanvas : Entity
  {
    private Dictionary<string, UIElement> elements;

    public Texture2D ThemeTexture { get; set; }
    public Texture2D FontTexture { get; set; }
    public ContentManager contentManager;

    public Rectangle bounds;

    public MouseState mouse;
    public KeyboardState keyboard;

    public bool MousePress { get; set; }
    public bool MouseDown { get; set; }
    public bool MouseUp { get; set; }

    public UIElement activeElement;

    private bool hasClicked;
    private bool wasSomethingClicked;

    private UIPrompt globalPrompt;

    public UICanvas(int width, int height)
    {
      elements = new Dictionary<string, UIElement>();
      bounds = new Rectangle(0, 0, width, height);

      globalPrompt = AddElement("GlobalPrompt", new UIPrompt(title: "Prompt", message: "None"), 0, 0, 400, 300) as UIPrompt;

      //FollowCamera = false;
    }

    public UIElement AddElement(string label, UIElement element, int x = 0, int y = 0, int width = 0, int height = 0, bool startInvisible = false)
    {
      UIElement e = element;

      e.offsetX = x;
      e.offsetY = y;
      e.bounds = new Rectangle(x, y, width, height);

      e.label = label;
      e.Canvas = this;

      elements.Add(label, e);

      e.Init();

      if (startInvisible)
      {
        e.Disable();
      }

      return e;
    }

    public void SetActiveElement(UIElement elem)
    {
      activeElement = elem;
    }

    public UIElement GetElement(string label)
    {
      if (elements.ContainsKey(label))
      {
        return elements[label];
      }

      return null;
    }

    public void ShowPrompt(string title = "", string message = "", params UIButton[] buttons)
    {
      globalPrompt.Refresh(title, message, buttons);

      globalPrompt.Enable();
    }

    public void ClosePrompt()
    {
      globalPrompt.Disable();
    }

    public override void Update(GameTime gameTime)
    {
      mouse = Mouse.GetState();
      keyboard = Keyboard.GetState();

      wasSomethingClicked = false;
      MousePress = false;

      MouseDown = mouse.LeftButton == ButtonState.Pressed;
      MouseUp = !MouseDown;

      foreach (var element in elements.ToList())
      {
        if (element.Value.toBeDestroyed)
        {
          elements.Remove(element.Key);
        }
      }

      if (!hasClicked)
      {
        //Left mouse button has been pressed once
        if (mouse.LeftButton == ButtonState.Pressed)
        {
          hasClicked = true;

          foreach (var element in elements)
          {
            if (element.Value.isVisible)
            {
              if (element.Value.bounds.Contains(mouse.Position))
              {
                SetActiveElement(element.Value);

                element.Value.OnPress(mouse.Position);

                wasSomethingClicked = true;
              }
            }
          }

          if (!wasSomethingClicked)
          {
            SetActiveElement(null);
            MousePress = true;
          }
        }
      }
      else
      {
        //Left mouse button has been released
        if (mouse.LeftButton == ButtonState.Released)
        {
          hasClicked = false;

          foreach (var element in elements.ToList())
          {
            if (element.Value.isVisible)
            {
              if (element.Value.bounds.Contains(mouse.Position))
              {
                element.Value.OnRelease();
              }
            }
          }
        }
      }

      foreach (var element in elements)
      {
        if (mouse.LeftButton == ButtonState.Pressed)
        {
          if (element.Value.isVisible)
          {
            if (element.Value.bounds.Contains(mouse.Position))
            {
              element.Value.OnHold();
            }
          }
        }

        element.Value.Update(gameTime);
      }
    }

    public override void Draw(SpriteBatch sb, GameTime gameTime)
    {
      foreach (var element in elements)
      {
        if (element.Value.isVisible)
        {
          element.Value.Draw(sb);
        }
      }
    }
  }
}