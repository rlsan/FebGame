namespace Fubar.Tiles
{
  public class ColumnBrush : TileBrush
  {
    //public override string Name { get; set; } = "Column";

    public ColumnBrush(string name, TileBrush single, TileBrush top, TileBrush middle, TileBrush bottom)
    {
      //Name = name;
      //Inputs = new TileBrush[3] { top, middle, bottom };

      AddInput(single);
      AddInput(top);
      AddInput(middle);
      AddInput(bottom);

      brushType = TileBrushType.Column;
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