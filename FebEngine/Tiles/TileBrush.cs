namespace FebEngine.Tiles
{
  public class TileBrush
  {
    public int id;
    public int[] frames;
    public TileType[] properties;
    public Tile rawTile;

    public bool IsAnimated { get { return frames.Length > 0; } }

    public string PropertiesToString
    {
      get
      {
        string s = "null";

        if (properties != null)
        {
          s = "";

          for (int i = 0; i < properties.Length; i++)
          {
            s += properties[i].ToString();

            if (i < properties.Length - 1)
            {
              s += ", ";
            }
          }
        }

        return s;
      }
    }
  }
}