using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace FebEngine.Utility
{
  public static class Time
  {
    private static GameTime _GameTime { get; set; }

    public static float CurrentTime => (float)_GameTime.TotalGameTime.TotalSeconds;
    public static float DeltaTime => (float)_GameTime.ElapsedGameTime.TotalSeconds;

    public static float SinTime => (float)Math.Sin(CurrentTime);
    public static float CosTime => (float)Math.Cos(CurrentTime);

    public static void Update(GameTime gameTime)
    {
      _GameTime = gameTime;
    }
  }
}