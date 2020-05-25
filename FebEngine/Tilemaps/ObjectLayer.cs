using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fubar
{
  public class ObjectLayer
  {
    public List<ObjectID> Objects = new List<ObjectID>();

    public void Add(Vector2 position, string name)
    {
      Objects.Add(new ObjectID(name, position));
    }
  }

  public struct ObjectID
  {
    public string name;
    public Vector2 position;

    public ObjectID(string name, Vector2 position)
    {
      this.name = name;
      this.position = position;
    }

    public override string ToString()
    {
      return name;
    }
  }
}