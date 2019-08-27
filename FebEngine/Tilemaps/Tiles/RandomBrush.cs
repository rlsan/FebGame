namespace FebEngine.Tiles
{
  public class RandomBrush : TileBrush
  {
    public override string Name { get; } = "Random";

    public float[] probabilityValues;

    public RandomBrush(params TileBrush[] tiles)
    {
      foreach (var child in Children)
      {
        child.Parent = this;
      }

      Children = tiles;
    }

    public override int GetFrame(Tile tile)
    {
      /*
      var pickedTile = Children[tile.Layer.hashArray[tile.X, tile.Y] % Children.Length];

      return pickedTile.GetFrame(tile);
      */

      return 0;
    }
  }
}