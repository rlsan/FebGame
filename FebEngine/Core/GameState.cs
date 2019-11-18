using FebEngine.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FebEngine
{
  public abstract class GameState
  {
    public string name;

    public bool IsActive { get; set; } = false;
    public bool IsLoaded { get; set; } = false;

    public bool isLocked;

    public WorldManager world;
    public Camera Camera { get { return world.camera; } }
    public GUICanvas canvas;
    public Game game;

    public Factory Create { get; set; }

    public void Initialize()
    {
      Create = new Factory(world, this, game.Content);
    }

    public virtual void Activate()
    {
      IsActive = true;

      Console.WriteLine("Activated state: {0}", name);
    }

    public virtual void Deactivate()
    {
      IsActive = false;

      Console.WriteLine("Deactivated state: {0}", name);
    }

    public virtual void Load(ContentManager content)
    {
      IsLoaded = true;

      canvas = new GUICanvas(1920, 1080);
      canvas.Font = content.Load<SpriteFont>("ui");
      canvas.Theme = content.Load<Texture2D>("theme");

      Console.WriteLine("Loaded state: {0}", name);
    }

    public virtual void Unload(ContentManager content)
    {
      IsLoaded = false;

      canvas.Clear();
      //canvas = null;
      canvas = new GUICanvas(0, 0);

      Console.WriteLine("Unloaded state: {0}", name);
    }

    public virtual void Start()
    {
    }

    public virtual void Update(GameTime gameTime)
    {
    }

    public virtual void Draw(RenderManager renderer, GameTime gameTime)
    {
    }
  }
}