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
using FebEngine.Entities;
using FebEngine.Utility;

namespace FebGame.States
{
  internal class GameplayState : GameState
  {
    private Actor player;
    private Actor thing;
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
      Debug.Text(timer.ToString(), 1000, 0);

      KeyboardState ks = Keyboard.GetState();

      if (ks.IsKeyDown(Keys.A))
      {
        thing.Body.velocity.X += -0.2f;
      }
      if (ks.IsKeyDown(Keys.D))
      {
        thing.Body.velocity.X += 0.2f;
      }

      if (ks.IsKeyDown(Keys.W))
      {
        if (thing.Body.OnFloor)
        {
          thing.Body.velocity.Y = -8f;
        }
      }

      if (thing.Position.Y > 800)
      {
        thing.Position = Vector2.Zero;
        thing.Body.Reset();
      }
    }

    public override void Draw(RenderManager renderer, GameTime gameTime)
    {
    }
  }
}