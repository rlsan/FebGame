using FebEngine;
using FebEngine.Entities;
using FebEngine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebGame.States
{
  public class TestState : GameState
  {
    public Sprite foo;

    public List<Sprite> bub = new List<Sprite>();

    public override void Draw(RenderManager renderer, GameTime gameTime)
    {
      base.Draw(renderer, gameTime);
    }

    public override void Load(ContentManager content)
    {
      foo = Create.Sprite("foo", "cerv");
      foo.CenterOrigin();

      for (int i = 0; i < 30; i++)
      {
        bub.Add(Create.Sprite("bub" + i));
      }

      base.Load(content);
    }

    public override void Start()
    {
      base.Start();
    }

    public override void Unload(ContentManager content)
    {
      base.Unload(content);
    }

    public override void Update(GameTime gameTime)
    {
      foo.Position = new Vector2(
        (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 2) * 100,
        (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 3) * 100
        );

      for (int i = 0; i < bub.Count; i++)
      {
        bub[i].Position = new Vector2(
        (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 0.1f + i) * 800,
        (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 0.14f + i) * 800
        );
      }

      base.Update(gameTime);
    }
  }
}