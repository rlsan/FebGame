using Microsoft.Xna.Framework;

namespace FebEngine
{
  public abstract class Manager
  {
    public MainGame Game { get; }

    public Manager(MainGame game)
    {
      Game = game;
    }

    public virtual void Initialize()
    {
      Game.AddManager(this);
    }

    public virtual void LoadContent()
    {
    }

    public virtual void UnloadContent()
    {
    }

    public virtual void Update(GameTime gameTime)
    {
    }

    public virtual void Draw(GameTime gameTime)
    {
    }
  }
}