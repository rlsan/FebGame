using FebEngine;
using FebEngine.Tiles;
using FebEngine.GUI;
using FebGame.Editor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace FebGame.States
{
  public class MainEditor : GameState
  {
    internal MapEditor mapEditor;
    internal GroupEditor groupEditor;
    internal TileEditor tileEditor;

    public MapGroup mapGroup;
    public Tileset tileset;

    public Tilemap activeTilemap;

    public Texture2D rectTexture;

    private Vector2 prevCamPos;
    private bool camHasMoved;
    private int previousScroll;
    public bool panning;

    public GUIElement menu;
    public GUIElement body;

    public override void Load(ContentManager content)
    {
      tileset = TilesetIO.Import("Tileset.ats", content);
      rectTexture = content.Load<Texture2D>("recthandles");

      base.Load(content);
    }

    public override void Start()
    {
      canvas.bounds.Height = 1080;
      canvas.bounds.Width = 1920;

      groupEditor = StateManager.instance.AddState("GroupEditor", new GroupEditor()) as GroupEditor;
      groupEditor.editor = this;
      groupEditor.NewMapGroup();

      mapEditor = StateManager.instance.AddState("MapEditor", new MapEditor()) as MapEditor;
      mapEditor.editor = this;

      tileEditor = StateManager.instance.AddState("TileEditor", new TileEditor()) as TileEditor;
      tileEditor.editor = this;

      StateManager.instance.LoadStateAdditive("MapEditor", true);
      StateManager.instance.LoadStateAdditive("GroupEditor", true);
      StateManager.instance.LoadStateAdditive("TileEditor", true);

      var body = canvas.AddElement(new GUIContainer(), 0, 0, 1920, 1080);

      menu = body.AddPanel(1, 30, ScalingType.percentage, ScalingType.absolute);
      menu.anchorPosition = AnchorPosition.TopLeft;
      menu.division = Division.Horizontal;

      menu.AddButton("Map Editor", ActivateMapEditor);
      menu.AddButton("Group Editor", ActivateGroupEditor);
      menu.AddButton("Tile Editor", ActivateTileEditor);
    }

    public override void Update(GameTime gameTime)
    {
      UpdateCameraControl();

      if (Keyboard.GetState().IsKeyDown(Keys.Up)) world.Game.stateManager.LoadState("Sandbox", true);
    }

    internal void ActivateMapEditor()
    {
      mapGroup.Show();

      StateManager.instance.DeactivateState("GroupEditor");
      StateManager.instance.DeactivateState("TileEditor");
      StateManager.instance.ActivateState("MapEditor");

      world.camera.Position = Vector2.Zero;
    }

    internal void ActivateGroupEditor()
    {
      mapGroup.Hide();

      StateManager.instance.DeactivateState("MapEditor");
      StateManager.instance.DeactivateState("TileEditor");
      StateManager.instance.ActivateState("GroupEditor");

      world.camera.Position = Vector2.Zero;
    }

    internal void ActivateTileEditor()
    {
      mapGroup.Hide();

      StateManager.instance.DeactivateState("MapEditor");
      StateManager.instance.DeactivateState("GroupEditor");

      StateManager.instance.ActivateState("TileEditor");

      world.camera.Position = Vector2.Zero;
    }

    private void UpdateCameraControl()
    {
      if (canvas.MouseDown)
      {
        if (canvas.keyboard.IsKeyDown(Keys.Space))
        {
          if (!camHasMoved)
          {
            panning = true;

            prevCamPos = world.camera.Position + canvas.mouse.Position.ToVector2() / world.camera.scaleFactor;
            camHasMoved = true;
          }

          world.camera.Position = -canvas.mouse.Position.ToVector2() / world.camera.scaleFactor + prevCamPos;
        }
      }
      else
      {
        panning = false;

        camHasMoved = false;
      }

      if (canvas.mouse.ScrollWheelValue < previousScroll && world.camera.scaleFactor > 0.5f)
      {
        world.camera.scaleFactor -= 0.1f;
      }
      else if (canvas.mouse.ScrollWheelValue > previousScroll && world.camera.scaleFactor < 2)
      {
        world.camera.scaleFactor += 0.1f;
      }
      previousScroll = canvas.mouse.ScrollWheelValue;
    }
  }
}