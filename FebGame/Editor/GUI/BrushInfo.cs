using FebEngine.Tiles;
using FebEngine.GUI;
using Microsoft.Xna.Framework;
using System.Linq;

namespace FebGame.Editor
{
  /// <summary>
  /// Displays information about the brush provided.
  /// </summary>
  public class BrushInfo : GUIContainer
  {
    public TileBrush Brush { get; set; }
    private GUIText text;

    public override void Init()
    {
      //AddBar("Brush Info");
      text = AddText(alignment: TextAlignment.TopLeft);

      base.Init();
    }

    public override void Update(GameTime gameTime)
    {
      if (Brush != null)
      {
        var indexes = string.Join(", ", from item in Brush.Inputs select item.Name);
        string subtiles = "No Subtiles";

        if (Brush.HasInputs)
        {
          subtiles = Brush.Inputs.Count + " Subtiles: " + indexes;
        }

        text.SetMessage(
          "Name: " + Brush.Name,
          "Id: " + Brush.id,
          "Type: " + Brush.GetType().Name,
          subtiles
          );
      }
      else
      {
        text.SetMessage("No brush selected.");
      }

      base.Update(gameTime);
    }
  }
}