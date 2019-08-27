namespace FebEngine.Tiles
{
  public class TileBrush
  {
    virtual public string Name { get; } = "Tile";

    public int id;
    public int FrameId { get; }

    public TileBrush Parent { get; set; }
    public TileBrush[] Children { get; set; }

    public TileBrush(string name = "Tile", int frameId = -1)
    {
      Name = name;
      FrameId = frameId;
    }

    public virtual int GetFrame(Tile tile)
    {
      return FrameId;
    }
  }
}