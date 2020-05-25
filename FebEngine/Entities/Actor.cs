using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fubar
{
  public abstract class Actor : Sprite
  {
    public abstract void Move(float x, float y);

    public abstract void Jump();
  }
}