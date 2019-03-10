using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebEngine.Tiles
{
  public class TileProperties
  {
    private TileType[] Properties { get; set; }

    public TileType Property
    {
      get
      {
        return Properties[0];
      }
    }

    public TileProperties()
    {
      Properties = new TileType[] { 0 };
    }

    public override string ToString()
    {
      string s = "null";

      if (Properties != null)
      {
        s = "";

        for (int i = 0; i < Properties.Length; i++)
        {
          s += Properties[i].ToString();

          if (i < Properties.Length - 1)
          {
            s += ", ";
          }
        }
      }

      return s;
    }
  }
}