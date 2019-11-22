using System;
using Microsoft.Xna.Framework;

namespace FebEngine
{
  public class Timer : Entity
  {
    public float time;
    public float duration;
    public bool isFinished;
    public bool isRunning;

    private Delegate onFinish;

    public void Start(float duration, Action onFinish)
    {
      time = duration;
      this.duration = duration;
      this.onFinish = onFinish;

      isFinished = false;
      isRunning = true;
    }

    public void Stop()
    {
      if (!isFinished) isRunning = false;
    }

    public void Resume()
    {
      if (!isFinished) isRunning = true;
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
        //var delta = (float)gt.ElapsedGameTime.TotalSeconds;

        time -= Time.DeltaTime;

        if (time <= 0)
        {
          isFinished = true;
          isRunning = false;

          time = 0;

          if (onFinish != null)
          {
            onFinish.DynamicInvoke();
          }
        }
      }
    }
  }
}