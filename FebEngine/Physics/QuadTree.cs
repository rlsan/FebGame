using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FebEngine.Utility;

namespace FebEngine.Physics
{
  public class QuadTree
  {
    private Rectangle bounds;
    private int capacity;
    public List<Body> bodies;

    private Rectangle range;

    private QuadTree nw;
    private QuadTree ne;
    private QuadTree sw;
    private QuadTree se;

    private bool isDivided = false;

    public QuadTree(Rectangle bounds, int capacity)
    {
      this.bounds = bounds;
      this.capacity = capacity;

      bodies = new List<Body>();
    }

    public bool Insert(Body body)
    {
      if (!bounds.Contains(body.Parent.Position))
      {
        return false;
      }

      if (bodies.Count < capacity)
      {
        bodies.Add(body);
        return true;
      }
      if (!isDivided)
      {
        Subdivide();
      }

      if (nw.Insert(body) || ne.Insert(body) || sw.Insert(body) || se.Insert(body)) return true;

      return false;
    }

    private void Subdivide()
    {
      nw = new QuadTree(new Rectangle(bounds.Left, bounds.Top, bounds.Width / 2, bounds.Height / 2), capacity);
      ne = new QuadTree(new Rectangle(bounds.Left + bounds.Width / 2, bounds.Top, bounds.Width / 2, bounds.Height / 2), capacity);
      sw = new QuadTree(new Rectangle(bounds.Left, bounds.Top + bounds.Height / 2, bounds.Width / 2, bounds.Height / 2), capacity);
      se = new QuadTree(new Rectangle(bounds.Left + bounds.Width / 2, bounds.Top + bounds.Height / 2, bounds.Width / 2, bounds.Height / 2), capacity);

      isDivided = true;

      foreach (var body in bodies)
      {
        nw.Insert(body);
        ne.Insert(body);
        sw.Insert(body);
        se.Insert(body);
      }

      //bodies.Clear();
    }

    public List<Body> Query(Rectangle range, List<Body> foundBodies = null)
    {
      this.range = range;

      if (foundBodies == null)
      {
        foundBodies = new List<Body>();
      }

      if (!bounds.Intersects(range))
      {
        return foundBodies;
      }
      else
      {
        foreach (var body in bodies)
        {
          if (range.Contains(body.Parent.Position))
          {
            foundBodies.Add(body);
          }
        }

        if (isDivided)
        {
          nw.Query(range, foundBodies);
          ne.Query(range, foundBodies);
          sw.Query(range, foundBodies);
          se.Query(range, foundBodies);
        }

        return foundBodies;
      }
    }

    public void Draw()
    {
      Debug.DrawRect(bounds);

      if (isDivided)
      {
        nw.Draw();
        ne.Draw();
        sw.Draw();
        se.Draw();
      }
    }
  }
}