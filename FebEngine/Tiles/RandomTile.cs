namespace FebEngine.Tiles
{
  public class RandomTile : Tile
  {
    public override string Name { get; } = "Random";

    public float[] probabilityValues;

    public RandomTile(params Tile[] tiles)
    {
      children = tiles;
    }

    public override int ReturnFrame(TilemapLayer layer, int x, int y)
    {
      var pickedTile = children[layer.hashArray[x, y] % children.Length];

      //Debug.Text(x + ", " + y, x * 16, y * 16);

      return pickedTile.ReturnFrame(layer, x, y);
    }
  }
}