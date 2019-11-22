using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebEngine
{
  public static class Extensions
  {
    public static Vector2 Limit(this Vector2 a, Vector2 b)
    {
      if (Math.Abs(a.X) > b.X) a.X = b.X * Math.Sign(a.X);
      if (Math.Abs(a.Y) > b.Y) a.Y = b.Y * Math.Sign(a.Y);

      return a;
    }
  }
}