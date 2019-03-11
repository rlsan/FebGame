using Microsoft.Xna.Framework;
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

    public UICanvas()
    {
      elements = new Dictionary<string, UIElement>();
    }

    public void AddElement(string label, UIElement element, Rectangle bounds)
    {
      var e = element;
      e.label = label;
      e.canvas = this;
      e.bounds = bounds;
      elements.Add(label, e);
    }

    public UIElement GetElement(string label)
    {
      return elements[label];
    }

    public void DrawElements(SpriteBatch sb)
    {
      foreach (var element in elements)
      {
        element.Value.Draw(sb);
      }
    }
  }
}