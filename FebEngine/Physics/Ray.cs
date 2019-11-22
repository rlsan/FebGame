using Microsoft.Xna.Framework;

namespace FebEngine
{
  public struct Ray
  {
    public Vector2 Origin { get; }
    public Vector2 Direction { get; }

    public Ray(Vector2 origin, Vector2 direction)
    {
      Origin = origin;
      Direction = direction;
    }
  }
}