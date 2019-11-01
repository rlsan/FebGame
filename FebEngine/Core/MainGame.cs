using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FebEngine.Utility;
using FebEngine.Tiles;

namespace FebEngine
{
  public class MainGame : Game
  {
    public GraphicsDeviceManager Graphics { get; set; }
    public List<Manager> Managers;

    public RenderManager renderManager;
    public StateManager stateManager;

    public WorldManager worldManager;

    public MainGame()
    {
      //Window.IsBorderless = true;

      Graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";

      Managers = new List<Manager>();

      stateManager = new StateManager(this);
      worldManager = new WorldManager(this);
      renderManager = new RenderManager(this);

      MapIO.content = Content;
    }

    protected override void Initialize()
    {
      stateManager.Initialize();
      renderManager.Initialize();

      base.Initialize();
    }

    protected override void LoadContent()
    {
      foreach (var manager in Managers)
      {
        manager.LoadContent(Content);
      }

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