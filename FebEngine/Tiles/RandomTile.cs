namespace FebEngine.Tiles
{
  public class RandomTile : Tile
  {
    public override string Name { get; set; } = "Random";

    public float[] probabilityValues;

    public RandomTile(bool hidden, params Tile[] tiles)
    {
      this.hidden = hidden;

      foreach (var child in children)
      {
        child.parent = this;
      }

      children = tiles;
    }

    public override int ReturnFrame(TilemapLayer layer, int x, int y)
    {
      var pickedTile = children[layer.hashArray[x, y] % children.Length];

      return pickedTile.ReturnFrame(layer, x, y);
    }
  }
}