using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FebEngine.UI
{
  public class UIElement
  {
    public string label;
    public UICanvas canvas;
    public UIElement parentElement;
    public List<UIElement> childrenElements = new List<UIElement>();

    public bool isVisible = true;

    public Rectangle bounds;

    public int X { get { return bounds.X; } set { bounds.X = value; } }
    public int Y { get { return bounds.Y; } set { bounds.Y = value; } }

    public void Add(UIElement element)
    {
      var e = element;
      e.canvas = canvas;
      e.parentElement = this;
      childrenElements.Add(e);
    }

    public virtual void Draw(SpriteBatch sb)
    {
      if (isVisible)
      {
        foreach (var element in childrenElements)
        {
          element.Draw(sb);
        }
      }
    }
  }
}