﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FebEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FebGame.Entities
{
  public class Player : Sprite
  {
    public Player()
    {
      TexturePath = "foods";
    }

    public override void Update(GameTime gt)
    {
      KeyboardState kb = Keyboard.GetState();

      if (kb.IsKeyDown(Keys.A))
      {
        transform.Position += Vector2.One;
      }
    }
  }
}