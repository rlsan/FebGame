using Microsoft.Xna.Framework;

namespace FebEngine
{
  public struct RaycastHit
  {
    public Body Collider { get; }
    public Vector2 Point { get; }
    public float Distance { get; }

    public RaycastHit(Body collider, Vector2 point, float distance)
    {
      Collider = collider;
      Point = point;
      Distance = distance;
    }
  }
}