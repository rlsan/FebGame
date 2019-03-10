namespace FebEngine.Tiles
{
  public class RowTile : Tile
  {
    public override string Name { get; } = "Row";

    public RowTile(Tile left, Tile middle, Tile right)
    {
      children = new Tile[3] { left, middle, right };
    }

    public override int ReturnFrame(TilemapLayer layer, int x, int y)
    {
      Tile t;

      if (layer.GetTileIndexXY(x - 1, y) != id)
      {
        t = children[0];
      }
      else if (layer.GetTileIndexXY(x + 1, y) != id)
      {
        t = children[2];
      }
      else
      {
        t = children[1];
      }

      return t.ReturnFrame(layer, x, y);
    }
  }
}