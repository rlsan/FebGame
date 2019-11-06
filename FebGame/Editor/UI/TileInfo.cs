using FebEngine.Tiles;
using FebEngine.UI;
using Microsoft.Xna.Framework;

namespace FebGame.Editor
{
  internal class TileInfo : UIWindow
  {
    private TileEditorWindow tileEditor;

    private UITextBox text;

    public TileInfo(TileEditorWindow tileEditor)
    {
      this.tileEditor = tileEditor;
    }

    public override void Init()
    {
      AddText("Tile Info", 0.5f);
      text = AddText();

      base.Init();
    }

    public override void Update(GameTime gameTime)
    {
      if (tileEditor.selectedTile != null)
      {
        text.SetMessage(
          "Id: " + tileEditor.selectedTile.id,
          "Name: " + tileEditor.selectedTile.Name,
          "Type: " + tileEditor.selectedTile.GetType().Name,
          "Subtiles: " + tileEditor.selectedTile.Inputs.Count
          //"Selected Subtile: " + tileEditor.brushView.selectedTile.Name
          );
      }

      base.Update(gameTime);
    }
  }
}