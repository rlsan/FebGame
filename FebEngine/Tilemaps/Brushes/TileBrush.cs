namespace FebEngine.Tiles
{
  public class TileBrush
  {
    public Tileset tileset;
    virtual public string Name { get; }

    public int id;
    public int FrameId { get; }

    public TileBrush[] Inputs { get; set; }

    public bool HasInputs
    {
      get
      {
        if (Inputs != null)
        {
          return Inputs.Length > 0;
        }

        return false;
      }
    }

    public bool isHidden;

    public TileBrush(string name = "Tile", int frameId = -1, bool isHidden = false)
    {
      Name = name;
      FrameId = frameId;
      this.isHidden = isHidden;
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
}