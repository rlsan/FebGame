using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Fubar
{
  public abstract class Manager
  {
    public MainGame Game { get; }

    public Manager(MainGame game)
    {
      Game = game;
      Game.AddManager(this);
    }

    public virtual void Initialize()
    {
    }

    public virtual void LoadContent(ContentManager content)
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