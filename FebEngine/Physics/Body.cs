using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebEngine
{
  public class CollisionArgs : EventArgs
  {
    public Sprite Primary { get; set; }
    public Sprite Other { get; set; }
  }

  public class Body
  {
    public int xOffset;
    public int yOffset;
    public int width;
    public int height;

    public Rectangle Bounds
    {
      get
      {
        int scaledXOffset = xOffset * (int)Parent.Scale.X;
        int scaledYOffset = yOffset * (int)Parent.Scale.Y;
        int scaledWidth = width * (int)Parent.Scale.X;
        int scaledHeight = height * (int)Parent.Scale.Y;

        return new Rectangle(
          scaledXOffset + (int)Parent.Position.X - scaledWidth / 2,
          scaledYOffset + (int)Parent.Position.Y - scaledHeight / 2,
          scaledWidth, scaledHeight);
      }
    }

    public Vector2 velocity;
    public Vector2 maxVelocity = new Vector2(float.PositiveInfinity);

    public bool enabled = true;

    public float gravity;

    //will this body query for other bodies?
    public bool isDynamic = true;

    public bool collidesLeft = true;
    public bool collidesRight = true;
    public bool collidesUp = true;
    public bool collidesDown = true;

    public List<int> collisionLayers = new List<int>();

    public Sprite Parent { get; }

    public bool isTrigger = false;

    public Body(Sprite Parent)
    {
      this.Parent = Parent;

      xOffset = 0;
      yOffset = 0;
      width = Parent.Bounds.Width;
      height = Parent.Bounds.Height;

      SetLayers(0);
    }

    public bool hasGravity = true;
    public bool collidesWithWorldBounds = false;

    public Blocked blocked;

    public bool OnFloor { get { return blocked.Down; } }
    public bool OnWall { get { return blocked.Left || blocked.Right; } }

    public void SetBounds(int xOffset, int yOffset, int width, int height)
    {
      this.xOffset = xOffset;
      this.yOffset = yOffset;
      this.width = width;
      this.height = height;
    }

    public void SetLayers(params int[] layers)
    {
      collisionLayers.Clear();

      for (int i = 0; i < layers.Length; i++)
      {
        collisionLayers.Add(layers[i]);
      }
    }

    public void AddToLayer(int layer)
    {
      collisionLayers.Add(layer);
    }

    public void Reset()
    {
      //velocity = Vector2.Zero;
      blocked.Reset();
    }
  }

  public struct Blocked
  {
    public bool Up, Down, Left, Right;

    public void Reset()
    {
      Up = false;
      Down = false;
      Left = false;
      Right = false;
    }

    public override string ToString()
    {
      return "Up: " + Up + ", Down: " + Down + ", Left: " + Left + ", Right: " + Right;
    }
  }
}