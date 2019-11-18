using System.Linq;

namespace FebEngine.Tiles
{
  public class AnimatedBrush : TileBrush
  {
    //public override StringBuilder Name { get; set; } = "Animated";

    public int fps = 12;
    public bool random = false;

    public AnimatedBrush(int fps, bool random, params TileBrush[] inputs)
    {
      Inputs = inputs.ToList();

      this.fps = fps;
      this.random = random;

      brushType = TileBrushType.Animated;
    }

    public override int GetPreviewFrame()
    {
      int timeFrame = (int)(Time.CurrentTime * fps) % Inputs.Count;
      var picked = Inputs[timeFrame];

      return picked.GetPreviewFrame();
    }

    public override int GetFrame(Tile tile)
    {
      int timeFrame = 0;
      if (random)
      {
        timeFrame = ((int)(Time.CurrentTime * fps) + tile.Hash) % Inputs.Count;
      }
      else
      {
        timeFrame = (int)(Time.CurrentTime * fps) % Inputs.Count;
      }

      var picked = Inputs[timeFrame];

      return picked.GetFrame(tile);
    }
  }
}