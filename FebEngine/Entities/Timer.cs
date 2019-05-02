using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FebEngine
{
  public class Timer : Entity
  {
    public float time;
    public float duration;
    public bool isFinished;
    public bool isRunning;

    private Delegate callback;

    public void Start(float duration, Action callback)
    {
      time = duration;
      this.duration = duration;
      this.callback = callback;

      isFinished = false;
      isRunning = true;
    }

    public void Stop()
    {
      if (!isFinished)
      {
        isRunning = false;
      }
    }

    public void Resume()
    {
      if (!isFinished)
      {
        isRunning = true;
      }
    }

    public void Reset()
    {
      time = duration;

      isFinished = false;
      isRunning = true;
    }

    public override string ToString()
    {
      TimeSpan formattedTime = TimeSpan.FromSeconds(time);
      return formattedTime.ToString("mm':'ss':'ff");
    }

    public override void Update(GameTime gt)
    {
      if (isRunning)
      {
        var delta = (float)gt.ElapsedGameTime.TotalSeconds;

        time -= delta;

        if (time <= 0)
        {
          isFinished = true;
          isRunning = false;

          time = 0;

          callback.DynamicInvoke();
        }
      }
    }

    public override void Draw(SpriteBatch sb, GameTime gt)
    {
    }
  }
}