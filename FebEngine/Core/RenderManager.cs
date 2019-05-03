using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FebEngine.Utility;

namespace FebEngine
{
  public class RenderManager : Manager
  {
    public SpriteBatch SpriteBatch { get; private set; }

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

      // Setup debug
      Texture2D pixelTexture = new Texture2D(GraphicsDevice, 4, 4);

      Color[] colorData = new Color[16];
      for (int i = 0; i < 16; i++)
      {
        colorData[i] = new Color(255, 255, 255, 1f);
      }
      pixelTexture.SetData(colorData);

      Debug.pixelTexture = pixelTexture;

      base.Initialize();
    }

    public override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.CornflowerBlue);

      SpriteBatch.Begin();

      foreach (var ent in Game.world.entities)
      {
        ent.Draw(SpriteBatch, gameTime);
      }

      Debug.Draw(SpriteBatch);

      SpriteBatch.End();
    }
  }
}