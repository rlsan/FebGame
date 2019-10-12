using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FebEngine.Physics;
using FebEngine.Entities;
using FebEngine.UI;

namespace FebEngine
{
  public class WorldManager : Manager
  {
    public Dictionary<Entity, GameState> entities = new Dictionary<Entity, GameState>();
    public List<Actor> sprites = new List<Actor>();

    public PhysicsHandler physics;

    public Rectangle bounds;

    public Camera camera;
    public UICanvas canvas;

    public WorldManager(MainGame game) : base(game)
    {
      physics = new PhysicsHandler(this);
      camera = new Camera();

      base.Initialize();
    }

    public override void Initialize()
    {
    }

    public override void UnloadContent()
    {
    }

    public void RemoveEntity(Entity entity)
    {
      entities.Remove(entity);
    }

    public override void Update(GameTime gameTime)
    {
      foreach (var state in Game.stateManager.states.Values)
      {
        if (state.IsActive)
        {
          foreach (KeyValuePair<Entity, GameState> entity in entities.ToList())
          {
            // Update if the entity's state matches the state being iterated on.
            if (entity.Value == state)
            {
              entity.Key.Update(gameTime);
            }
          }

          state.Update(gameTime);

          state.canvas.Update(gameTime);
        }
      }

      camera.Update(Game.GraphicsDevice.Viewport);
      physics.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
    }
  }
}