using Microsoft.Xna.Framework;
using FebEngine;

namespace FebGame
{
  public class Player : Actor
  {
    private PlayerState state = PlayerState.Grounded;
    private float speed = 500;
    private float jumpForce = 10;

    private bool isJumping = false;
    private bool hasJumped;

    private float jumpTime = 0.3f;
    private float jumpTimer;

    private float coyoteTime = 0.17f;
    private float coyoteTimer;

    private bool wasGrounded = false;

    private PlayerState DetermineState()
    {
      if (Body.OnFloor) return PlayerState.Grounded;
      else return PlayerState.Airborne;
    }

    public override void Update(GameTime gameTime)
    {
      tag = "Player";
      InputManager.actor = this;

      Body.hasGravity = true;
      Body.maxVelocity.X = 10;
      Body.maxVelocity.Y = 20;

      state = DetermineState();
      Body.velocity.X *= 0.5f;

      if (state == PlayerState.Grounded)
      {
        if (!wasGrounded)
        {
          wasGrounded = true;
        }

        coyoteTimer = coyoteTime;
      }
      else if (state == PlayerState.Airborne)
      {
        coyoteTimer -= Time.DeltaTime;

        wasGrounded = false;
      }
      jumpTimer -= Time.DeltaTime;

      hasJumped = isJumping;
      isJumping = false;
    }

    public override void OnCollision(CollisionArgs e)
    {
      base.OnCollision(e);
    }

    public override void Move(float x, float y)
    {
      Body.velocity.X += x * speed * Time.DeltaTime;
    }

    public override void Jump()
    {
      if (jumpTimer > 0)
      {
        if (coyoteTimer > 0)
        {
          Body.velocity.Y = -jumpForce;

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

    private enum PlayerState { Grounded, Airborne }
  }
}