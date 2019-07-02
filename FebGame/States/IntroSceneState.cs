using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FebEngine;
using FebGame.Entities;

namespace FebGame.States
{
  internal class IntroSceneState : GameState
  {
    public override void Load(ContentManager content)
    {
    }

    public override void Unload(ContentManager content)
    {
      content.Unload();
    }

    public override void Start()
    {
    }

    public override void Update(GameTime gameTime)
    {
    }

    public override void Draw(RenderManager renderer, GameTime gameTime)
    {
    }
  }
}