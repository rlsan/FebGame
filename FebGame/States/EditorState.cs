using FebEngine;
using FebEngine.Tiles;
using FebEngine.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FebGame.States
{
  internal class EditorState : GameState
  {
    internal MapEditor mapEditor;
    internal GroupEditor groupEditor;
    internal TileEditor tileEditor;

    public MapGroup mapGroup = new MapGroup();
    public Tilemap activeTilemap;

    private Vector2 prevCamPos;
    private bool camHasMoved;
    private int previousScroll;
    public bool panning;

    public override void Start()
    {
      canvas.bounds.Height = 30;
      canvas.bounds.Width = 1920;

      mapEditor = StateManager.instance.AddState("MapEditor", new MapEditor()) as MapEditor;
      groupEditor = StateManager.instance.AddState("GroupEditor", new GroupEditor()) as GroupEditor;
      tileEditor = StateManager.instance.AddState("TileEditor", new TileEditor()) as TileEditor;

      StateManager.instance.LoadStateAdditive("MapEditor", true);
      StateManager.instance.LoadStateAdditive("GroupEditor", true);
      StateManager.instance.LoadStateAdditive("TileEditor", true);

      canvas.AddElement("MapEditorTab", new UIButton(title: "Map Editor", onClick: ActivateMapEditor), 0, 0, 150, 30);
      canvas.AddElement("GroupEditorTab", new UIButton(title: "Group Editor", onClick: ActivateGroupEditor), 150, 0, 150, 30);
      canvas.AddElement("TileEditorTab", new UIButton(title: "Tile Editor", onClick: ActivateTileEditor), 300, 0, 150, 30);

      canvas.AddElement("NewGroup", new UIButton(title: "New Group", onClick: NewMapGroup), 450, 0, 150, 30);
      canvas.AddElement("SaveGroup", new UIButton(title: "Save Group...", onClick: SaveGroup), 600, 0, 150, 30);
      canvas.AddElement("LoadGroup", new UIButton(title: "Load Group...", onClick: LoadGroup), 750, 0, 150, 30);

      mapEditor.editor = this;
      groupEditor.editor = this;
      tileEditor.editor = this;
    }

    private void SaveGroup()
    {
      GroupIO.Export(mapGroup, "/");
    }

    private void LoadGroup()
    {
      //var group = GroupIO.Import("Group1.amg");
      //mapGroup = group;

      mapGroup.Load("Group1.amg");

      groupEditor.LoadGroupThumbs();
    }

    public override void Update(GameTime gameTime)
    {
      UpdateCameraControl();
    }

    private void NewMapGroup()
    {
      mapGroup = new MapGroup { Name = "Group1" };

      groupEditor.LoadGroupThumbs();
    }

    internal void ActivateMapEditor()
    {
      StateManager.instance.DeactivateState("GroupEditor");
      StateManager.instance.DeactivateState("TileEditor");
      StateManager.instance.ActivateState("MapEditor");

      world.camera.Position = Vector2.Zero;
    }

    internal void ActivateGroupEditor()
    {
      StateManager.instance.DeactivateState("MapEditor");
      StateManager.instance.DeactivateState("TileEditor");
      StateManager.instance.ActivateState("GroupEditor");

      world.camera.Position = Vector2.Zero;
    }

    internal void ActivateTileEditor()
    {
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