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

    public override void Init()
    {
      window = AddChild("Window", new UIWindow("Map Info", isDraggable: true, isCloseable: false), 1500, 500, 300, 300) as UIWindow;
      mapNameField = window.AddChild("MapNameField", new UITextField(), 20, 20, 100, 20) as UITextField;

      base.Init();
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
    }
  }
}