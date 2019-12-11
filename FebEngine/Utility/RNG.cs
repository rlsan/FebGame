using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebEngine
{
  public static class RNG
  {
    private static Random r = new Random();

    /// <summary>
    /// Returns a random value between 0 and 1.
    /// </summary>
    public static float Random()
    {
      return (float)r.NextDouble();
    }

    public static float Random(int seed)
    {
      var r = new Random(seed);

      return (float)r.NextDouble();
    }

    /// <summary>
    /// Returns a random value between -1 and 1.
    /// </summary>
    public static float Normal()
    {
      return RandRange(-1, 1);
    }

    public static Color RandomColor()
    {
      var c = new Color(Random(), Random(), Random());

      return c;
    }

    /// <summary>
    /// Returns a random value within a specific range.
    /// </summary>
    public static float RandRange(float min, float max)
    {
      return (float)r.NextDouble() * (max - min) + min;
    }

    /// <summary>
    /// Returns a random integer.
    /// </summary>
    public static int RandInt()
    {
      return r.Next();
    }

    public static int RandInt(int seed)
    {
      var r = new Random(seed);

      return r.Next();
    }

    /// <summary>
    /// Returns a random integer within a specific range.
    /// </summary>
    public static int RandIntRange(int min, int max)
    {
      return r.Next(min, max);
    }

    public static int RandIntRange(int min, int max, int seed)
    {
      var r = new Random(seed);

      return r.Next(min, max);
    }

    /// <summary>
    /// Randomly chooses an element from an array.
    /// </summary>
    public static object Pick(object[] array)
    {
      return array[r.Next(array.Length)];
    }

    public static object Pick(object[] array, int seed)
    {
      return array[r.Next(array.Length)];
    }

    /// <summary>
    /// Returns a random vector with a magnitude of 1.
    /// </summary>
    public static Vector2 PointOnUnitCircle()
    {
      return Vector2.Normalize(new Vector2(Normal(), Normal()));
    }

    /// <summary>
    /// Returns a random vector with a magnitude less than or equal to 1.
    /// </summary>
    public static Vector2 PointInsideUnitCircle()
    {
      var pt_angle = Random() * 2 * Math.PI;
      var pt_radius_sq = Random();
      float pt_x = (float)(Math.Sqrt(pt_radius_sq) * Math.Cos(pt_angle));
      float pt_y = (float)(Math.Sqrt(pt_radius_sq) * Math.Sin(pt_angle));

      return new Vector2(pt_x, pt_y);
    }

    /// <summary>
    /// Returns a random point inside the given rectangle.
    /// </summary>
    public static Vector2 PointInRectangle(Rectangle rectangle)
    {
      float x = RandRange(rectangle.X, rectangle.X + rectangle.Width);
      float y = RandRange(rectangle.Y, rectangle.Y + rectangle.Height);

      return new Vector2(x, y);
    }
  }
}