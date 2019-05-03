using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FebEngine.Utility;

namespace FebEngine
{
  public class MainGame : Game
  {
    public GraphicsDeviceManager Graphics { get; set; }
    public List<Manager> Managers;

    public RenderManager renderManager;
    public StateManager stateManager;
    public World world;

    public MainGame()
    {
      Graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";

      Graphics.PreferredBackBufferWidth = 1920;
      Graphics.PreferredBackBufferHeight = 1080;

      Managers = new List<Manager>();

      renderManager = new RenderManager(this);
      stateManager = new StateManager(this);
      world = new World(this);
    }

    protected override void Initialize()
    {
      renderManager.Initialize();
      stateManager.Initialize();

      base.Initialize();
    }

    protected override void LoadContent()
    {
      Debug.fontTexture = Content.Load<Texture2D>("debug2");

      base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
      foreach (var manager in Managers)
      {
        manager.Update(gameTime);
      }

      Time.Update(gameTime);
      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      foreach (var manager in Managers)
      {
        manager.Draw(gameTime);
      }

      base.Draw(gameTime);
    }

    public void AddManager(Manager m)
    {
      Managers.Add(m);
    }
  }
}