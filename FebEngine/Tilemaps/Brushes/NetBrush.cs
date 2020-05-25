namespace Fubar.Tiles
{
  public class NetBrush : TileBrush
  {
    //public override string Name { get; set; } = "Net";

    public NetBrush(string name, TileBrush single, TileBrush left, TileBrush middle, TileBrush right)
    {
      //Name = name;

      //Inputs = new TileBrush[4] { left, middle, right, single };

      AddInput(single);

      AddInput(left);
      AddInput(middle);
      AddInput(right);

      brushType = TileBrushType.Row;
    }

    public override int GetPreviewFrame()
    {
      return Inputs[0].GetPreviewFrame();
    }

    public override int GetFrame(Tile tile)
    {
      if (tile.Layer.GetTileIndexXY(tile.X - 1, tile.Y) != id && tile.Layer.GetTileIndexXY(tile.X + 1, tile.Y) != id)
      {
        return Inputs[0].GetFrame(tile);
      }

      if (tile.Layer.GetTileIndexXY(tile.X - 1, tile.Y) != id)
      {
        return Inputs[1].GetFrame(tile);
      }
      else if (tile.Layer.GetTileIndexXY(tile.X + 1, tile.Y) != id)
      {
        return Inputs[3].GetFrame(tile);
      }
      else
      {
        return Inputs[2].GetFrame(tile);
      }
    }
  }
}