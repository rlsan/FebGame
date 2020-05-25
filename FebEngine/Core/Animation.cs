using System.Collections.Generic;
using TexturePackerLoader;

namespace Fubar
{
  public class Animation
  {
    public string Name { get; }
    public List<SpriteFrame> frames;

    public float startTime;

    public int CurrentFrame
    {
      get
      {
        return (int)((startTime + Time.CurrentTime) * Framerate) % frames.Count;
      }
    }

    public float Framerate { get; set; }
    public bool Loop { get; set; } = true;

    public Animation(string name, string path, float framerate, bool loop, SpriteSheet spriteSheet)
    {
      Name = name;
      Framerate = framerate;
      Loop = loop;

      frames = new List<SpriteFrame>();

      foreach (var item in spriteSheet.spriteList)
      {
        if (item.Key.StartsWith(path))
        {
          frames.Add(item.Value);
        }
      }
    }
  }
}