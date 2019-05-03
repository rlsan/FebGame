using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FebEngine.Entities;

namespace FebEngine.Physics
{
  public class PhysicsHandler
  {
    public float gravity = 10;
    public List<Body> bodies = new List<Body>();

    public QuadTree quadTree;

    public World world;

    public PhysicsHandler(World world)
    {
      this.world = world;
    }

    public void Enable(Sprite s)
    {
      s.Body = new Body(s);

      bodies.Add(s.Body);
    }

    private void UpdateQuadTree()
    {
      quadTree = new QuadTree(new Rectangle(0, 0, world.bounds.Width, world.bounds.Height), 4);

      foreach (var body in bodies)
      {
        quadTree.Insert(body);
      }
    }

    public void Update(GameTime gameTime)
    {
      var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

      UpdateQuadTree();

      foreach (var sprite in world.sprites)
      {
        if (sprite.Body != null && sprite.Body.enabled && sprite.Body.isDynamic)
        {
          sprite.Body.blocked.Reset();

          if (Math.Abs(sprite.Body.velocity.X) > sprite.Body.maxVelocity.X)
          {
            sprite.Body.velocity.X = sprite.Body.maxVelocity.X * Math.Sign(sprite.Body.velocity.X);
          }
          if (Math.Abs(sprite.Body.velocity.Y) > sprite.Body.maxVelocity.Y)
          {
            sprite.Body.velocity.Y = sprite.Body.maxVelocity.Y * Math.Sign(sprite.Body.velocity.Y);
          }

          Rectangle rangeRect = new Rectangle((int)sprite.transform.Position.X - 200, (int)sprite.transform.Position.Y - 200, 400, 400);
          List<Body> Fbodies = quadTree.Query(rangeRect);

          //Debug.DrawRect(rangeRect, new Color(Color.Red, 0.05f));

          if (sprite.Body.hasGravity) sprite.Body.velocity += Vector2.UnitY * gravity * delta;

          foreach (var body in Fbodies)
          {
            if (body != sprite.Body)
            {
              var bb1 = sprite.Body.bounds;
              var bb2 = body.bounds;

              bb1.Location = sprite.transform.Position.ToPoint();
              bb2.Location = body.Parent.transform.Position.ToPoint();

              //Debug.DrawLine(Vector2.Zero, bb2.Location.ToVector2());

              //Debug.DrawRect(bb1);

              //X collision
              if (bb1.Top < bb2.Bottom && bb1.Bottom > bb2.Top)
              {
                float moveX = 0;

                //moving right
                if (sprite.Body.velocity.X > 0)
                {
                  //does the object have collision on the left side?
                  if (body.collidesLeft)
                  {
                    moveX = Math.Min(Math.Abs(sprite.Body.velocity.X), Math.Abs(bb2.Left - bb1.Right) - 1);

                    if (moveX == 0)
                    {
                      sprite.Body.blocked.Right = true;
                    }
                  }
                  else
                  {
                    moveX = sprite.Body.velocity.X;
                  }
                }
                //moving left
                else if (sprite.Body.velocity.X < 0)
                {
                  //does the object have collision on the right side?
                  if (body.collidesRight)
                  {
                    moveX = -Math.Min(Math.Abs(sprite.Body.velocity.X), Math.Abs(bb2.Right - bb1.Left) - 1);

                    if (moveX == 0)
                    {
                      sprite.Body.blocked.Left = true;
                    }
                  }
                  else
                  {
                    moveX = sprite.Body.velocity.X;
                  }
                }

                sprite.Body.velocity.X = moveX;
              }

              //Y collision
              if (bb1.Left < bb2.Right && bb1.Right > bb2.Left)
              {
                float moveY = 0;

                //moving down
                if (sprite.Body.velocity.Y > 0)
                {
                  //does the object have collision on the top?
                  if (body.collidesUp)
                  {
                    moveY = Math.Min(Math.Abs(sprite.Body.velocity.Y), Math.Abs(bb2.Top - bb1.Bottom) - 1);

                    if (moveY == 0)
                    {
                      sprite.Body.blocked.Down = true;
                    }
                  }
                  else
                  {
                    moveY = sprite.Body.velocity.Y;
                  }
                }
                //moving up
                else if (sprite.Body.velocity.Y < 0)
                {
                  //does the object have collision on the bottom?
                  if (body.collidesDown)
                  {
                    moveY = -Math.Min(Math.Abs(sprite.Body.velocity.Y), Math.Abs(bb2.Bottom - bb1.Top) - 1);

                    if (moveY == 0)
                    {
                      sprite.Body.blocked.Up = true;
                    }
                  }
                  else
                  {
                    moveY = sprite.Body.velocity.Y;
                  }
                }

                sprite.Body.velocity.Y = moveY;
              }
            }
          }

          //sprite.transform.Position += new Vector2(sprite.Body.velocity.X, 0);
          //sprite.transform.Position += new Vector2(0, sprite.Body.velocity.Y);

          sprite.transform.Position += sprite.Body.velocity;
        }
      }
    }
  }
}