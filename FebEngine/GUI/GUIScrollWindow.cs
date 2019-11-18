using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FebEngine.GUI
{
  public class GUIScrollWindow<T> : GUIContainer
  {
    private Action<T> onSelect;
    private List<T> items = new List<T>();

    private List<T> visibleItems;

    private int maxVisibleSize;
    private int visibleSize;
    private int startPosition;

    public int spacing = 26;

    public GUIScrollWindow(int size = 10, Action<T> onSelect = null, params T[] items)
    {
      maxVisibleSize = size;
      this.onSelect = onSelect;
      SetItems(items);
    }

    public override void Init()
    {
      var content = AddPanel();
      content.division = Division.Horizontal;

      var scrollBar = content.AddPanel(20, 200, ScalingType.absolute);

      scrollBar.division = Division.Vertical;

      scrollBar.AddButton("^", ScrollUp);
      scrollBar.AddButton("v", ScrollDown);

      drawBackground = true;

      base.Init();
    }

    public override void OnPress(Point mousePos)
    {
      base.OnPress(mousePos);

      int index = (Canvas.mouse.Y - Y) / spacing;

      if (items.Count > startPosition + index)
      {
        if (onSelect != null)
        {
          onSelect.DynamicInvoke(items[startPosition + index]);
        }
      }
    }

    public void SetItems(params T[] newItems)
    {
      items.Clear();

      for (int i = 0; i < newItems.Length; i++)
      {
        items.Add(newItems[i]);
      }

      visibleSize = Math.Min(items.Count, maxVisibleSize);

      //ResetScroll();
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
        /*
        if (Canvas.keyboard.IsKeyDown(Keys.W))
        {
          ScrollUp();
        }
        if (Canvas.keyboard.IsKeyDown(Keys.S))
        {
          ScrollDown();
        }
        */
      }

      GetVisibleItems();

      base.Update(gameTime);
    }

    public override void Draw(SpriteBatch sb)
    {
      base.Draw(sb);
      for (int i = 0; i < visibleItems.Count; i++)
      {
        string itemName = visibleItems[i].ToString();

        if (itemName.Length > 50)
        {
          itemName = itemName.Remove(50);
        }

        sb.DrawString(Canvas.Font, "   " + itemName, new Vector2(X, Y + i * spacing), Color.White);
      }
    }
  }
}