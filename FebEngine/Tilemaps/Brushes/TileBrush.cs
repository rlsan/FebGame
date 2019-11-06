using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FebEngine.Tiles
{
  public class TileBrush
  {
    public Tileset tileset;
    virtual public string Name { get; set; }
    public TileBrushType brushType;

    public int id;
    public int FrameId { get; }

    public List<TileBrush> Inputs { get; set; } = new List<TileBrush>();

    public bool HasInputs
    {
      get
      {
        if (Inputs != null)
        {
          return Inputs.Count > 0;
        }

        return false;
      }
    }

    public bool isHidden;

    public TileBrush(string name = "Tile", int frameId = -1, bool isHidden = false)
    {
      brushType = TileBrushType.Tile;

      Name = name;
      FrameId = frameId;
      this.isHidden = isHidden;
    }

    public void AddInput(TileBrush brush)
    {
      Inputs.Add(brush);
    }

    /// <summary>
    /// Quickly return a frame without a tile reference. Used for creating thumbnails.
    /// </summary>
    public virtual int GetFirstFrame()
    {
      return FrameId;
    }

    public virtual int GetFrame(Tile tile)
    {
      return FrameId;
    }
  }

  public enum TileBrushType
  {
    Tile, Row, Column, Random, Animated
  }
}