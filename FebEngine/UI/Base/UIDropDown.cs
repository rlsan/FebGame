using FebEngine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebEngine.UI
{
  public class UIDropDown : UIElement
  {
    private string title = "";
    private List<string> items;

    private bool isPressed;
    private bool isOpen;

    public UIDropDown(string title, params string[] items)
    {
      this.title = title;
      SetList(items);
    }

    public void SetList(params string[] items)
    {
      this.items = items.ToList();
    }

    public override void OnPress(Point mousePos)
    {
      isPressed = true;
    }

    public override void OnRelease()
    {
      if (isPressed)
      {
        if (isOpen == false)
        {
          isOpen = true;
        }
        else if (isOpen == true)
        {
          isOpen = false;
        }
        /*
        if (onClick != null)
        {
          onClick.DynamicInvoke();
        }
        */
      }
      isPressed = false;
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);

      if (isOpen)
      {
        Height = 1000;
      }
    }

    public override void Draw(SpriteBatch sb)
    {
      Debug.Text(title, X + 2, Y + 2);

      if (isOpen)
      {
        for (int i = 0; i < items.Count; i++)
        {
          Debug.Text(items[i], X, Y + 16 * i);
        }
      }

      Color color = Color.White;
      if (isPressed)
      {
        color = Color.Gray;
      }

      sb.Draw(Canvas.ThemeTexture,
        new Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height),
        new Rectangle(0, 0, 16, 16),
        color
          );

      base.Draw(sb);
    }
  }
}