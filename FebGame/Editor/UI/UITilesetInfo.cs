using FebEngine.Tiles;
using FebEngine.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebGame.Editor
{
  internal class UITilesetInfo : UIWindow
  {
    private UIWindow window;
    public Tileset tileset;

    public UITextField mapNameField;
    public UITextField mapMusicField;

    public UITilesetInfo(Tileset tileset)
    {
      this.tileset = tileset;
    }

    public override void Init()
    {
      window = AddChild("Window", new UIWindow("Tileset Info", isDraggable: false, isCloseable: false), 960, 30, 960, 400) as UIWindow;

      window.AddChild("MapNameLabel", new UITextBox("Name:"), 20, window.menuBarHeight + 20, 0, 0);
      mapNameField = window.AddChild("MapNameField", new UITextField(), 100, window.menuBarHeight + 20, 200, 20) as UITextField;

      window.AddChild("MapMusicLabel", new UITextBox("Tiles:"), 20, window.menuBarHeight + 40, 0, 0);
      mapMusicField = window.AddChild("MapMusicField", new UITextField(), 100, window.menuBarHeight + 40, 200, 20) as UITextField;

      base.Init();
    }

    public override void Update(GameTime gameTime)
    {
      mapNameField.SetMessage(tileset.name);
      mapMusicField.SetMessage(tileset.BrushCount);
      base.Update(gameTime);
    }
  }
}