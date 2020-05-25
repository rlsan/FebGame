using System;

namespace Fubar
{
  public static class Mathf
  {
    /// <summary>
    /// Returns true if the difference between both values is less than epsilon.
    /// </summary>
    public static bool Approx(float a, float b, float e)
    {
      return Math.Abs(a - b) <= e;
    }

    /// <summary>
    /// Clamps the value between two specified values.
    /// </summary>
    public static float Clamp(float x, float min, float max)
    {
      return Math.Max(min, Math.Min(max, x));
    }

    /// <summary>
    /// Clamps the value between 0 and 1.
    /// </summary>
    public static float Clamp01(float x)
    {
      return Math.Max(0, Math.Min(1, x));
    }

    /// <summary>
    /// Interpolates between two values.
    /// </summary>
    public static float Lerp(float a, float b, float f)
    {
      return (a * (1f - f)) + (b * f);
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