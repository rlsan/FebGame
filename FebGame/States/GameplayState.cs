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

namespace FebGame.States
{
  internal class GameplayState : GameState
  {
    private Timer timer;

    public override void Load(ContentManager content)
    {
    }

    public override void Unload(ContentManager content)
    {
      content.Unload();
    }

    public override void Start()
    {
      /*
      player = world.AddSprite(new Player());

      timer = world.create.Timer();
      timer.Start(0.2f, player.Kill);

      for (int i = 0; i < 40; i++)
      {
        var obsticle = world.create.Sprite("missing");

        obsticle.Transform.Position = new Vector2(RNG.RandIntRange(0, 800), RNG.RandIntRange(0, 800));

        world.physics.Enable(obsticle);

        obsticle.Body.hasGravity = false;
        obsticle.Body.isDynamic = false;

        obsticle.Body.collidesUp = true;
        obsticle.Body.collidesDown = false;
        obsticle.Body.collidesLeft = false;
        obsticle.Body.collidesRight = false;
      }

      thing = world.create.Sprite("missing");

      world.physics.Enable(thing);
      thing.Body.gravity = 25;
      */
    }

    public override void Update(GameTime gameTime)
    {
    }

    public override void Draw(RenderManager renderer, GameTime gameTime)
    {
    }
  }
}