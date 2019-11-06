using FebEngine.Utility;
using System.Linq;

namespace FebEngine.Tiles
{
  public class AnimatedBrush : TileBrush
  {
    public override string Name { get; set; } = "Animated";

    public int fps = 12;
    public bool random = false;

    public AnimatedBrush(int fps, bool random, params TileBrush[] inputs)
    {
      Inputs = inputs.ToList();

      this.fps = fps;
      this.random = random;

      brushType = TileBrushType.Animated;
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