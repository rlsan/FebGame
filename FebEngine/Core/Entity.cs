using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using TexturePackerLoader;

namespace FebEngine
{
  public class Entity
  {
    public StringBuilder name;
    public string label;
    public string tag;
    public int id;

    public Vector2 Position { get; set; }
    public Vector2 Scale { get; set; } = Vector2.One;
    public float Rotation { get; set; }

    public bool IsActive { get; set; } = true;

    public int DrawOrder { get; set; } = 0;
    public bool FollowCamera { get; set; } = true;

    public bool IsVisible { get; set; } = true;
    public bool IsFrozen { get; set; } = false;

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

    public void Freeze()
    {
      IsFrozen = true;
    }

    public void Thaw()
    {
      IsFrozen = false;
    }

    public virtual void Init()
    {
      // Default init code.
    }

    public virtual void Update(GameTime gameTime)
    {
      // Default update code.
    }

    public virtual void Draw(RenderManager renderer, GameTime gameTime)
    {
      // Default drawing code.
    }
  }
}