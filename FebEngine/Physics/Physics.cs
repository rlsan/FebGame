using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace FebEngine
{
  public class Physics
  {
    /// <summary>
    /// The downward force of gravity.
    /// </summary>
    public float gravity = 20;

    /// <summary>
    /// How far from a body to check for collision.
    /// </summary>
    public int queryRange = 400;

    private static WorldManager World { get; set; }
    private static QuadTree QuadTree { get; set; }

    public Physics(WorldManager world, int quadTreeDepth = 4)
    {
      World = world;
      QuadTree = new QuadTree(new Rectangle(-4000, -4000, 8000, 8000), quadTreeDepth);
    }

    private void RebuildQuadTree()
    {
      QuadTree.Reset();

      foreach (var sprite in World.GetEntities<Sprite>())
        QuadTree.Insert(sprite.Body);
    }

    public static bool Raycast(Vector2 origin, Vector2 direction, out RaycastHit hit, float distance = 1000, params string[] ignoreTags)
    {
      hit = new RaycastHit();
      var ray = new Ray(origin, Vector2.Normalize(direction));

      Rectangle rangeRect = new Rectangle((int)origin.X - (int)distance, (int)origin.Y - (int)distance, (int)distance * 2, (int)distance * 2);
      List<Body> Fbodies = QuadTree.Query(rangeRect);

      Debug.DrawRect(rangeRect, new Color(Color.Red, 1f));

      float recordDistance = float.PositiveInfinity;
      Vector2 closestPoint = ray.Origin;

      bool intersect = false;
      foreach (var body in Fbodies)
      {
        var bb = body.Bounds;

        if (bb.Contains(origin)) continue;
        if (ignoreTags.Contains(body.Parent.tag)) continue;

        // Top edge start
        float x1 = bb.X;
        float y1 = bb.Y;
        // Top edge end
        float x2 = bb.X + bb.Width;
        float y2 = bb.Y;

        // left edge start
        float x3 = bb.X;
        float y3 = bb.Y;
        // left edge end
        float x4 = bb.X;
        float y4 = bb.Y + bb.Height;

        // right edge start
        float x5 = bb.X + bb.Width;
        float y5 = bb.Y;
        // right edge end
        float x6 = bb.X + bb.Width;
        float y6 = bb.Y + bb.Height;

        // bottom edge start
        float x7 = bb.X;
        float y7 = bb.Y + bb.Height;
        // bottom edge end
        float x8 = bb.X + bb.Width;
        float y8 = bb.Y + bb.Height;

        var lineA = new Line(new Vector2(x1, y1), new Vector2(x2, y2));
        var lineB = new Line(new Vector2(x3, y3), new Vector2(x4, y4));
        var lineC = new Line(new Vector2(x5, y5), new Vector2(x6, y6));
        var lineD = new Line(new Vector2(x7, y7), new Vector2(x8, y8));
        var rayLine = new Line(ray.Origin, ray.Origin + ray.Direction * 2);

        //body.Parent.Tint = Color.Red;

        //if (Line.Intersect(rayLine, bb, out Line line))
        //{
        //return true;
        //Debug.DrawLine(line.start, line.end);
        //}

        if (Line.Intersect(lineA, rayLine, out Vector2 outPointA))
        {
          intersect = true;

          float d = Vector2.Distance(outPointA, ray.Origin);
          if (d < recordDistance)
          {
            recordDistance = d;
            closestPoint = outPointA;

            hit = new RaycastHit(body, outPointA, d);
          }
        }
        if (Line.Intersect(lineB, rayLine, out Vector2 outPointB))
        {
          intersect = true;

          float d = Vector2.Distance(outPointB, ray.Origin);
          if (d < recordDistance)
          {
            recordDistance = d;
            closestPoint = outPointB;

            hit = new RaycastHit(body, outPointB, d);
          }
        }

        if (Line.Intersect(lineC, rayLine, out Vector2 outPointC))
        {
          intersect = true;

          float d = Vector2.Distance(outPointC, ray.Origin);
          if (d < recordDistance)
          {
            recordDistance = d;
            closestPoint = outPointC;

            hit = new RaycastHit(body, outPointC, d);
          }
        }
        if (Line.Intersect(lineD, rayLine, out Vector2 outPointD))
        {
          intersect = true;

          float d = Vector2.Distance(outPointD, ray.Origin);
          if (d < recordDistance)
          {
            recordDistance = d;
            closestPoint = outPointD;

            hit = new RaycastHit(body, outPointD, d);
          }
        }
      }

      if (intersect)
      {
        Debug.DrawPoint(closestPoint);
        Debug.DrawLine(ray.Origin, closestPoint);
        Debug.Text(hit.Collider.Parent.name, hit.Point);
      }
      else
      {
        Debug.DrawRay(ray.Origin, direction * distance);
      }

      return intersect;
    }

    private void Collide(Body bodyA, Body bodyB)
    {
      var e = new CollisionArgs();
      e.Primary = bodyA.Parent;
      e.Other = bodyB.Parent;

      var bb1 = bodyA.Bounds;
      var bb2 = bodyB.Bounds;

      // Trigger collision
      if (bodyB.isTrigger)
      {
        if (bb1.Intersects(bb2))
        {
          bodyA.Parent.OnTriggerStay(e);
        }
      }
      else
      {
        // X collision
        if (bb1.Top < bb2.Bottom && bb1.Bottom > bb2.Top)
        {
          float moveX = bodyA.velocity.X;

          // Moving right
          if (bodyA.velocity.X > 0)
          {
            // Does the object have collision on the left side?
            if (bodyB.collidesLeft)
            {
              moveX = Math.Min(Math.Abs(bodyA.velocity.X), Math.Abs(bb2.Left - bb1.Right) - 1);

              if (moveX == 0)
              {
                bodyA.Parent.OnCollision(e);
                bodyA.blocked.Right = true;
              }
            }
          }
          //moving left
          else if (bodyA.velocity.X < 0)
          {
            //does the object have collision on the right side?
            if (bodyB.collidesRight)
            {
              moveX = -Math.Min(Math.Abs(bodyA.velocity.X), Math.Abs(bb2.Right - bb1.Left) - 1);

              if (moveX == 0)
              {
                bodyA.Parent.OnCollision(e);
                bodyA.blocked.Left = true;
              }
            }
          }

          bodyA.velocity.X = moveX;
        }

        //Y collision
        if (bb1.Left < bb2.Right && bb1.Right > bb2.Left)
        {
          float moveY = bodyA.velocity.Y;

          //moving down
          if (bodyA.velocity.Y > 0)
          {
            //does the object have collision on the top?
            if (bodyB.collidesUp)
            {
              moveY = Math.Min(Math.Abs(bodyA.velocity.Y), Math.Abs(bb2.Top - bb1.Bottom) - 1);

              if (moveY == 0)
              {
                bodyA.Parent.OnCollision(e);
                bodyA.blocked.Down = true;
              }
            }
          }
          //moving up
          else if (bodyA.velocity.Y < 0)
          {
            //does the object have collision on the bottom?
            if (bodyB.collidesDown)
            {
              moveY = -Math.Min(Math.Abs(bodyA.velocity.Y), Math.Abs(bb2.Bottom - bb1.Top) - 1);

              if (moveY == 0)
              {
                bodyA.Parent.OnCollision(e);
                bodyA.blocked.Up = true;
              }
            }
          }

          bodyA.velocity.Y = moveY;
        }
      }
    }

    public void Update()
    {
      // Rebuild the quadtree with updated values.
      RebuildQuadTree();

      // Iterate through each sprite in the world.
      foreach (var sprite in World.GetEntities<Sprite>())
      {
        var body = sprite.Body;

        // Reset the body's collision values.
        body.blocked.Reset();

        // Skip if the sprite is dead or if its body is disabled.
        if (!sprite.isAlive) continue;
        if (!body.enabled) continue;

        // Apply gravity.
        if (body.hasGravity) body.velocity += Vector2.UnitY * gravity * Time.DeltaTime;

        // Limit velocity for each axis.
        body.velocity = body.velocity.Limit(body.maxVelocity);

        // A non-dynamic body does not personally check for collision.
        if (!body.isDynamic) continue;

        // Set the area that the quadtree should query.
        Rectangle queryArea = new Rectangle(
          (int)sprite.Position.X - queryRange / 2,
          (int)sprite.Position.Y - queryRange / 2,
          queryRange, queryRange);

        // Check collision against each body detected by the quadtree.
        foreach (var queriedBody in QuadTree.Query(queryArea))
        {
          // Skip if it's checking collision on itself.
          if (queriedBody == body) continue;

          // Skip if there are no shared collision layers.
          bool hasMatch =
            body.collisionLayers.Any
            (x => queriedBody.collisionLayers.Any(y => y == x));
          if (!hasMatch) continue;

          // Adjust the body's velocity.
          Collide(body, queriedBody);
        }

        // Check collision against all tilemaps.
        foreach (var mapGroup in World.GetEntities<MapGroup>())
        {
          CollideTilemap(body, mapGroup.CurrentMap);
        }

        // Apply the velocity to the sprite.
        sprite.Position += body.velocity;
      }
    }

    private void CollideTilemap(Body body, Tilemap map)
    {
      if (map == null) return;

      var layer = map.GetLayer(1);
      int size = 64;
      int dist = 4;

      Rectangle bb = body.Bounds;

      int leftSide = Mathf.FloorToGrid(bb.Left, size) / size;
      int rightSide = Mathf.FloorToGrid(bb.Right, size) / size;

      int topSide = Mathf.FloorToGrid(bb.Top, size) / size;
      int bottomSide = Mathf.FloorToGrid(bb.Bottom, size) / size;

      float moveY = body.velocity.Y;

      if (body.velocity.Y > 0)
      {
        int hitPoint = 10000;
        for (int x = leftSide; x <= rightSide; x++)
        {
          for (int y = bottomSide; y <= bottomSide + dist; y++)
          {
            //Debug.DrawLine(new Vector2(x * size, y * size), new Vector2(x * size, bottomSide * size));
            var tile = layer.GetTile(x, y);

            if (tile > -1)
            {
              if (y < hitPoint) hitPoint = y;
              break;
            }
          }
        }

        //Debug.DrawPoint(leftSide * size, hitPoint * size);

        moveY = Math.Min(Math.Abs(body.velocity.Y), Math.Abs(hitPoint * size - body.Bounds.Bottom) - 1);

        if (moveY == 0)
        {
          body.blocked.Down = true;
        }

        //body.velocity.X = moveX;
      }
      else if (body.velocity.Y < 0)
      {
        int hitPoint = -10000;
        for (int x = leftSide; x <= rightSide; x++)
        {
          for (int y = topSide + 1; y > topSide - dist; y--)
          {
            //Debug.DrawLine(new Vector2(x * size, y * size), new Vector2(x * size, topSide * size));
            var tile = layer.GetTile(x, y);

            if (tile > -1)
            {
              if (y + 1 > hitPoint) hitPoint = y + 1;
              break;
            }
          }
        }

        //Debug.DrawPoint(leftSide * size, hitPoint * size);

        moveY = -Math.Min(Math.Abs(body.velocity.Y), Math.Abs(hitPoint * size - body.Bounds.Top) - 1);

        if (moveY == 0)
        {
          body.blocked.Up = true;
        }

        //body.velocity.X = moveX;
      }

      body.velocity.Y = moveY;

      float moveX = body.velocity.X;

      if (body.velocity.X > 0)
      {
        int hitPoint = 10000;
        for (int y = topSide; y <= bottomSide; y++)
        {
          for (int x = rightSide - 1; x <= rightSide + dist; x++)
          {
            //Debug.DrawLine(new Vector2((rightSide - 1) * size, y * size), new Vector2((rightSide + dist) * size, y * size));
            var tile = layer.GetTile(x, y);

            if (tile > -1)
            {
              if (x < hitPoint) hitPoint = x;
              break;
            }
          }
        }

        moveX = Math.Min(Math.Abs(body.velocity.X), Math.Abs(hitPoint * size - body.Bounds.Right) - 1);

        if (moveX == 0)
        {
          body.blocked.Right = true;
        }

        //body.velocity.X = moveX;
      }
      else if (body.velocity.X < 0)
      {
        int hitPoint = -10000;
        for (int y = topSide; y <= bottomSide; y++)
        {
          for (int x = leftSide + 1; x > leftSide - dist; x--)
          {
            //Debug.DrawLine(new Vector2((leftSide + 1) * size, y * size), new Vector2((leftSide - dist) * size, y * size));
            var tile = layer.GetTile(x, y);

            if (tile > -1)
            {
              if (x + 1 > hitPoint) hitPoint = x + 1;
              break;
            }
          }
        }

        moveX = -Math.Min(Math.Abs(body.velocity.X), Math.Abs(hitPoint * size - body.Bounds.Left) - 1);

        if (moveX == 0)
        {
          body.blocked.Left = true;
        }
      }

      body.velocity.X = moveX;
      return;

      if (body.velocity.X > 0)
      {
        int min = Mathf.FloorToGrid(body.Parent.Position.X, size) / size;
        int max = Mathf.FloorToGrid(body.Parent.Position.X + dist, size) / size;

        int y = Mathf.FloorToGrid(body.Parent.Position.Y, size) / size;

        int hit = 10000;
        for (int i = min; i <= max; i++)
        {
          var tile = layer.GetTile(i, y);

          if (tile > -1)
          {
            hit = i * size;
            break;
          }
        }

        //var moveX = Math.Min(Math.Abs(body.velocity.X), Math.Abs(hit - body.Bounds.Right) - 1);

        body.velocity.X = moveX;
      }
      else if (body.velocity.X < 0)
      {
        int min = Mathf.FloorToGrid(body.Parent.Position.X, size) / size;
        int max = Mathf.FloorToGrid(body.Parent.Position.X - dist, size) / size;

        int y = Mathf.FloorToGrid(body.Parent.Position.Y, size) / size;

        int hit = 10000;
        for (int i = min; i > max; i--)
        {
          var tile = layer.GetTile(i, y);

          if (tile > -1)
          {
            hit = i * size + size;
            break;
          }
        }

        //var moveX = Math.Min(Math.Abs(body.velocity.X), Math.Abs(hit - body.Bounds.Left) - 1);

        body.velocity.X = -moveX;
      }
      if (body.velocity.Y > 0)
      {
        int min = Mathf.FloorToGrid(body.Parent.Position.Y, size) / size;
        int max = Mathf.FloorToGrid(body.Parent.Position.Y + dist, size) / size;

        int x = Mathf.FloorToGrid(body.Parent.Position.X, size) / size;

        int hit = 10000;
        for (int i = min; i <= max; i++)
        {
          var tile = layer.GetTile(x, i);

          if (tile > -1)
          {
            hit = i * size;
            break;
          }
        }

        //var moveY = Math.Min(Math.Abs(body.velocity.Y), Math.Abs(hit - body.Bounds.Bottom) - 1);

        body.velocity.Y = moveY;
      }
      else if (body.velocity.Y < 0)
      {
        int min = Mathf.FloorToGrid(body.Parent.Position.Y, size) / size;
        int max = Mathf.FloorToGrid(body.Parent.Position.Y - dist, size) / size;

        int x = Mathf.FloorToGrid(body.Parent.Position.X, size) / size;

        int hit = 10000;
        for (int i = min; i > max; i--)
        {
          var tile = layer.GetTile(x, i);

          if (tile > -1)
          {
            hit = i * size + size;
            break;
          }
        }

        //var moveY = Math.Min(Math.Abs(body.velocity.Y), Math.Abs(hit - body.Bounds.Top) - 1);

        body.velocity.Y = -moveY;
      }
    }
  }
}