using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebEngine
{
  public class ObjectLayer
  {
    public List<ObjectID> Objects = new List<ObjectID>();

    public void Add(Vector2 position, int id)
    {
      Objects.Add(new ObjectID(id, position));
    }
  }

  public struct ObjectID
  {
    public int id;
    public Vector2 position;

    public ObjectID(int id, Vector2 position)
    {
      this.id = id;
      this.position = position;
    }

    public override string ToString()
    {
      return id.ToString();
    }
  }
}