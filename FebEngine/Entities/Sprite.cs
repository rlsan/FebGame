using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TexturePackerLoader;

namespace FebEngine
{
  public class Sprite : Entity
  {
    public enum SpriteFacing { Left, Right };

    public delegate void CollisionEventHandler(object sender, CollisionArgs e);

    public event CollisionEventHandler Collision;

    public event CollisionEventHandler TriggerEnter;

    public event CollisionEventHandler TriggerStay;

    public static Sprite Empty { get { return new Sprite(); } }

    public string SheetSuffix { get; set; }
    public string TexturePath { get; set; }

    public Texture2D Texture { get; set; }
    public Color Tint { get; set; }
    public Vector2 Origin { get; set; }
    public Rectangle Bounds { get; set; }

    public SpriteFacing Facing { get; set; } = SpriteFacing.Right;

    public Animator Animations { get; } = new Animator();
    public Body Body { get; set; }

    public bool isAlive = true;

    public SpriteSheet spriteSheet;

    public Sprite()
    {
      Tint = Color.White;

      Bounds = new Rectangle(0, 0, 32, 32);

      Body = new Body(this);
    }

    public void SetTexture(SpriteSheet texture)
    {
      spriteSheet = texture;
      Animations.spriteSheet = texture;
    }

    public virtual void Kill()
    {
      isAlive = false;
    }

    public virtual void Revive()
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
        if (Animations != null && Animations.current != null)
        {
          if (Facing == SpriteFacing.Left)
          {
            renderer.SpriteRender.Draw(Animations.CurrentFrame, Position, Tint, Rotation, Scale.X, Scale.Y, SpriteEffects.FlipHorizontally);
          }
          else if (Facing == SpriteFacing.Right)
          {
            renderer.SpriteRender.Draw(Animations.CurrentFrame, Position, Tint, Rotation, Scale.X, Scale.Y);
          }
        }
      }
    }

    public override void Update(GameTime gameTime)
    {
    }

    public virtual void OnCollision(CollisionArgs collision)
    {
      var handler = Collision;
      handler?.Invoke(this, collision);
    }

    public virtual void OnTriggerEnter(CollisionArgs collision)
    {
      var handler = TriggerEnter;
      handler?.Invoke(this, collision);
    }

    public virtual void OnTriggerStay(CollisionArgs collision)
    {
      var handler = TriggerStay;
      handler?.Invoke(this, collision);
    }
  }
}