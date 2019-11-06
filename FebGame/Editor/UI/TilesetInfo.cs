using FebEngine.Tiles;
using FebEngine.UI;
using Microsoft.Xna.Framework;

namespace FebGame.Editor
{
  internal class TilesetInfo : UIWindow
  {
    private TileEditorWindow tileEditor;
    private UITextBox text;

    public TilesetInfo(TileEditorWindow tileEditor)
    {
      this.tileEditor = tileEditor;
    }

    public override void Init()
    {
      AddText("Tileset Info", 0.5f);
      text = AddText();

      base.Init();
    }

    public override void Update(GameTime gameTime)
    {
      if (tileEditor.tileset != null)
      {
        text.SetMessage(
          "Name: " + tileEditor.tileset.name,
          "Tile count: " + tileEditor.tileset.BrushCount
          );
      }
      base.Update(gameTime);
    }
  }
}