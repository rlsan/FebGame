using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FebEngine
{
  public abstract class Entity
  {
    public string Name { get; set; }
    public int Id { get; set; }

    public Vector2 Position { get; set; }
    public Vector2 Scale { get; set; }
    public float Rotation { get; set; }

    public virtual void Update(GameTime gameTime)
    {
      // Default update code.
    }

    public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
      // Default drawing code.
    }
  }
}