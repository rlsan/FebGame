using FebEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebGame.States
{
  class State1 : IState
  {
    public void Load()
    {
      Sprite mySprite = SpriteFactory.CreateSprite();
    }

    public void Unload()
    {
    }

    public void Update(GameTime gameTime)
    {
    }

    public void Draw(Renderer renderer)
    {
    }
  }
}