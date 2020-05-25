using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fubar
{
  public class Camera : Entity
  {
    public Matrix Transform;
    public float scaleFactor = 1f;
    public Rectangle Bounds { get; protected set; }
    public Rectangle VisibleArea { get; protected set; }

    public Entity Target { get; set; }

    private float shakeAmount;
    private float shakeTimer;

    public Rectangle Viewport
    {
      get
      {
        return new Rectangle(
        (int)(Position.X - (Bounds.Width / scaleFactor / 2)),
        (int)(Position.Y - (Bounds.Height / scaleFactor / 2)),
        (int)(Bounds.Width / scaleFactor),
        (int)(Bounds.Height / scaleFactor));
      }
    }

    public Camera()
    {
      Transform = new Matrix();
    }

    public Vector2 ToWorld(Vector2 vector)
    {
      var matrix = Matrix.Invert(Transform);
      return Vector2.Transform(vector, matrix);
    }

    public Vector2 ToWorld(Point point)
    {
      return ToWorld(point.ToVector2());
    }

    public Vector2 ToScreen(Vector2 vector)
    {
      var matrix = Transform;
      return Vector2.Transform(vector, Transform);
    }

    public Vector2 ToScreen(Point point)
    {
      return ToScreen(point.ToVector2());
    }

    public void Shake(float intensity, float duration = 0.5f)
    {
      shakeTimer = duration;
      shakeAmount = intensity;
    }

    public void Follow(Entity target)
    {
      Target = target;
    }

    public void Unfollow()
    {
      Target = null;
    }

    private void UpdateVisibleArea()
    {
      var inverseViewMatrix = Matrix.Invert(Transform);

      var tl = Vector2.Transform(Vector2.Zero, inverseViewMatrix);
      var tr = Vector2.Transform(new Vector2(Bounds.X, 0), inverseViewMatrix);
      var bl = Vector2.Transform(new Vector2(0, Bounds.Y), inverseViewMatrix);
      var br = Vector2.Transform(new Vector2(Bounds.Width, Bounds.Height), inverseViewMatrix);

      var min = new Vector2(
          MathHelper.Min(tl.X, MathHelper.Min(tr.X, MathHelper.Min(bl.X, br.X))),
          MathHelper.Min(tl.Y, MathHelper.Min(tr.Y, MathHelper.Min(bl.Y, br.Y))));
      var max = new Vector2(
          MathHelper.Max(tl.X, MathHelper.Max(tr.X, MathHelper.Max(bl.X, br.X))),
          MathHelper.Max(tl.Y, MathHelper.Max(tr.Y, MathHelper.Max(bl.Y, br.Y))));
      VisibleArea = new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
    }

    private void UpdateMatrix()
    {
      Transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
              Matrix.CreateScale(scaleFactor) *
              Matrix.CreateTranslation(new Vector3(Bounds.Width * 0.5f, Bounds.Height * 0.5f, 0));
      UpdateVisibleArea();
    }

    public void Update(Viewport viewport)
    {
      shakeTimer -= Time.DeltaTime;

      if (shakeTimer > 0)
      {
        Position += RNG.PointInsideUnitCircle() * shakeAmount * shakeTimer;
      }
      if (Target != null)
      {
        Position = Vector2.Lerp(Position, Target.Position, 0.1f);
      }

      Bounds = viewport.Bounds;
      UpdateMatrix();
    }
  }
}