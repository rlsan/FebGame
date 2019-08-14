using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FebEngine.Utility;
using Microsoft.Xna.Framework.Content;

namespace FebEngine
{
  public class RenderManager : Manager
  {
    public static RenderManager instance;

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
      if (instance == null)
      {
        instance = this;
      }

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

      // Remove these two lines if weird time-based stuff starts happening.
      Graphics.SynchronizeWithVerticalRetrace = false;
      Game.IsFixedTimeStep = false;

      base.Initialize();
    }

    public override void LoadContent(ContentManager content)
    {
      Debug.fontTexture = content.Load<Texture2D>("debug2");
    }

    public override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.CornflowerBlue);
      //Debug.Clear();

      //Initial layer for sprites and other visual gameplay elements

      SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Game.world.camera.TransformMatrix);

      foreach (var ent in Game.world.entities)
      {
        ent.Draw(SpriteBatch, gameTime);
      }

      foreach (var state in Game.stateManager.states.Values)
      {
        if (state.isActive)
        {
          state.Draw(Game.renderManager, gameTime);
        }
      }

      SpriteBatch.End();

      //Post layer for UI and debug

      SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

      Game.uiManager.canvas.DrawElements(SpriteBatch);
      Debug.Draw(SpriteBatch);

      SpriteBatch.End();
    }
  }
}