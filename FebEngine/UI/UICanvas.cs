using Microsoft.Xna.Framework;
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
    public ContentManager contentManager;

    public UICanvas(Texture2D texture)
    {
      elements = new Dictionary<string, UIElement>();
      ThemeTexture = texture;
    }

    public void AddElement(string label, UIElement element)
    {
      var e = element;
      e.label = label;
      e.canvas = this;
      elements.Add(label, e);
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
      if (elements.ContainsKey(label))
      {
        return elements[label];
      }

      return null;
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