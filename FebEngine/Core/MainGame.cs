using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace FebEngine
{
  public class MainGame : Game
  {
    public GraphicsDeviceManager Graphics { get; set; }
    public List<Manager> Managers;

    public RenderManager renderManager;
    public InputManager inputManager;
    public Audio audioManager;
    public StateManager stateManager;
    public WorldManager worldManager;

    public Config Config { get; }

    public MainGame()
    {
      Graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";

      Managers = new List<Manager>();

      renderManager = new RenderManager(this);
      inputManager = new InputManager(this);
      audioManager = new Audio(this);
      worldManager = new WorldManager(this);
      stateManager = new StateManager(this);

      Config = new Config(this);

      Config.OpenConfigFile(@"\Data\config.cfg");
      Config.OpenInputFile(@"\Data\input.cfg");
    }

    protected override void Initialize()
    {
      foreach (var manager in Managers) manager.Initialize();

      base.Initialize();
    }

    protected override void LoadContent()
    {
      foreach (var manager in Managers) manager.LoadContent(Content);

      base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
      Time.Update(gameTime);
      Debug.Clear();

      foreach (var manager in Managers) manager.Update(gameTime);

      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      foreach (var manager in Managers) manager.Draw(gameTime);

      base.Draw(gameTime);
    }

    public void AddManager(Manager m)
    {
      Managers.Add(m);
    }
  }
}