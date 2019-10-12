namespace FebEngine.Tiles
{
  public class RowBrush : TileBrush
  {
    public override string Name { get; } = "Row";

    public RowBrush(string name, TileBrush left, TileBrush middle, TileBrush right, TileBrush single)
    {
      Name = name;

      Inputs = new TileBrush[4] { left, middle, right, single };
    }

    public override int GetFirstFrame()
    {
      return Inputs[3].GetFirstFrame();
    }

    public override int GetFrame(Tile tile)
    {
      if (tile.Layer.GetTileIndexXY(tile.X - 1, tile.Y) != id && tile.Layer.GetTileIndexXY(tile.X + 1, tile.Y) != id)
      {
        return Inputs[3].GetFrame(tile);
      }

      if (tile.Layer.GetTileIndexXY(tile.X - 1, tile.Y) != id)
      {
        return Inputs[0].GetFrame(tile);
      }
      else if (tile.Layer.GetTileIndexXY(tile.X + 1, tile.Y) != id)
      {
        return Inputs[2].GetFrame(tile);
      }
      else
      {
        return Inputs[1].GetFrame(tile);
      }
    }
  }
}