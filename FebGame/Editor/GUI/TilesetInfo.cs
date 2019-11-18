using FebEngine.Tiles;
using FebEngine.GUI;
using Microsoft.Xna.Framework;

namespace FebGame.Editor
{
  public class TilesetInfo : GUIContainer
  {
    public Tileset Tileset { get; set; }
    private GUIText text;

    public override void Init()
    {
      AddBar("Tileset Info");

      drawBackground = true;

      text = AddText();
      text.alignment = TextAlignment.TopLeft;

      base.Init();
    }

    public override void Update(GameTime gameTime)
    {
      if (Tileset != null)
      {
        text.SetMessage(
          "Name: " + Tileset.name,
          "Tile count: " + Tileset.BrushCount
          );
      }
      else
      {
        text.SetMessage("No tileset loaded.");
      }
      base.Update(gameTime);
    }
  }
}