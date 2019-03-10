namespace FebEngine
{
  public class Tile
  {
    public int id;
    public int[] frames;
    public TileType[] properties;

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

    public void Reset()
    {
      id = -1;
      frames = new int[] { -1 };
      properties = new TileType[] { 0 };
    }
  }
}