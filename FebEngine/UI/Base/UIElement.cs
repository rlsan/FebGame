using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FebEngine.UI
{
  public enum Division
  {
    horizontal, vertical
  }

  public abstract class UIElement
  {
    public UICanvas Canvas { get; set; }
    public UIElement parentElement;
    public List<UIElement> childrenElements = new List<UIElement>();

    public string label;

    public bool isVisible = true;
    public bool IsActive { get { return Canvas.activeElement == this; } }

    public Rectangle bounds;
    public int X { get { return bounds.X; } set { bounds.X = value; } }
    public int Y { get { return bounds.Y; } set { bounds.Y = value; } }
    public int Width { get { return bounds.Width; } set { bounds.Width = value; } }
    public int Height { get { return bounds.Height; } set { bounds.Height = value; } }

    public int offsetX;
    public int offsetY;

    public float percent = 1;

    private bool startInvisible;

    public bool toBeDestroyed;

    public Division division = Division.vertical;

    public UIElement AddElement(UIElement e, float percent = 1)
    {
      e.Canvas = Canvas;

      e.parentElement = this;
      childrenElements.Add(e);

      e.percent = percent;
      e.Init();

      return e;
    }

    public UIContainer AddPanel(float percent = 1)
    {
      var e = new UIContainer();

      e.Canvas = Canvas;

      e.parentElement = this;
      childrenElements.Add(e);

      e.percent = percent;
      e.Init();

      return e;
    }

    public UIButton AddButton(string title, Action onClick = null)
    {
      var e = new UIButton(title, onClick);

      e.Canvas = Canvas;

      e.parentElement = this;
      childrenElements.Add(e);

      e.Init();

      return e;
    }

    public UITextField AddTextField(string message = "", float percent = 1)
    {
      var e = new UITextField(message);

      e.Canvas = Canvas;

      e.parentElement = this;
      childrenElements.Add(e);

      e.percent = percent;
      e.Init();

      return e;
    }

    public UITextBox AddText(string message = "", float percent = 1)
    {
      var e = new UITextBox(message);

      e.Canvas = Canvas;

      e.parentElement = this;
      childrenElements.Add(e);

      e.percent = percent;
      e.Init();

      return e;
    }

    public UIElement AddChild(string label, UIElement element, int x = 0, int y = 0, int width = 0, int height = 0, bool startInvisible = false)
    {
      UIElement e = element;

      this.startInvisible = startInvisible;

      Canvas.AddElement(this.label + label, e, x, y, width, height);

      e.parentElement = this;
      childrenElements.Add(e);

      if (startInvisible)
      {
        e.Disable();
      }

      return e;
    }

    public virtual void Init()
    {
      if (startInvisible)
      {
        Disable();
      }
    }

    public void Disable()
    {
      if (IsActive)
      {
        Canvas.SetActiveElement(null);
      }

      isVisible = false;

      foreach (var child in childrenElements)
      {
        child.Disable();
      }
    }

    public void Enable()
    {
      isVisible = true;

      foreach (var child in childrenElements)
      {
        child.Enable();
      }
    }

    public void Remove()
    {
      toBeDestroyed = true;

      foreach (var child in childrenElements)
      {
        child.Remove();
      }
    }

    public virtual void OnHold()
    {
    }

    public virtual void OnPress(Point mousePos)
    {
      foreach (UIElement element in childrenElements)
      {
        if (element.bounds.Contains(mousePos))
        {
          element.OnPress(mousePos);
        }
      }
    }

    public virtual void OnRelease()
    {
      foreach (UIElement element in childrenElements)
      {
        element.OnRelease();
      }
    }

    public virtual void Update(GameTime gameTime)
    {
      if (parentElement != null)
      {
        bounds.Location = parentElement.bounds.Location + new Point(offsetX, offsetY);
      }
      else
      {
        bounds.Location = new Point(offsetX, offsetY);
      }

      float elementsLeft = childrenElements.Count;
      float remainingWidth = Width;
      float remainingHeight = Height;
      float offset = 0;

      foreach (UIElement element in childrenElements)
      {
        if (division == Division.vertical)
        {
          float size = remainingHeight / elementsLeft * element.percent;

          element.offsetY = (int)(offset);
          element.Height = (int)size;

          element.offsetX = 0;
          element.Width = Width;

          offset += size;
          remainingHeight -= size;

          elementsLeft--;
        }
        else if (division == Division.horizontal)
        {
          float size = remainingWidth / elementsLeft * element.percent;

          element.offsetX = (int)(offset);
          element.Width = (int)size;

          element.offsetY = 0;
          element.Height = Height;

          offset += size;
          remainingWidth -= size;

          elementsLeft--;
        }

        element.Update(gameTime);
      }
    }

    public virtual void Draw(SpriteBatch sb)
    {
      //if (isVisible)
      //{
      foreach (UIElement element in childrenElements)
      {
        element.Draw(sb);
      }
      //}
    }
  }
}