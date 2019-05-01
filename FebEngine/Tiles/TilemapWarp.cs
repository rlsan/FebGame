using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebEngine.Tiles
{
  public class TilemapWarp
  {
    public int tileX;
    public int tileY;

    public Tilemap destinationMap;

    public int destTileX;
    public int destTileY;

    public TilemapWarp(int tileX, int tileY)
    {
      this.tileX = tileX;
      this.tileY = tileY;
      //this.destinationMap = destinationMap;
    }
  }
}