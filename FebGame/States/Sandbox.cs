using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using FebEngine;
using TexturePackerLoader;

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

      mapGroup = Create.Entity<MapGroup>();
      mapGroup.Load("c1a_gc");

      var spring = Create.Sprite<Objects.Spring>(sheet);
      spring.Position = new Vector2(400, 400);

      //var pepper = Create.Sprite<Objects.Pepper>(sheet);
      //pepper.Position = new Vector2(500, 600);

      player = Create.Sprite<Objects.Player>(sheet);
      player.Position = new Vector2(200, 400);

      for (int i = 0; i < 10; i++)
      {
        var butterfly1 = Create.Sprite<Objects.Butterfly>(sheet);
        butterfly1.Position = new Vector2(600, 850) + RNG.PointInsideUnitCircle() * 10;
        butterfly1.SetHome();
      }

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

      if (canvas.MouseDown)
      {
        mapGroup.CurrentMap.GetLayer(1).PutTile(0, worldMouse);
      }

      if (player.Position.Y > 2000) player.Position = Vector2.Zero;

      if (canvas.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.E)) StateManager.instance.ChangeState("Editor");
    }
  }
}