using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FebEngine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FebEngine.UI
{
  public class UIScrollWindow : UIElement
  {
    private Action<string> onSelect;
    private List<string> items = new List<string>();

    private List<string> visibleItems;

    private int maxVisibleSize;
    private int visibleSize;
    private int startPosition;

    public UIScrollWindow(int size = 10, Action<string> onSelect = null, params string[] items)
    {
      maxVisibleSize = size;
      this.onSelect = onSelect;
      SetItems(items);
    }

    public override void Init()
    {
      AddChild(label + "ScrollUp", new UIButton(title: "^", onClick: ScrollUp), Width, 0, 25, 25);
      AddChild(label + "ScrollDown", new UIButton(title: "v", onClick: ScrollDown), Width, 25, 25, 25);

      base.Init();
    }

    public override void OnRelease()
    {
      int index = (Canvas.mouse.Y - Y) / 20;

      if (items.Count > startPosition + index)
      {
        if (onSelect != null)
        {
          onSelect.DynamicInvoke(items[startPosition + index]);
        }
      }
    }

    public void SetItems(params string[] newItems)
    {
      items.Clear();

      for (int i = 0; i < newItems.Length; i++)
      {
        items.Add(newItems[i]);
      }

      visibleSize = Math.Min(items.Count, maxVisibleSize);

      ResetScroll();
    }

    private void GetVisibleItems()
    {
      visibleItems = items.GetRange(startPosition, visibleSize);
    }

    public void ResetScroll()
    {
      startPosition = 0;
    }

    public void Scroll(int amount)
    {
      startPosition += amount;

      startPosition = Math.Min(items.Count - visibleSize, startPosition);
      startPosition = Math.Max(0, startPosition);
    }

    public void ScrollUp()
    {
      Scroll(-1);
    }

    public void ScrollDown()
    {
      Scroll(1);
    }

    public override void Update(GameTime gameTime)
    {
      if (IsActive)
      {
        if (Canvas.keyboard.IsKeyDown(Keys.W))
        {
          ScrollUp();
        }
        if (Canvas.keyboard.IsKeyDown(Keys.S))
        {
          ScrollDown();
        }
      }

      GetVisibleItems();

      base.Update(gameTime);
    }

    public override void Draw(SpriteBatch sb)
    {
      sb.Draw(Canvas.ThemeTexture,
        new Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height),
        new Rectangle(0, 16, 16, 16),
        Color.White
          );

      for (int i = 0; i < visibleItems.Count; i++)
      {
        string itemName = visibleItems[i];

        if (itemName.Length > 50)
        {
          itemName = itemName.Remove(50);
        }

        Debug.Text(itemName, X, Y + (20 * i));
      }

      base.Draw(sb);
    }
  }
}