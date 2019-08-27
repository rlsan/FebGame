using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FebEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using FebEngine.Entities;

namespace FebGame.Entities
{
  public class Player : Actor
  {
    public Player()
    {
      Name = "Player";
      Id = 1;
    }

    public override void Update(GameTime gt)
    {
      KeyboardState kb = Keyboard.GetState();

      if (kb.IsKeyDown(Keys.A))
      {
        //transform.Position += Vector2.One;
      }
    }
  }
}