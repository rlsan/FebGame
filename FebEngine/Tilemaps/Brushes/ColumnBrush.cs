namespace FebEngine.Tiles
{
  public class ColumnBrush : TileBrush
  {
    public override string Name { get; } = "Column";

    public ColumnBrush(TileBrush top, TileBrush middle, TileBrush bottom)
    {
      Inputs = new TileBrush[3] { top, middle, bottom };
    }

    public override int GetFrame(Tile tile)
    {
      TileBrush t;

      if (tile.Layer.GetTileIndexXY(tile.X, tile.Y - 1) != id)
      {
        t = Inputs[0];
      }
      else if (tile.Layer.GetTileIndexXY(tile.X, tile.Y + 1) != id)
      {
        t = Inputs[2];
      }
      else
      {
        t = Inputs[1];
      }

      return t.GetFrame(tile);
    }
  }
}