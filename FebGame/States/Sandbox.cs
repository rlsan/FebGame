using Microsoft.Xna.Framework;
using FebEngine;
using TexturePackerLoader;

namespace FebGame.States
{
  public class Sandbox : GameState
  {
    private SpriteSheet spriteSheet;
    private SpriteSheet effectSheet;

    private MapGroup mapGroup;
    private Tileset tileset;

    private Sprite player;

    public override void Load(Microsoft.Xna.Framework.Content.ContentManager content)
    {
      tileset = TilesetIO.Import("Tileset.ats", content);

      var spriteSheetLoader = new SpriteSheetLoader(content, RenderManager.Instance.GraphicsDevice);

      spriteSheet = spriteSheetLoader.Load("sp1");
      effectSheet = spriteSheetLoader.Load("ef1");

      base.Load(content);
    }

    public override void Start()
    {
      mapGroup = Create.MapGroup("MapGroup");
      mapGroup.Load(@"C:\Users\Public\Test\Group1.amg");
      mapGroup.ChangeMap(0);

      foreach (var map in mapGroup.Tilemaps)
      {
        map.Tileset = tileset;
      }

      player = world.AddEntity(new Player(), this) as Player;

      Camera.Follow(player);
    }

    public override void Update(GameTime gameTime)
    {
      var worldMouse = Camera.ToWorld(canvas.mouse.Position);

      if (canvas.MousePress)
      {
        player.Position = worldMouse;
        player.Body.Reset();
      }

      if (player.Position.Y > 2000) player.Position = Vector2.Zero;
      //if (canvas.IsKeyDown(Keys.E)) StateManager.instance.ChangeState("Editor");

      Debug.DrawRect(player.Body.Bounds);
    }
  }
}