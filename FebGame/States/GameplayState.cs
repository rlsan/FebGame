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
  internal class GameplayState : GameState
  {
    private Sprite player;
    private Sprite thing;
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
      player = world.AddSprite(new Player());
      thing = world.create.Sprite("missing");

      timer = world.create.Timer();
      timer.Start(5, player.Kill);
    }

    public override void Update(GameTime gameTime)
    {
      Debug.Text(timer.ToString(), 1000, 0);
    }

    public override void Draw(RenderManager renderer)
    {
      var sb = renderer.SpriteBatch;
      renderer.GraphicsDevice.Clear(Color.CornflowerBlue);

      renderer.SpriteBatch.Begin();
      Debug.Draw(sb);
      renderer.SpriteBatch.End();
    }
  }
}