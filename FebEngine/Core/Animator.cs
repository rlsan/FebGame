using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexturePackerLoader;

namespace FebEngine
{
  public class Animator
  {
    public Dictionary<string, Animation> Animations { get; set; }

    public SpriteSheet spriteSheet;

    public Animation current;

    public Animator()
    {
      Animations = new Dictionary<string, Animation>();
    }

    public void Add(string name, string path, float framerate = 12, bool loop = true)
    {
      var a = new Animation(name, path, framerate, loop, spriteSheet);

      Animations.Add(name, a);

      if (current == null)
      {
        current = a;
      }
    }

    public void Play(string name)
    {
      Animations.TryGetValue(name, out Animation animation);

      if (animation != null)
      {
        current = animation;
      }
    }
  }

  public class Animation
  {
    public string Name { get; }
    public List<SpriteFrame> frames;

    public int frame;

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