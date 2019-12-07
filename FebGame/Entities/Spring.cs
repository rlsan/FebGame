using FebEngine;

namespace FebGame.Entities
{
  public class Spring : Sprite
  {
    public float springForce = 100;

    public override void Init()
    {
      label = "obj_spring";
      tag = "Bounce";

      TexturePath = "Sprite/Ball";
    }

    public override void OnCollision(CollisionArgs collision)
    {
      base.OnCollision(collision);

      collision.Other.Body.velocity.Y = -springForce;
    }
  }
}