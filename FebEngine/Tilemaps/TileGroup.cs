using System.Collections.Generic;

namespace Fubar
{
  public class aTile
  {
    private int ID;
  }

  public class TileGroup
  {
    private Dictionary<Tile, int> TilePalette;
  }

  public class aTilemap
  {
    public Grid<aTile> tiles;
  }
}