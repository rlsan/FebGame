namespace FebEngine.Tiles
{
  public class RowBrush : TileBrush
  {
    public override string Name { get; } = "Row";

    public RowBrush(string name, TileBrush left, TileBrush middle, TileBrush right)
    {
      Name = name;

      Children = new TileBrush[3] { left, middle, right };

      foreach (var child in Children)
      {
        child.Parent = this;
      }
    }

    public override int GetFrame(Tile tile)
    {
      if (tile.Layer.GetTileIndexXY(tile.X - 1, tile.Y) != id)
      {
        return Children[0].GetFrame(tile);
      }
      else if (tile.Layer.GetTileIndexXY(tile.X + 1, tile.Y) != id)
      {
        return Children[2].GetFrame(tile);
      }
      else
      {
        return Children[1].GetFrame(tile);
      }
    }
  }
}