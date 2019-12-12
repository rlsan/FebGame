using FebEngine;

namespace FebGame.Objects
{
  public class Spring : Sprite
  {
    public float springForce = 5.5f;

    public override void Init()
    {
      label = "obj_spring";
      Animations.Add("Idle", "Sprite/AzureBall");

      Body.SetBounds(0, 0, 50, 50);

      Body.collidesLeft = false;
      Body.collidesRight = false;
      Body.collidesDown = false;
    }

    public override void OnCollision(CollisionArgs collision)
    {
      base.OnCollision(collision);
      /*
      if (collision.Other.Body.blocked.Down)
      {
        Audio.PlaySound("bounce");
      }
      */
    }
  }
}