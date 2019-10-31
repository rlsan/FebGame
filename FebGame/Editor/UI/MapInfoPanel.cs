using FebEngine.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebGame
{
  public class MapInfoPanel : UIElement
  {
    private UIWindow window;
    public UITextField mapNameField;
    public UITextField mapMusicField;

    public override void Init()
    {
      window = AddChild("Window", new UIWindow("Map Info", isDraggable: true, isCloseable: false), 1500, 500, 300, 200) as UIWindow;

      window.AddChild("MapNameLabel", new UITextBox("Name:"), 20, window.menuBarHeight + 20, 0, 0);
      mapNameField = window.AddChild("MapNameField", new UITextField(), 100, window.menuBarHeight + 20, 200, 20) as UITextField;

      window.AddChild("MapMusicLabel", new UITextBox("Music:"), 20, window.menuBarHeight + 40, 0, 0);
      mapMusicField = window.AddChild("MapMusicField", new UITextField(), 100, window.menuBarHeight + 40, 200, 20) as UITextField;

      base.Init();
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
    }
  }
}