using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FebEngine.Physics;
using FebEngine.Entities;

namespace FebEngine
{
  public class World : Manager
  {
    public List<Entity> entities = new List<Entity>();
    public List<Actor> sprites = new List<Actor>();

    public Creator create;
    public PhysicsHandler physics;

    public Rectangle bounds;

    public Camera camera;

    public World(MainGame game) : base(game)
    {
      create = new Creator(this, game.Content);
      physics = new PhysicsHandler(this);
      camera = Add(new Camera()) as Camera;
      base.Initialize();
    }

    public Entity Add(Entity ent)
    {
      entities.Add(ent);
      return ent;
    }

    public Actor AddSprite(Actor s)
    {
      //s.Texture = Game.Content.Load<Texture2D>(s.TexturePath);
      entities.Add(s);
      sprites.Add(s);
      return s;
    }

    public override void Initialize()
    {
    }

    public override void UnloadContent()
    {
    }

    public override void Update(GameTime gameTime)
    {
      foreach (var ent in entities)
      {
        ent.Update(gameTime);
      }

      physics.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
    }
  }
}