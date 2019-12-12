using FebEngine;
using Microsoft.Xna.Framework;

namespace FebGame.Objects
{
  public class Pepper : Sprite
  {
    public float respawnTimer;
    public float respawnTime = 3;

    public override void Init()
    {
      Animations.Add("Idle", "Sprite/Zombie");

      Body.isTrigger = true;
      Body.isDynamic = false;
    }

    public override void Update(GameTime gameTime)
    {
      respawnTimer -= Time.DeltaTime;

      if (respawnTimer <= 0) Revive();
    }

    public override void Kill()
    {
      respawnTimer = respawnTime;
      base.Kill();
    }
  }
}