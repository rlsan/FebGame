using FebEngine;
using Microsoft.Xna.Framework;

namespace FebGame.Objects
{
  public class Butterfly : Actor
  {
    public bool isExcited = false;
    public float excitedTime = 3f;
    public float excitedTimer;
    public Vector2 homePoint;
    public Vector2 temporaryHomePoint;

    public Sprite scarer;

    public void SetHome()
    {
      homePoint = Position;
    }

    public override void Init()
    {
      Animations.Add("Idle", "Sprite/Butterfly/Idle", 30);
      Animations.Add("Flying", "Sprite/Butterfly/Flying", 100);

      Tint = RNG.RandomColor();

      Body.SetBounds(0, 0, 20, 20);
      Body.isTrigger = true;
      Body.hasGravity = false;

      Body.maxVelocity = new Vector2(2, 1);
    }

    public override void Update(GameTime gameTime)
    {
      if (Body.velocity.X > 0)
      {
        Facing = SpriteFacing.Right;
      }
      else
      {
        Facing = SpriteFacing.Left;
      }

      if (isExcited)
      {
        Animations.Play("Flying");

        excitedTimer -= Time.DeltaTime;

        if (excitedTimer > 0)
        {
          Body.velocity += RNG.PointOnUnitCircle() * 1f;
        }
        else
        {
          Body.velocity += Vector2.Normalize(homePoint - Position) * 0.1f;
          Body.velocity += RNG.PointOnUnitCircle() * 1f;

          if (Position.Approx(homePoint, 10))
          {
            isExcited = false;
            //Position = homePoint;
            Body.velocity = Vector2.Zero;
          }
        }
      }
      else
      {
        Animations.Play("Idle");
      }
    }

    public override void OnCollision(CollisionArgs collision)
    {
      if (collision.Other.tag == "Player")
      {
        if (!isExcited)
        {
          Audio.PlaySound("flee", 0.5f);
          excitedTimer = excitedTime;
          isExcited = true;

          scarer = collision.Other;
        }
      }

      base.OnCollision(collision);
    }

    public override void Jump()
    {
    }

    public override void Move(float x, float y)
    {
    }
  }
}