using System.Linq;

namespace FebEngine.Tiles
{
  public class RandomBrush : TileBrush
  {
    public override string Name { get; set; } = "Random";

    public float[] probabilityValues;

    public RandomBrush(string name, params TileBrush[] inputs)
    {
      Name = name;

      Inputs = inputs.ToList();

      brushType = TileBrushType.Random;
    }

    public override int GetFirstFrame()
    {
      if (HasInputs) return Inputs[0].GetFirstFrame();

      return 0;
    }

    public override int GetFrame(Tile tile)
    {
      var pickedTile = Inputs[tile.Hash % Inputs.Count];

      return pickedTile.GetFrame(tile);
    }
  }
}