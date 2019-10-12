using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebEngine.Utility
{
  public static class Mathf
  {
    public static float Min(float a, float b, float min)
    {
      return Math.Min(a, b);
    }

    /// <summary>
    /// Returns true if the difference between both values is less than epsilon.
    /// </summary>
    public static bool Approx(float a, float b, float e)
    {
      return Math.Abs(a - b) <= e;
    }

    public static bool ApproxVec(Vector2 a, Vector2 b, float e)
    {
      return Math.Abs(a.X - b.X) <= e && Math.Abs(a.Y - b.Y) <= e;
    }

    public static int RoundToGrid(float input, int gridSize, float offset = 0)
    {
      return (int)(Math.Round(input / gridSize) * gridSize + (offset % gridSize));
    }

    public static int CeilingToGrid(float input, int gridSize, float offset = 0)
    {
      return (int)(Math.Ceiling(input / gridSize) * gridSize + (offset % gridSize));
    }

    public static int FloorToGrid(float input, int gridSize, float offset = 0)
    {
      return (int)(Math.Floor(input / gridSize) * gridSize + (offset % gridSize));
    }
  }
}