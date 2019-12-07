using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using FebEngine;

namespace FebGame.States
{
  public class Sandbox : GameState
  {
    private MapGroup mapGroup;
    private Tileset tileset;

    private Entities.Player player;

    public override void Load(ContentManager content)
    {
      tileset = TilesetIO.Import("test", content);

      mapGroup = Create.Entity<MapGroup>();
      mapGroup.Load("c1a_gc");

      var spring = Create.Entity<Entities.Spring>();
      player = Create.Entity<Entities.Player>();

      base.Load(content);
    }

    public override void Start()
    {
      // Fix this.
      foreach (var map in mapGroup.Tilemaps)
      {
        map.Tileset = tileset;
      }

      Camera.Follow(player);
    }

    public override void Update(GameTime gameTime)
    {
      Vector2 worldMouse = Camera.ToWorld(canvas.mouse.Position);

      if (canvas.MousePress)
      {
        player.Position = worldMouse;
        player.Body.velocity = Vector2.Zero;
      }

      if (player.Position.Y > 2000) player.Position = Vector2.Zero;
      if (canvas.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.E)) StateManager.instance.ChangeState("Editor");

      Debug.DrawRect(player.Body.Bounds);
    }
  }
}