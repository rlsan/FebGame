using FebEngine;
using FebEngine.Tiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace FebGame
{
  public class MapCompiler
  {
    /// <summary>
    /// Compiles a list of thumbnails and adds their maps to the provided map group.
    /// </summary>
    public static void Compile(MapGroup mapGroup, List<MapThumbnail> thumbnails)
    {
      mapGroup.Reset();

      // Iterate through all thumbnails.
      foreach (var thumbnail in thumbnails)
      {
        // Reset all warps.
        thumbnail.tilemap.sideWarps.Clear();

        // Test every thumbnail against this one.
        foreach (var other in thumbnails)
        {
          // Do not test against itself.
          if (other == thumbnail) continue;
          // Names should not be identical.
          if (other.tilemap.Name == thumbnail.tilemap.Name) return;
          // Maps should not intersect each other.
          if (other.Bounds.Intersects(thumbnail.Bounds)) return;

          // Extend the bounds of this thumbnail to see if it just barely overlaps another.
          Rectangle inflated = new Rectangle(thumbnail.Bounds.Location, thumbnail.Bounds.Size);
          inflated.Inflate(thumbnail.gridSize, thumbnail.gridSize);

          // If this thumbnail touches another...
          if (other.Bounds.Intersects(inflated))
          {
            // To the right...
            if (thumbnail.X + thumbnail.Width == other.X)
              AddSideWarp(thumbnail, other, WarpDirection.Right);

            // To the left...
            else if (thumbnail.X == other.X + other.Width)
              AddSideWarp(thumbnail, other, WarpDirection.Left);

            // Below...
            else if (thumbnail.Y + thumbnail.Height == other.Y)
              AddSideWarp(thumbnail, other, WarpDirection.Down);

            // Above...
            else
              AddSideWarp(thumbnail, other, WarpDirection.Up);
          }
        }

        // Update the thumbnail's tilemap and add it to the group.
        thumbnail.RefreshTilemap();
        mapGroup.AddMap(thumbnail.tilemap);
      }
    }

    private static void AddSideWarp(MapThumbnail thumbnail, MapThumbnail other, WarpDirection direction)
    {
      if (direction == WarpDirection.Left || direction == WarpDirection.Right)
      {
        var start = Math.Max(thumbnail.Bounds.Top, other.Bounds.Top);
        var end = Math.Min(thumbnail.Bounds.Bottom, other.Bounds.Bottom);

        // Make coordinates relative to the map's position.
        start = (start - thumbnail.Y) / thumbnail.gridSize;
        end = (end - thumbnail.Y) / thumbnail.gridSize;

        thumbnail.tilemap.sideWarps.Add(new SideWarp(other.tilemap.Name, start, end, direction));
      }
      else
      {
        var start = Math.Max(thumbnail.Bounds.Left, other.Bounds.Left);
        var end = Math.Min(thumbnail.Bounds.Right, other.Bounds.Right);

        // Make coordinates relative to the map's position.
        start = (start - thumbnail.X) / thumbnail.gridSize;
        end = (end - thumbnail.X) / thumbnail.gridSize;

        thumbnail.tilemap.sideWarps.Add(new SideWarp(other.tilemap.Name, start, end, direction));
      }
    }
  }
}