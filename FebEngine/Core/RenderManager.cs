using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FebEngine.Utility;
using Microsoft.Xna.Framework.Content;

namespace FebEngine
{
  public class RenderManager : Manager
  {
    public static RenderManager instance;

    public ContentManager Content
    {
      get { return Game.Content; }
      set { Game.Content = value; }
    }

    public SpriteBatch SpriteBatch { get; private set; }

    private Viewport viewport;

    public int VirtualWidth = 1920;
    public int VirtualHeight = 1080;

    public int ScreenWidth = 1920;
    public int ScreenHeight = 1080;

    private float RatioX { get { return (float)viewport.Width / VirtualWidth; } }
    private float RatioY { get { return (float)viewport.Height / VirtualHeight; } }

    private RenderTarget2D ViewRenderTarget;

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

      ViewRenderTarget = new RenderTarget2D(
        GraphicsDevice,
        VirtualWidth,
        VirtualHeight,
        false,
        SurfaceFormat.Color,
        DepthFormat.None,
        1,
        RenderTargetUsage.DiscardContents
        );

      Graphics.PreferredBackBufferWidth = ScreenWidth;
      Graphics.PreferredBackBufferHeight = ScreenHeight;

      Graphics.ApplyChanges();

      SetupDebug();

      // Remove these two lines if weird time-based stuff starts happening.
      Graphics.SynchronizeWithVerticalRetrace = false;
      Game.IsFixedTimeStep = false;

      base.Initialize();
    }

    private void SetupDebug()
    {
      Texture2D pixelTexture = new Texture2D(GraphicsDevice, 4, 4);

      Color[] colorData = new Color[16];
      for (int i = 0; i < 16; i++)
      {
        colorData[i] = new Color(255, 255, 255, 1f);
      }
      pixelTexture.SetData(colorData);

      Debug.pixelTexture = pixelTexture;
    }

    public override void LoadContent(ContentManager content)
    {
      Debug.fontTexture = content.Load<Texture2D>("debug2");
    }

    public override void Draw(GameTime gameTime)
    {
      Debug.Clear();

      // Initial layer for sprites and other visual gameplay elements.

      GraphicsDevice.SetRenderTarget(ViewRenderTarget);

      GraphicsDevice.Clear(Color.Black);

      foreach (var state in Game.stateManager.states.Values)
      {
        if (state.IsActive)
        {
          // Entity layer.

          SpriteBatch.Begin(transformMatrix: Game.worldManager.camera.Transform);

          foreach (KeyValuePair<Entity, GameState> entity in Game.worldManager.entities)
          {
            // Draw if the entity's state matches the state being iterated on.
            if (entity.Value == state && entity.Key.FollowCamera == true)
            {
              entity.Key.Draw(SpriteBatch, gameTime);
            }
          }

          state.Draw(Game.renderManager, gameTime);

          SpriteBatch.End();

          // Post canvas layer

          SpriteBatch.Begin();

          if (state.canvas != null)
          {
            state.canvas.Draw(SpriteBatch, gameTime);
          }

          SpriteBatch.End();
        }
      }

      // Post debug layer.

      SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
      Debug.Draw(SpriteBatch);
      SpriteBatch.End();

      GraphicsDevice.SetRenderTarget(null);
      // Draw rendertarget.

      SpriteBatch.Begin();
      SpriteBatch.Draw(ViewRenderTarget, new Rectangle(0, 0, ScreenWidth, ScreenHeight), Color.White);
      SpriteBatch.End();
    }
  }
}