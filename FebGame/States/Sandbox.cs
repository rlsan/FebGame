using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using FebEngine;
using TexturePackerLoader;
using System;

namespace FebGame.States
{
  public class Sandbox : GameState
  {
    private MapGroup mapGroup;
    private Tileset tileset;

    private SpriteSheetLoader spriteSheetLoader;

    private Objects.Player player;

    public override void Load(ContentManager content)
    {
      tileset = TilesetIO.Import("test", content);

      spriteSheetLoader = new SpriteSheetLoader(content, RenderManager.Instance.GraphicsDevice);

      var sheet = spriteSheetLoader.Load("sp1");
      var effectSheet = spriteSheetLoader.Load("ef1");

      mapGroup = Create.Entity<MapGroup>();
      mapGroup.Load("c1a_gc");

      var spring = Create.Sprite<Objects.Spring>(sheet);
      spring.Position = new Vector2(400, 400);

      var pepper = Create.Sprite<Objects.Pepper>(sheet);
      pepper.Position = new Vector2(500, 600);

      player = Create.Sprite<Objects.Player>(sheet);
      player.Position = new Vector2(200, 400);

      for (int i = 0; i < 3; i++)
      {
        var butterfly1 = Create.Sprite<Objects.Butterfly>(sheet);
        butterfly1.Position = new Vector2(600, 850) + RNG.PointInsideUnitCircle() * 10;
        butterfly1.SetHome();
      }

      var checkpoint1 = Create.Sprite<Objects.Checkpoint>(sheet);
      checkpoint1.Position = new Vector2(800, 850);

      var checkpoint2 = Create.Sprite<Objects.Checkpoint>(sheet);
      checkpoint2.Position = new Vector2(1000, 850);

      base.Load(content);
    }

    public override void Start()
    {
      // Fix this.
      foreach (var map in mapGroup.Tilemaps)
      {
        map.Tileset = tileset;
      }

      //Camera.Follow(player);
      Camera.scaleFactor = 1.5f;
    }

    public override void Update(GameTime gameTime)
    {
      var map = mapGroup.CurrentMap;
      var rect = new Rectangle(0, 0, map.Bounds.Width * map.TileWidth, map.Bounds.Height * map.TileHeight);

      Camera.Position = player.Position;

      //var v = Camera.Viewport;

      //Rectangle.Intersect(ref v, ref rect, out Rectangle r);

      //Camera.Position = r.Location.ToVector2();

      var clamped = Camera.Viewport.Clamp(rect);

      Camera.Position = clamped.Location.ToVector2() + new Vector2(Camera.Viewport.Width / 2, Camera.Viewport.Height / 2);

      Debug.Text(clamped, Camera.Position);
      Debug.DrawRect(clamped);
      //Debug.DrawRect(r);
      //System.Console.WriteLine(map.Bounds);
      /*
      float x = 0;
      if (Camera.Viewport.Left > 0 && Camera.Viewport.Right < map.Bounds.Width)
      {
        x = Math.Max(0, Camera.Viewport.Left);
      }

      Camera.Position = new Vector2(player.Position.X, Camera.Position.Y);
      Camera.Position = new Vector2(Camera.Position.X, player.Position.Y);
      */

      Vector2 worldMouse = Camera.ToWorld(canvas.mouse.Position);

      if (canvas.MouseDown)
      {
        mapGroup.CurrentMap.GetLayer(1).PutTile(0, worldMouse);
      }

      foreach (var item in world.GetEntities<Sprite>())
      {
        //Debug.DrawRect(item.Body.Bounds);
      }

      if (player.Position.Y > 2000) player.Position = player.respawnPoint;

      if (canvas.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.E)) StateManager.instance.ChangeState("Editor");
    }
  }
}