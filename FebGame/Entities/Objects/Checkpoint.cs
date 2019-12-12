using FebEngine;
using Microsoft.Xna.Framework;

namespace FebGame.Objects
{
  public class Checkpoint : Sprite
  {
    public override void Init()
    {
      label = "obj_checkpoint";
      tag = "Checkpoint";

      Body.isTrigger = true;
      Body.isDynamic = false;
      Body.SetBounds(0, 0, 100, 100);
    }
  }
}