namespace FebEngine.Tiles
{
  public class RandomBrush : TileBrush
  {
    public override string Name { get; } = "Random";

    public float[] probabilityValues;

    public RandomBrush(string name, params TileBrush[] inputs)
    {
      Name = name;

      Inputs = inputs;
    }

    public override int GetFirstFrame()
    {
      return Inputs[0].GetFirstFrame();
    }

    public override int GetFrame(Tile tile)
    {
      var pickedTile = Inputs[tile.Hash % Inputs.Length];

      return pickedTile.GetFrame(tile);
    }
  }
}