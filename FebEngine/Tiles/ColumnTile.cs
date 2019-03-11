namespace FebEngine.Tiles
{
  public class ColumnTile : Tile
  {
    public override string Name { get; } = "Column";

    public ColumnTile(bool hidden, Tile top, Tile middle, Tile bottom)
    {
      this.hidden = hidden;
      children = new Tile[3] { top, middle, bottom };
    }

    public override int ReturnFrame(TilemapLayer layer, int x, int y)
    {
      Tile t;

      if (layer.GetTileIndexXY(x, y - 1) != id)
      {
        t = children[0];
      }
      else if (layer.GetTileIndexXY(x, y + 1) != id)
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