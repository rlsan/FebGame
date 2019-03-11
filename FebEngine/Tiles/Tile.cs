using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace FebEngine.Tiles
{
  public class Tile
  {
    virtual public string Name { get; } = "Tile";

    public int id = -1;
    public TileType[] properties;

    public Tile[] children = new Tile[0];
    public Color tint = Color.White;

    public int frame = 0;
    public bool hidden = false;

    public int X { get; set; }
    public int Y { get; set; }

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
            if (i < properties.Length - 1) s += ", ";
          }
        }
        return s;
      }
    }

    public Tile()
    {
      properties = new TileType[] { TileType.None };
    }

    public void Reset()
    {
      id = -1;
      properties = new TileType[] { 0 };
    }

    public virtual int ReturnFirstFrame()
    {
      if (Name == "Tile")
      {
        return id;
      }
      else
      {
        return children[0].ReturnFirstFrame();
      }
    }

    public virtual int ReturnFrame(TilemapLayer layer, int x, int y)
    {
      if (Name == "Tile")
      {
        return id;
      }
      else
      {
        return children[0].ReturnFrame(layer, x, y);
      }
    }
  }
}