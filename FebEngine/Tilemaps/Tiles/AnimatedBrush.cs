using FebEngine.Utility;

namespace FebEngine.Tiles
{
  public class AnimatedBrush : TileBrush
  {
    public override string Name { get; } = "Animated";

    public int fps = 12;
    public bool random = false;

    public AnimatedBrush(int fps, bool random, params TileBrush[] tiles)
    {
      Children = tiles;

      foreach (var child in Children)
      {
        child.Parent = this;
      }

      this.fps = fps;
      this.random = random;
    }

    public override int GetFrame(Tile tile)
    {
      /*
      int timeFrame = 0;
      if (random)
        timeFrame = ((int)(Time.Seconds * fps) + tile.Layer.hashArray[tile.X, tile.Y]) % Children.Length;
      else
        timeFrame = (int)(Time.Seconds * fps) % Children.Length;

        var picked = Children[timeFrame];

      return picked.GetFrame(tile);
      */

      return 0;
    }
  }
}