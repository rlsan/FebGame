using FebEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebGame.Emitters
{
  public class Stars : ParticleEmitter
  {
    public Stars()
    {
      var spriteSheetFramesPath = "Effect/WhitePuff";

      Capacity = 100;
      shape = EmitterShape.Circle;
      radius = 10;

      velocity = new Microsoft.Xna.Framework.Vector2(50, 50);
      gravity = 0;
      randomAmount = 1.0f;
      minSpeed = 0;
      maxSpeed = 1;

      minLifetime = .1f;
      maxLifetime = .4f;
    }
  }
}