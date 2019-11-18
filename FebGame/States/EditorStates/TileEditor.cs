using FebEngine;
using FebEngine.GUI;
using FebGame.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebGame.States
{
  public class TileEditor : GameState
  {
    public MainEditor editor;

    public override void Start()
    {
      var mapView = canvas.AddElement(new GUITileView(editor.tileset), 0, 30, 1920, 1080 - 30) as GUITileView;
      mapView.anchorPosition = AnchorPosition.Bottom;
    }
  }
}