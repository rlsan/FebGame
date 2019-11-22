using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace FebEngine
{
  public class Line
  {
    public Vector2 start;
    public Vector2 end;

    public static bool Intersect(Line a, Line b, out Vector2 point)
    {
      point = Vector2.Zero;

      float x1 = a.start.X;
      float y1 = a.start.Y;
      float x2 = a.end.X;
      float y2 = a.end.Y;

      float x3 = b.start.X;
      float y3 = b.start.Y;
      float x4 = b.end.X;
      float y4 = b.end.Y;

      float denominator = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
      if (denominator == 0) return false;

      float t = ((x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4)) / denominator;
      float u = -((x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3)) / denominator;

      // If the lines intersect...
      if (t > 0 && t < 1 && u > 0)
      {
        float hitPointX = x1 + t * (x2 - x1);
        float hitPointY = y1 + t * (y2 - y1);
        point = new Vector2(hitPointX, hitPointY);

        return true;
      }

      return false;
    }

    public static bool Intersect(Line line, Rectangle rect, out Line intersectionLine)
    {
      intersectionLine = new Line(Vector2.Zero, Vector2.Zero);

      var topLeft = new Vector2(rect.X, rect.Y);
      var topRight = new Vector2(rect.X + rect.Width, rect.Y);

      var bottomLeft = new Vector2(rect.X, rect.Y + rect.Height);
      var bottomRight = new Vector2(rect.X + rect.Width, rect.Y + rect.Height);

      var top = new Line(topLeft, topRight);
      var left = new Line(topLeft, bottomLeft);
      var right = new Line(topRight, bottomRight);
      var bottom = new Line(bottomLeft, bottomRight);

      Debug.DrawLine(top);
      Debug.DrawLine(left);
      Debug.DrawLine(right);
      Debug.DrawLine(bottom);

      Debug.DrawLine(line);

      if (Intersect(line, top, out Vector2 point))
      {
        //Debug.DrawPoint(point);
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