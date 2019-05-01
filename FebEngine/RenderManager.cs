using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FebEngine
{
  public class RenderManager : Manager
  {
    public SpriteBatch SpriteBatch { get; private set; }
    //public Dictionary<string, Texture2D> Textures { get; private set; }

    public GraphicsDeviceManager Graphics
    {
      get { return Game.Graphics; }
      set { Game.Graphics = value; }
    }

    public GraphicsDevice GraphicsDevice
    {
      get { return Game.GraphicsDevice; }
    }

    public RenderManager(MainGame game) : base(game)
    {
      base.Initialize();
    }

    public override void Initialize()
    {
      SpriteBatch = new SpriteBatch(GraphicsDevice);
      //Textures = new Dictionary<string, Texture2D>();

      base.Initialize();
    }

    public override void Draw(GameTime gameTime)
    {
      SpriteBatch.Begin();

      foreach (var ent in Game.world.entities)
      {
        ent.Draw(SpriteBatch, gameTime);
      }

      //Debug.Draw(SpriteBatch);

      SpriteBatch.End();
    }
  }
}