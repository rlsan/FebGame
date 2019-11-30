using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FebEngine.GUI;

namespace FebEngine
{
  public class WorldManager : Manager
  {
    public Dictionary<Entity, GameState> entities = new Dictionary<Entity, GameState>();

    public Physics physics;

    public Rectangle bounds;

    public Camera camera;
    public GUICanvas canvas;

    public AudioHandler audio;

    public WorldManager(MainGame game) : base(game)
    {
      physics = new Physics(this);
      camera = new Camera();
      base.Initialize();
    }

    public override void Initialize()
    {
    }

    public override void UnloadContent()
    {
    }

    public Entity AddEntity(Entity entity, GameState state)
    {
      entity.world = this;
      entities.Add(entity, state);
      return entity;
    }

    public void RemoveEntity(Entity entity)
    {
      entities.Remove(entity);
      entity = null;
    }

    public override void Update(GameTime gameTime)
    {
      //Time.Update(gameTime);

      var activeObjects = new StringBuilder();

      foreach (var state in Game.stateManager.states.Values.ToList())
      {
        if (state.IsActive)
        {
          foreach (KeyValuePair<Entity, GameState> entity in entities.ToList())
          {
            // Update if the entity's state matches the state being iterated on.
            if (entity.Value == state)
            {
              if (!state.IsLoaded)
              {
                RemoveEntity(entity.Key);
              }
              else
              {
                entity.Key.Update(gameTime);

                activeObjects.AppendLine(entity.Key.ToString() + " - " + entity.Value.name);
              }
            }
          }

          state.Update(gameTime);

          state.canvas.Update(gameTime);
        }
      }

      //Console.Clear();
      //Console.WriteLine(activeObjects);

      camera.Update(Game.GraphicsDevice.Viewport);
      physics.Update();
    }

    public override void Draw(GameTime gameTime)
    {
    }

    /// <summary>
    /// Returns a list of entities of the type specified.
    /// </summary>
    public List<T> GetEntities<T>()
    {
      List<T> foundEntities = new List<T>();

      foreach (object ent in entities.Keys)
      {
        if (ent is T)
        {
          foundEntities.Add((T)ent);
        }
      }

      return foundEntities;
    }
  }
}