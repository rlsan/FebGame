using FebEngine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebEngine.UI
{
  public class UIWindow : UIElement
  {
    public string title;

    public bool isDraggable = false;
    public bool isDragging;
    public bool isCloseable;
    public Point mouseOffset;

    public int menuBarHeight = 20;

    public UIWindow(string title = "", bool isDraggable = false, bool isCloseable = true)
    {
      this.title = title;
      this.isDraggable = isDraggable;
      this.isCloseable = isCloseable;
    }

    public override void Init()
    {
      if (isCloseable)
      {
        //AddChild(label + "Close", new UIButton(title: "x", onClick: Disable), Width - 20, 0, 20, 20);
      }

      base.Init();
    }

    public override void OnPress(Point mousePos)
    {
      if (isDraggable)
      {
        if (Canvas.mouse.Y < bounds.Top + menuBarHeight)
        {
          if (!isDragging)
          {
            Canvas.SetActiveElement(this);
            isDragging = true;
            mouseOffset = bounds.Location - Canvas.mouse.Position;
          }
        }
      }

      base.OnPress(mousePos);
    }

    public override void OnRelease()
    {
      isDragging = false;

      base.OnRelease();
    }

    public override void Update(GameTime gameTime)
    {
      if (isDragging)
      {
        var drag = Canvas.mouse.Position + mouseOffset;
        offsetX = drag.X;
        offsetY = drag.Y;
      }

      base.Update(gameTime);
    }

    public override void Draw(SpriteBatch sb)
    {
      if (isVisible)
      {
        Debug.Text(title, X + 2, Y + 2);

        //Draw window
        sb.Draw(Canvas.ThemeTexture,
          new Rectangle(bounds.X + 1, bounds.Y + 1, bounds.Width - 1, bounds.Height - 1),
          new Rectangle(18, 2, 10, 10),
          Color.White
            );

        //Draw menu bar
        /*
        sb.Draw(Canvas.ThemeTexture,
          new Rectangle(bounds.X, bounds.Y, bounds.Width, menuBarHeight),
          new Rectangle(0, 0, 16, 16),
          Color.White
            );
            */

        base.Draw(sb);
      }
    }
  }
}