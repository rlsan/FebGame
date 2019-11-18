using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace FebEngine.GUI
{
  public enum Division
  {
    Horizontal, Vertical
  }

  public enum AnchorPosition
  {
    TopLeft, Top, TopRight,
    Left, Center, Right,
    BottomLeft, Bottom, BottomRight
  }

  public enum ScalingType
  {
    absolute, percentage
  }

  public abstract class GUIElement
  {
    public GUICanvas Canvas { get; set; }
    public GUIElement parentElement;
    public List<GUIElement> childrenElements = new List<GUIElement>();

    public string label;

    public bool isVisible = true;
    public bool IsActive { get { return Canvas.activeElement == this; } }

    public Rectangle bounds;
    public int X { get { return bounds.X; } set { bounds.X = value; } }
    public int Y { get { return bounds.Y; } set { bounds.Y = value; } }
    public int Width { get { return bounds.Width; } set { bounds.Width = value; } }
    public int Height { get { return bounds.Height; } set { bounds.Height = value; } }

    public int realWidth;
    public int realHeight;

    public int offsetX;
    public int offsetY;

    public float percent = 1;

    private bool startInvisible;

    public bool toBeDestroyed;

    public Division division = Division.Vertical;
    public AnchorPosition anchorPosition = AnchorPosition.Center;

    public ScalingType widthScalingType = ScalingType.percentage;
    public ScalingType heightScalingType = ScalingType.percentage;

    public int paddingLeft;
    public int paddingRight;
    public int paddingTop;
    public int paddingBottom;

    public bool isHolding;
    public bool isPressed;
    public bool isReleased;

    public int Padding
    {
      set
      {
        paddingLeft = value;
        paddingRight = value;
        paddingTop = value;
        paddingBottom = value;
      }
    }

    public GUIElement AddElement(GUIElement e, float width = 1, float height = 1, ScalingType widthScalingType = ScalingType.percentage, ScalingType heightScalingType = ScalingType.percentage)
    {
      Canvas.AddChild(e);
      e.Canvas = Canvas;
      e.parentElement = this;
      childrenElements.Add(e);

      e.realWidth = (int)width;
      e.realHeight = (int)height;

      e.widthScalingType = widthScalingType;
      e.heightScalingType = heightScalingType;

      e.percent = percent;
      e.Init();

      return e;
    }

    public GUIContainer AddPanel(float width = 1, float height = 1, ScalingType widthScalingType = ScalingType.percentage, ScalingType heightScalingType = ScalingType.percentage, bool showBackground = false)
    {
      var e = new GUIContainer();

      //Canvas.AddChild(e);
      e.Canvas = Canvas;

      e.parentElement = this;
      childrenElements.Add(e);

      e.realWidth = (int)width;
      e.realHeight = (int)height;

      e.widthScalingType = widthScalingType;
      e.heightScalingType = heightScalingType;

      e.drawBackground = showBackground;

      e.percent = percent;
      e.Init();

      return e;
    }

    public GUIButton AddButton(string title, Action onClick = null, float width = 1, float height = 1, ScalingType widthScalingType = ScalingType.percentage, ScalingType heightScalingType = ScalingType.percentage)
    {
      var e = new GUIButton(title, onClick);

      Canvas.AddChild(e);
      e.Canvas = Canvas;

      e.parentElement = this;
      childrenElements.Add(e);

      e.realWidth = (int)width;
      e.realHeight = (int)height;

      e.widthScalingType = widthScalingType;
      e.heightScalingType = heightScalingType;

      e.Init();

      return e;
    }

    public GUITextField AddTextField(string message = "", float percent = 1)
    {
      var e = new GUITextField(message);

      Canvas.AddChild(e);
      e.Canvas = Canvas;

      e.parentElement = this;
      childrenElements.Add(e);

      e.percent = percent;
      e.Init();

      return e;
    }

    public GUIBar AddBar(string title)
    {
      var e = new GUIBar();
      e.Title = title;

      e.Canvas = Canvas;

      e.parentElement = this;
      childrenElements.Add(e);

      e.widthScalingType = ScalingType.percentage;
      e.heightScalingType = ScalingType.absolute;

      e.percent = percent;
      //e.realWidth = (int)width;
      e.realHeight = 30;
      e.Init();

      return e;
    }

    public GUIText AddText(string message = "", float percent = 1, TextAlignment alignment = TextAlignment.Center)
    {
      var e = new GUIText(message);

      e.Canvas = Canvas;

      e.parentElement = this;
      childrenElements.Add(e);

      e.percent = percent;
      e.alignment = alignment;
      e.Init();

      return e;
    }

    public GUIElement AddChild(string label, GUIElement element, int x = 0, int y = 0, int width = 0, int height = 0, bool startInvisible = false)
    {
      GUIElement e = element;

      this.startInvisible = startInvisible;

      Canvas.AddElement(e, x, y, width, height);

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
      isVisible = false;

      if (IsActive)
      {
        Canvas.SetActiveElement(null);
      }

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

    public void Toggle()
    {
      if (isVisible)
      {
        Disable();
      }
      else
      {
        Enable();
        Canvas.SetActiveElement(this);
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

    public virtual void OnKey(Keys key)
    {
    }

    public virtual void OnHold()
    {
    }

    public virtual void OnPress(Point mousePos)
    {
      foreach (GUIElement element in childrenElements)
      {
        if (element.bounds.Contains(mousePos))
        {
          element.OnPress(mousePos);
        }
      }
    }

    public virtual void OnRelease()
    {
      foreach (GUIElement element in childrenElements)
      {
        element.OnRelease();
      }
    }

    private void Align()
    {
      Rectangle bounds;

      if (parentElement == null)
      {
        bounds = Canvas.bounds;
      }
      else
      {
        bounds = parentElement.bounds;
      }

      switch (anchorPosition)
      {
        case AnchorPosition.TopLeft:
          X = bounds.X;
          Y = bounds.Y;
          break;

        case AnchorPosition.Top:
          X = bounds.X + bounds.Width / 2 - Width / 2;
          Y = bounds.Y;
          break;

        case AnchorPosition.TopRight:
          X = bounds.Width - Width;
          Y = bounds.Y;
          break;

        case AnchorPosition.Left:
          X = bounds.X;
          Y = bounds.Y + bounds.Height / 2 - Height / 2;
          break;

        case AnchorPosition.Center:
          X = bounds.X + bounds.Width / 2 - Width / 2;
          Y = bounds.Y + bounds.Height / 2 - Height / 2;
          break;

        case AnchorPosition.Right:
          X = bounds.Width - Width;
          Y = bounds.Y + bounds.Height / 2 - Height / 2;
          break;

        case AnchorPosition.BottomLeft:
          X = bounds.X;
          Y = bounds.Height - Height;
          break;

        case AnchorPosition.Bottom:
          X = bounds.X + bounds.Width / 2 - Width / 2;
          Y = bounds.Height - Height;
          break;

        case AnchorPosition.BottomRight:
          X = bounds.Width - Width;
          Y = bounds.Height - Height;
          break;
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
        bounds.Location = new Point(Canvas.bounds.X + offsetX, Canvas.bounds.Y + offsetY);
      }

      if (parentElement == null)
      {
        Align();
      }

      float elementsLeft = childrenElements.Count;
      float remainingWidth = Width;
      float remainingHeight = Height;
      float offset = 0;

      foreach (GUIElement element in childrenElements)
      {
        if (division == Division.Vertical)
        {
          if (element.widthScalingType == ScalingType.percentage)
          {
            element.offsetX = element.paddingLeft;
            element.Width = Width;
          }
          else if (element.widthScalingType == ScalingType.absolute)
          {
            element.offsetX = element.paddingLeft;
            element.Width = element.realWidth;
          }

          float size = 0;
          if (element.heightScalingType == ScalingType.percentage)
          {
            size = remainingHeight / elementsLeft * element.percent;

            element.offsetY = (int)offset + element.paddingTop;
            element.Height = (int)size - element.paddingBottom * 2;

            offset += size;
          }
          else if (element.heightScalingType == ScalingType.absolute)
          {
            size = element.realHeight;

            element.offsetY = (int)offset;
            element.Height = element.realHeight;

            offset += size;
          }

          remainingHeight -= size;
          elementsLeft--;
        }
        else if (division == Division.Horizontal)
        {
          float size = remainingWidth / elementsLeft * element.percent;

          if (element.widthScalingType == ScalingType.absolute)
          {
            size = element.realWidth;

            element.offsetX = (int)offset;
            element.Width = element.realWidth;

            offset += size;
          }
          else if (element.widthScalingType == ScalingType.percentage)
          {
            element.offsetX = (int)offset + element.paddingLeft;
            element.Width = (int)size - element.paddingRight * 2;

            offset += size;
          }
          if (element.heightScalingType == ScalingType.percentage)
          {
            element.offsetY = element.paddingTop;
            element.Height = Height;
          }
          else if (element.heightScalingType == ScalingType.absolute)
          {
            element.offsetY = element.paddingTop;
            element.Height = element.realHeight;
          }

          remainingWidth -= size;
          elementsLeft--;
        }

        if (element.isVisible)
        {
          element.Update(gameTime);
        }
      }
    }

    public virtual void Draw(SpriteBatch sb)
    {
      if (isVisible)
      {
        foreach (GUIElement element in childrenElements)
        {
          element.Draw(sb);
        }
      }
    }
  }
}