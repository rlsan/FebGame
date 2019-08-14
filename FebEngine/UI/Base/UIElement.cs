using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FebEngine.UI
{
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

    private bool startInvisible;

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

    public virtual void OnHold()
    {
    }

    public virtual void OnPress(Point mousePos)
    {
    }

    public virtual void OnRelease()
    {
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

      foreach (UIElement element in childrenElements)
      {
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