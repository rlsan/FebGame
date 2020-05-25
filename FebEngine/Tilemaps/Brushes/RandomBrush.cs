using System.Linq;

namespace Fubar.Tiles
{
  public class RandomBrush : TileBrush
  {
    //public override string Name { get; set; } = "Random";

    public float[] probabilityValues;

    public RandomBrush(string name, params TileBrush[] inputs)
    {
      //Name = name;

      Inputs = inputs.ToList();

      brushType = TileBrushType.Random;
    }

    public override int GetPreviewFrame()
    {
      if (HasInputs) return Inputs[0].GetPreviewFrame();

      return 0;
    }

    public override int GetFrame(Tile tile)
    {
      var pickedTile = Inputs[tile.Hash % Inputs.Count];

      return pickedTile.GetFrame(tile);
    }
  }
}