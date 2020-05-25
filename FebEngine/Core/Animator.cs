using System.Collections.Generic;
using TexturePackerLoader;

namespace Fubar
{
  public class Animator
  {
    private Dictionary<string, Animation> Animations { get; set; }
    public SpriteSheet spriteSheet;
    public Animation current;

    public SpriteFrame CurrentFrame
    {
      get
      {
        return current.frames[current.CurrentFrame];
      }
    }

    public Animator()
    {
      Animations = new Dictionary<string, Animation>();
    }

    public bool IsAnimationPlaying(string animationName)
    {
      return current.Name == animationName;
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
        if (animation != current)
        {
          animation.startTime = Time.CurrentTime;
          current = animation;
        }
      }
    }
  }
}