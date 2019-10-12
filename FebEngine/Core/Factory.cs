using FebEngine.Entities;
using FebEngine.UI;
using FebEngine.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace FebEngine
{
  public class Factory
  {
    private WorldManager World { get; set; }
    private GameState State { get; set; }
    private ContentManager Content { get; set; }

    public Factory(WorldManager world, GameState state, ContentManager content)
    {
      World = world;
      State = state;
      Content = content;
    }

    public Entity Entity(Entity entityToAdd, string name)
    {
      var entity = entityToAdd;
      entity.Name = name;
      entity.world = World;

      World.entities.Add(entity as Entity, State);

      return entity;
    }

    public Sprite Sprite(string name, string textureName = "missing")
    {
      var entity = new Sprite(Content.Load<Texture2D>(textureName));
      entity.Name = name;
      entity.world = World;

      World.entities.Add(entity as Entity, State);

      return entity;
    }

    public MapGroup MapGroup(string name)
    {
      var entity = new MapGroup();
      entity.Name = name;
      entity.world = World;

      World.entities.Add(entity as Entity, State);

      return entity;
    }

    public Camera Camera(string name)
    {
      var entity = new Camera();
      entity.Name = name;
      entity.world = World;

      World.entities.Add(entity as Entity, State);

      return entity;
    }

    public UICanvas Canvas(string name)
    {
      var entity = new UICanvas(100, 100);
      entity.Name = name;
      entity.world = World;

      World.entities.Add(entity as Entity, State);

      return entity;
    }
  }
}