using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Fubar
{
  public class Line
  {
    public Vector2 start;
    public Vector2 end;

    public static bool Intersect(Line a, Line b, out Vector2 intersectionPoint)
    {
      intersectionPoint = Vector2.Zero;

      float x1 = a.start.X;
      float y1 = a.start.Y;
      float x2 = a.end.X;
      float y2 = a.end.Y;

      float x3 = b.start.X;
      float y3 = b.start.Y;
      float x4 = b.end.X;
      float y4 = b.end.Y;

      float deltaACy = a.start.Y - b.start.Y;
      float deltaDCx = b.end.X - b.start.X;
      float deltaACx = a.start.X - b.start.X;
      float deltaDCy = b.end.Y - b.start.Y;
      float deltaBAx = a.end.X - a.start.X;
      float deltaBAy = a.end.Y - a.start.Y;

      float denominator = deltaBAx * deltaDCy - deltaBAy * deltaDCx;
      float numerator = deltaACy * deltaDCx - deltaACx * deltaDCy;

      if (denominator == 0)
      {
        if (numerator == 0)
        {
          // collinear. Potentially infinite intersection points.
          // Check and return one of them.
          if (a.start.X >= b.start.X && a.start.X <= b.end.X)
          {
            intersectionPoint = a.start;
            return true;
          }
          else if (b.start.X >= a.start.X && b.start.X <= a.end.X)
          {
            intersectionPoint = b.start;
            return true;
          }
          else
          {
            return false;
          }
        }
        else
        { // parallel
          return false;
        }
      }

      double r = numerator / denominator;
      if (r < 0 || r > 1)
      {
        return false;
      }

      double s = (deltaACy * deltaBAx - deltaACx * deltaBAy) / denominator;
      if (s < 0 || s > 1)
      {
        return false;
      }

      intersectionPoint = new Vector2((float)(a.start.X + r * deltaBAx), (float)(a.start.Y + r * deltaBAy));
      return true;
    }

    public static bool Intersect(Line line, Rectangle rect, out Vector2 intersectionPoint)
    {
      bool intersect = false;
      float shortestDistance = float.PositiveInfinity;
      Vector2 closestPoint = line.start;

      intersectionPoint = line.end;

      var topLeft = new Vector2(rect.X, rect.Y);
      var topRight = new Vector2(rect.X + rect.Width, rect.Y);

      var bottomLeft = new Vector2(rect.X, rect.Y + rect.Height);
      var bottomRight = new Vector2(rect.X + rect.Width, rect.Y + rect.Height);

      var top = new Line(topLeft, topRight);
      var left = new Line(topLeft, bottomLeft);
      var right = new Line(topRight, bottomRight);
      var bottom = new Line(bottomLeft, bottomRight);

      Vector2 point;

      var points = new List<Vector2>();

      if (Intersect(line, top, out point))
      {
        intersect = true;

        float d = Vector2.Distance(point, line.start);
        if (d < shortestDistance)
        {
          shortestDistance = d;
          closestPoint = point;
        }
      }
      if (Intersect(line, left, out point))
      {
        intersect = true;

        float d = Vector2.Distance(point, line.start);
        if (d < shortestDistance)
        {
          shortestDistance = d;
          closestPoint = point;
        }
      }
      if (Intersect(line, right, out point))
      {
        intersect = true;

        float d = Vector2.Distance(point, line.start);
        if (d < shortestDistance)
        {
          shortestDistance = d;
          closestPoint = point;
        }
      }
      if (Intersect(line, bottom, out point))
      {
        intersect = true;

        float d = Vector2.Distance(point, line.start);
        if (d < shortestDistance)
        {
          shortestDistance = d;
          closestPoint = point;
        }
      }

      if (intersect)
      {
        intersectionPoint = closestPoint;

        return true;
      }

      return false;
    }

    public float Distance
    {
      get
      {
        float p1 = end.X - start.X;
        float p2 = end.Y - start.Y;
        p1 *= p1;
        p2 *= p2;
        return (float)Math.Sqrt(p1 + p2);
      }
    }

    public float Slope
    {
      get
      {
        float p1 = end.X - start.X;
        float p2 = end.Y - start.Y;

        return p2 / p1;
      }
    }

    public float Angle
    {
      get
      {
        float dy = end.Y - start.Y;
        float dx = end.X - start.X;
        float theta = (float)Math.Atan2(dy, dx); // range (-PI, PI]
                                                 //theta *= 180 / (float)Math.PI; // rads to degs, range (-180, 180]

        //if (theta < 0) theta = 360 + theta; // range [0, 360)
        return theta;
      }
    }

    public Rectangle Bounds
    {
      get
      {
        var rect = new Rectangle();
        float width = end.X - start.X;
        float height = end.Y - start.Y;

        if (start.X < end.X)
        {
          rect.X = (int)start.X;
          rect.Width = (int)width;
        }
        else
        {
          rect.X = (int)(start.X + width);
          rect.Width = -(int)width;
        }
        if (start.Y < end.Y)
        {
          rect.Y = (int)start.Y;
          rect.Height = (int)height;
        }
        else
        {
          rect.Y = (int)(start.Y + height);
          rect.Height = -(int)height;
        }
        return rect;
      }
    }

    public Line(Vector2 start, Vector2 end)
    {
      this.start = start;
      this.end = end;
    }

    public Line(Point start, Point end)
    {
      this.start = start.ToVector2();
      this.end = end.ToVector2();
    }

    public Line(Vector2 start, Point end)
    {
      this.start = start;
      this.end = end.ToVector2();
    }

    public Line(Point start, Vector2 end)
    {
      this.start = start.ToVector2();
      this.end = end;
    }
  }
}