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
  public class UICanvas
  {
    private Dictionary<string, UIElement> elements;

    public Texture2D ThemeTexture { get; set; }
    public Texture2D FontTexture { get; set; }
    public ContentManager contentManager;

    public Rectangle bounds;

    public MouseState mouse;
    public KeyboardState keyboard;

    public UIElement activeElement;

    private bool hasClicked;
    private bool wasSomethingClicked;

    private UIPrompt globalPrompt;

    public UICanvas()
    {
      elements = new Dictionary<string, UIElement>();
      bounds = new Rectangle(0, 0, 400, 400);

      globalPrompt = AddElement("GlobalPrompt", new UIPrompt(title: "Prompt", message: "None"), 0, 0, 400, 300) as UIPrompt;
    }

    public UIElement AddElement(string label, UIElement element, int x, int y, int width, int height, bool startInvisible = false)
    {
      UIElement e = element;

      e.offsetX = x;
      e.offsetY = y;
      e.bounds = new Rectangle(x, y, width, height);

      e.label = label;
      e.Canvas = this;

      if (startInvisible)
      {
        e.isVisible = false;
      }

      elements.Add(label, e);

      e.Init();

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
      globalPrompt.Refresh(title, message);

      globalPrompt.Enable();
    }

    public void Update(GameTime gameTime)
    {
      mouse = Mouse.GetState();
      keyboard = Keyboard.GetState();

      wasSomethingClicked = false;

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

                element.Value.OnPress();

                wasSomethingClicked = true;
              }
            }
          }

          if (!wasSomethingClicked)
          {
            SetActiveElement(null);
          }
        }
      }
      else
      {
        //Left mouse button has been released
        if (mouse.LeftButton == ButtonState.Released)
        {
          hasClicked = false;

          foreach (var element in elements)
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

    public void DrawElements(SpriteBatch sb)
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