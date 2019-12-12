using Microsoft.Xna.Framework;
using System;

namespace FebEngine
{
  public static class Extensions
  {
    /// <summary>
    /// Limits the vector by another vector.
    /// </summary>
    public static Vector2 Limit(this Vector2 a, Vector2 b)
    {
      if (Math.Abs(a.X) > b.X) a.X = b.X * Math.Sign(a.X);
      if (Math.Abs(a.Y) > b.Y) a.Y = b.Y * Math.Sign(a.Y);

      return a;
    }

    /// <summary>
    /// Interpolates between two vectors.
    /// </summary>
    public static Vector2 Lerp(Vector2 start, Vector2 end, float percent)
    {
      return start + percent * (end - start);
    }

    /// <summary>
    /// Returns true if both vectors are within e.
    /// </summary>
    public static bool Approx(this Vector2 a, Vector2 b, float e)
    {
      return Math.Abs(a.X - b.X) <= e && Math.Abs(a.Y - b.Y) <= e;
    }

    public static Rectangle Clamp(this Rectangle smaller, Rectangle larger)
    {
      Rectangle rect;

      rect.X = Math.Max(smaller.X, larger.X);
      rect.Y = Math.Max(smaller.Y, larger.Y);

      rect.Width = smaller.Width;
      rect.Height = smaller.Height;
      /*
      rect.X = Math.Max(smaller.X, larger.X);
      rect.Y = Math.Max(smaller.Y, larger.Y);
      rect.Width = Math.Min(smaller.X + smaller.Width, larger.X + larger.Width) - rect.X;
      rect.Height = Math.Min(smaller.Y + smaller.Height, larger.Y + larger.Height) - rect.Y;
      */
      return rect;
    }
  }
}