using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebEngine.Physics
{
  public class Body
  {
    public Rectangle bounds;
    public Vector2 velocity;
    public Vector2 maxVelocity = new Vector2(float.PositiveInfinity);

    public bool enabled = true;

    public Sprite Parent { get; }

    public Body(Sprite Parent)
    {
      this.Parent = Parent;

      bounds = new Rectangle(0, 0, Parent.Texture.Width, Parent.Texture.Height);
    }

    public bool hasGravity = true;
    public bool collidesWithWorldBounds = false;

    public Blocked blocked;

    public bool OnFloor { get { return blocked.Down; } }
    public bool OnWall { get { return blocked.Left || blocked.Right; } }
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