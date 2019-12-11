using Microsoft.Xna.Framework;
using FebEngine;
using System;

namespace FebGame.Objects
{
  public class Player : Actor
  {
    private PlayerState state = PlayerState.Grounded;
    private float speed = 1.8f;
    private float jumpForce = 3.4f;

    private bool isJumping = false;
    private bool hasJumped;

    private float jumpTime = 0.3f;
    private float jumpTimer;

    private Vector2 moveDirection;

    private float coyoteTime = 0.16f;
    private float coyoteTimer;

    private bool wasGrounded = false;
    private bool hitCeiling = false;

    private bool hasHoisted = false;

    private bool hasPepper = false;
    private float pepperTime = 0.6f;
    private float pepperTimer;
    private bool pepperEffectActive = false;
    private float pepperEffectTime = 0.5f;

    private Emitters.Stars p;

    private PlayerState DetermineState()
    {
      if (Body.OnFloor) return PlayerState.Grounded;
      else return PlayerState.Airborne;
    }

    public override void Init()
    {
      Animations.Add("Idle", "Sprite/RedBall");

      Body.SetBounds(0, 0, 40, 70);
      tag = "Player";
      InputManager.player1 = this;

      Body.hasGravity = true;
      Body.maxVelocity.X = 8;
      Body.maxVelocity.Y = 8;

      p = State.Create.Entity<Emitters.Stars>();
    }

    public override void Update(GameTime gameTime)
    {
      state = DetermineState();
      Body.velocity.X *= 0.5f;

      if (state == PlayerState.Grounded)
      {
        if (!wasGrounded)
        {
          p.Emit(10, new Vector2(Body.Bounds.Center.X, Body.Bounds.Bottom));
          Audio.PlaySound("land", 0.5f);
          wasGrounded = true;
        }

        hasHoisted = false;

        coyoteTimer = coyoteTime;
      }
      else if (state == PlayerState.Airborne)
      {
        if (Body.blocked.Up)
        {
          if (!hitCeiling)
          {
            p.Emit(10, new Vector2(Body.Bounds.Center.X, Body.Bounds.Top));
            Audio.PlaySound("hitCeiling", 0.5f);
            hitCeiling = true;
          }
        }

        coyoteTimer -= Time.DeltaTime;

        hitCeiling = false;
        wasGrounded = false;
      }
      jumpTimer -= Time.DeltaTime;

      if (hasPepper)
      {
        pepperTimer -= Time.DeltaTime;

        if (pepperTimer <= 0)
        {
          if (!pepperEffectActive)
          {
            pepperEffectActive = true;

            Audio.PlaySound("superjump");
          }
          world.camera.Shake(15);
          Body.velocity.Y = -5f;
        }
        if (pepperTimer <= -pepperEffectTime)
        {
          pepperEffectActive = false;
          hasPepper = false;
        }
      }

      hasJumped = isJumping;
      isJumping = false;
    }

    public override void OnTriggerStay(CollisionArgs collision)
    {
      if (collision.Other.label == "obj_pepper")
      {
        if (collision.Other.isAlive)
        {
          collision.Other.Kill();

          hasPepper = true;
          pepperTimer = pepperTime;

          Audio.PlaySound("collect");
        }
      }

      base.OnTriggerStay(collision);
    }

    public override void OnCollision(CollisionArgs collision)
    {
      if (state == PlayerState.Airborne && !collision.Other.Body.isTrigger)
      {
        if (Body.OnWall && !hasHoisted)
        {
          if (Body.velocity.Y < 1)
          {
            if (!Physics.Raycast(Position - Vector2.UnitY * 30, Vector2.UnitX * Body.velocity.X, out var hit, Body.Bounds.Width))
            {
              Body.velocity.Y = Math.Min(-jumpForce, Body.velocity.Y - 1);

              if (Body.blocked.Right)
              {
                p.Emit(10, new Vector2(Body.Bounds.Right, Body.Bounds.Center.Y));
              }
              else if (Body.blocked.Left)
              {
                p.Emit(10, new Vector2(Body.Bounds.Left, Body.Bounds.Center.Y));
              }

              Audio.PlaySound("giddup");

              hasHoisted = true;
            }
          }
        }
      }

      base.OnCollision(collision);
    }

    public override void Move(float x, float y)
    {
      moveDirection = new Vector2(x, y);

      Body.velocity.X += x * speed;
    }

    public override void Jump()
    {
      if (jumpTimer > 0)
      {
        if (coyoteTimer > 0)
        {
          if (state == PlayerState.Grounded)
          {
            Body.velocity.Y += -jumpForce;
            Audio.PlaySound("jump");
          }
          if (state == PlayerState.Airborne)
          {
            if (Body.velocity.Y >= 0)
            {
              Body.velocity.Y = -jumpForce;
              Audio.PlaySound("jump");
            }
          }

          coyoteTimer = 0;
          jumpTimer = 0;
        }
      }

      if (hasJumped == false)
      {
        jumpTimer = jumpTime;

        hasJumped = true;
      }

      isJumping = true;
    }

    private enum PlayerState { Grounded, Airborne, Climbing }
  }
}