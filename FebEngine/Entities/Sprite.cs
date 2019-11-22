using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TexturePackerLoader;

namespace FebEngine
{
  public class Sprite : Entity
  {
    public Texture2D Texture { get; set; }
    public Color Tint { get; set; }
    public Vector2 Origin { get; set; }
    public Rectangle Bounds { get; set; }

    public Animator Animations { get; } = new Animator();
    public Body Body { get; set; }

    public bool isAlive = true;

    public SpriteSheet spriteSheet;

    public Sprite(Texture2D texture)
    {
      Tint = Color.White;

      Texture = texture;

      Bounds = texture.Bounds;

      Body = new Body(this);
    }

    public void SetTexture(SpriteSheet texture)
    {
      spriteSheet = texture;
      Animations.spriteSheet = texture;
    }

    public void Kill()
    {
      isAlive = false;
    }

    public void Revive()
    {
      isAlive = true;
    }

    public void CenterOrigin()
    {
      Origin = new Vector2(0.5f, 0.5f);
    }

    public override void Draw(RenderManager renderer, GameTime gameTime)
    {
      if (isAlive)
      {
        int time = (int)(Time.CurrentTime * 1 * Animations.current.frames.Count) % Animations.current.frames.Count;
        renderer.SpriteRender.Draw(Animations.current.frames[time], Position, Tint, 0, Scale.X, Scale.Y);
        //renderer.SpriteBatch.Draw(Texture, Position - (Origin * Bounds.Size.ToVector2()), Tint);
        //renderer.SpriteBatch.Draw(Texture, new Rectangle(Position.ToPoint(), Scale.ToPoint()), Tint);
      }
    }

    public override void Update(GameTime gameTime)
    {
    }
  }
}