using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebEngine
{
  public class Transform
  {
    Vector2 Position { get; set; }
    float Rotation { get; set; }
    Vector2 Scale { get; set; }

    public Transform()
    {
      Scale = new Vector2(1, 1);
    }
  }
}