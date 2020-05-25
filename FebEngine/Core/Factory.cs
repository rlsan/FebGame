using Fubar.GUI;
using Fubar.Tiles;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TexturePackerLoader;
using System;

namespace Fubar
{
  public class Factory
  {
    private WorldManager World { get; }
    private GameState State { get; }
    private ContentManager Content { get; }
    private SpriteSheetLoader SpriteSheetLoader { get; }

    public Factory(WorldManager world, GameState state, ContentManager content)
    {
      World = world;
      State = state;
      Content = content;

      SpriteSheetLoader = new SpriteSheetLoader(Content, RenderManager.Instance.GraphicsDevice);
    }

    public Entity Entity(Entity entityToAdd, string name)
    {
      var entity = entityToAdd;
      entity.name = new StringBuilder(name);
      entity.world = World;

      World.entities.Add(entity as Entity, State);

      return entity;
    }

    public T Entity<T>() where T : Entity
    {
      var entity = (Entity)Activator.CreateInstance(typeof(T));

      World.AddEntity(entity, State);
      return entity as T;
    }

    public Sprite Sprite(string name, string path)
    {
      var entity = new Sprite();
      //entity.Texture
      entity.name = new StringBuilder(name);
      entity.world = World;

      entity.SetTexture(SpriteSheetLoader.Load("sp1"));
      entity.Animations.Add("Base", path);

      World.entities.Add(entity as Entity, State);

      return entity;
    }

    public T Sprite<T>(SpriteSheet spriteSheet) where T : Entity
    {
      var sprite = (Sprite)Activator.CreateInstance(typeof(T));

      sprite.SetTexture(spriteSheet);

      World.AddEntity(sprite, State);
      return sprite as T;
    }

    public ParticleEmitter Emitter(string name, string path, int capacity = 1000, bool startEmitting = true, EmitterShape emitterShape = EmitterShape.Circle)
    {
      var entity = new ParticleEmitter();
      entity.name = new StringBuilder(name);
      //entity.SetTexture(spriteSheetLoader.Load(path));
      entity.world = World;

      World.entities.Add(entity as Entity, State);

      return entity;
    }

    public MapGroup MapGroup(string name = "")
    {
      var entity = new MapGroup();
      entity.name = new StringBuilder(name);
      entity.world = World;

      World.entities.Add(entity as Entity, State);

      return entity;
    }

    public Camera Camera(string name)
    {
      var entity = new Camera();
      entity.name = new StringBuilder(name);
      entity.world = World;

      World.entities.Add(entity as Entity, State);

      return entity;
    }

    public Timer Timer(string name)
    {
      var entity = new Timer();
      entity.name = new StringBuilder(name);
      entity.world = World;

      World.entities.Add(entity as Entity, State);

      return entity;
    }

    public GUICanvas Canvas(string name)
    {
      var entity = new GUICanvas(100, 100);
      entity.name = new StringBuilder(name);
      entity.world = World;

      World.entities.Add(entity as Entity, State);

      return entity;
    }
  }
}