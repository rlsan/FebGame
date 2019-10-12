using FebEngine.UI;
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
    public UICanvas canvas;
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

      canvas = new UICanvas(0, 0);
      canvas.ThemeTexture = content.Load<Texture2D>("theme");

      Console.WriteLine("Loaded state: {0}", name);
    }

    public virtual void Unload(ContentManager content)
    {
      IsLoaded = false;

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