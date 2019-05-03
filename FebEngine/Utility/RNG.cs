using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebEngine.Utility
{
  public static class RNG
  {
    private static Random r = new Random();

    public static float Random()
    {
      return (float)r.NextDouble();
    }

    public static float Random(int seed)
    {
      var r = new Random(seed);

      return (float)r.NextDouble();
    }

    public static int RandInt()
    {
      return r.Next();
    }

    public static int RandInt(int seed)
    {
      var r = new Random(seed);

      return r.Next();
    }

    public static int RandIntRange(int min, int max)
    {
      return r.Next(min, max);
    }

    public static int RandIntRange(int min, int max, int seed)
    {
      var r = new Random(seed);

      return r.Next(min, max);
    }

    public static object Pick(object[] array)
    {
      return array[r.Next(array.Length)];
    }

    public static object Pick(object[] array, int seed)
    {
      return array[r.Next(array.Length)];
    }
  }
}