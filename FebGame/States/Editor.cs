using FebEngine;
using FebEngine.Tiles;
using FebEngine.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebGame.States
{
  internal class Editor : GameState
  {
    internal MapEditor mapEditor;
    internal GroupEditor groupEditor;
    internal TileEditor tileEditor;

    public MapGroup mapGroup;
    public Tileset tileset;

    public override void Start()
    {
      mapEditor = StateManager.instance.AddState("MapEditor", new MapEditor()) as MapEditor;
      groupEditor = StateManager.instance.AddState("GroupEditor", new GroupEditor()) as GroupEditor;
      tileEditor = StateManager.instance.AddState("TileEditor", new TileEditor()) as TileEditor;

      StateManager.instance.LoadStateAdditive("MapEditor", true);
      StateManager.instance.LoadStateAdditive("GroupEditor", true);
      StateManager.instance.LoadStateAdditive("TileEditor", true);

      canvas.AddElement("MapEditorTab", new UIButton(title: "Map Editor", onClick: ActivateMapEditor), 0, 0, 150, 30);
      canvas.AddElement("GroupEditorTab", new UIButton(title: "Group Editor", onClick: ActivateGroupEditor), 150, 0, 150, 30);
      canvas.AddElement("TileEditorTab", new UIButton(title: "Tile Editor", onClick: ActivateTileEditor), 300, 0, 150, 30);
    }

    public override void Update(GameTime gameTime)
    {
      mapGroup = groupEditor.mapGroup;
    }

    internal void ActivateMapEditor()
    {
      StateManager.instance.DeactivateState("GroupEditor");
      StateManager.instance.DeactivateState("TileEditor");
      StateManager.instance.ActivateState("MapEditor");
    }

    internal void ActivateGroupEditor()
    {
      StateManager.instance.DeactivateState("MapEditor");
      StateManager.instance.DeactivateState("TileEditor");
      StateManager.instance.ActivateState("GroupEditor");
    }

    internal void ActivateTileEditor()
    {
      StateManager.instance.DeactivateState("MapEditor");
      StateManager.instance.DeactivateState("GroupEditor");
      StateManager.instance.ActivateState("TileEditor");
    }
  }
}