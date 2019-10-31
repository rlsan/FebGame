using FebEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebGame
{
  public class UITileMapSetList : UIWindow
  {
    private UIScrollWindow nameBox;

    public UITileMapSetList(string title = "", bool isDraggable = false, bool isCloseable = true) : base(title, isDraggable, isCloseable)
    {
      this.title = title;
      this.isDraggable = isDraggable;
      this.isCloseable = isCloseable;
    }

    public override void Init()
    {
      nameBox = AddChild("ScrollWindow", new UIScrollWindow(10, SelectMap), 0, menuBarHeight, 160 - 25, 250) as UIScrollWindow;

      base.Init();
    }

    public void Refresh(params string[] items)
    {
      nameBox.SetItems(items);
      nameBox.Scroll(10000);
    }

    public void SelectMap(string item)

    {
    }
  }
}