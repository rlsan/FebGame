using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FebEngine.Entities
{
  public abstract class Entity
  {
    public string name;
    public Transform Transform { get; } = new Transform();

    public abstract void Update(GameTime gameTime);

    public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
  }
}