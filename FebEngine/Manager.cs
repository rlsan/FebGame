using Microsoft.Xna.Framework;

namespace FebEngine
{
  public abstract class Manager
  {
    public Game game;

    public abstract void Initialize();

    public abstract void LoadContent();

    public abstract void UnloadContent();

    public abstract void Update();

    public abstract void Draw();
  }
}