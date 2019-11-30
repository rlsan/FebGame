using System;
using Microsoft.Xna.Framework;

namespace FebEngine
{
  public static class Time
  {
    private static GameTime _GameTime { get; set; }

    public static float CurrentTime => (float)_GameTime.TotalGameTime.TotalSeconds;

    public static float DeltaTime => (float)_GameTime.ElapsedGameTime.TotalSeconds;

    public static float Sin => (float)Math.Sin(CurrentTime);
    public static float Cos => (float)Math.Cos(CurrentTime);

    public static void Update(GameTime gameTime)
    {
      _GameTime = gameTime;
    }
  }
}