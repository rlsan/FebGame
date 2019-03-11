﻿namespace FebEngine.Tiles
{
  public class AnimatedTile : Tile
  {
    public override string Name { get; } = "Animated";

    public int fps = 12;
    public bool random = false;

    public AnimatedTile(int fps, bool random, params Tile[] tiles)
    {
      children = tiles;
      this.fps = fps;
      this.random = random;
    }

    public override int ReturnFrame(TilemapLayer layer, int x, int y)
    {
      int timeFrame = 0;
      if (random)
        timeFrame = ((int)(Time.Seconds * fps) + layer.hashArray[x, y]) % children.Length;
      else
        timeFrame = (int)(Time.Seconds * fps) % children.Length;

      var picked = children[timeFrame];

      return picked.ReturnFrame(layer, x, y);
    }
  }
}