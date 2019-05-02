using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace FebEngine
{
  public static class Time
  {
    private static GameTime _GameTime { get; set; }

    public static float Seconds => (float)_GameTime.TotalGameTime.TotalSeconds;
    public static float ElapsedSeconds => (float)_GameTime.ElapsedGameTime.TotalSeconds;

    public static void Update(GameTime gameTime)
    {
      _GameTime = gameTime;
    }
  }
}