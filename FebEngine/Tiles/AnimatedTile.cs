namespace FebEngine.Tiles
{
  public class AnimatedTile : Tile
  {
    public override string Name { get; } = "Animated";

    public int fps;

    public AnimatedTile(params Tile[] tiles)
    {
      children = tiles;
    }

    /*
    public override int ReturnFrame(TilemapLayer layer)
    {
      for (int i = 0; i < children.Length; i++)
        return children[i].ReturnFrame(layer);
    }
    */
  }
}