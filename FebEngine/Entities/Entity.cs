using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace FebEngine
{
  public class Entity
  {
    public StringBuilder Name;
    public int Id { get; set; }

    public Vector2 Position { get; set; }
    public Vector2 Scale { get; set; }
    public float Rotation { get; set; }

    public bool IsActive { get; set; } = true;

    public int DrawOrder { get; set; } = 0;
    public bool FollowCamera { get; set; } = true;

    public bool IsVisible { get; set; } = true;

    public WorldManager world;

    public void Destroy()
    {
      world.RemoveEntity(this);
    }

    public void Hide()
    {
      IsVisible = false;
    }

    public void Show()
    {
      IsVisible = true;
    }

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