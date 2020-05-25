using Microsoft.Xna.Framework;

namespace Fubar
{
  public class Warp
  {
    public string DestinationMapName { get; }
    public Vector2 DestinationPosition { get; }

    public Warp(string destinationMapName, Vector2 destinationPosition)
    {
      DestinationMapName = destinationMapName;
      DestinationPosition = destinationPosition;
    }
  }

  public class SideWarp
  {
    public string DestinationMapName { get; }

    public WarpDirection Direction { get; }

    public float RangeMin { get; }
    public float RangeMax { get; }

    public SideWarp(string destinationMapName, float rangeMin, float rangeMax, WarpDirection direction)
    {
      DestinationMapName = destinationMapName;

      RangeMin = rangeMin;
      RangeMax = rangeMax;

      Direction = direction;
    }
  }

  public enum WarpDirection
  {
    Up, Down, Left, Right
  }
}