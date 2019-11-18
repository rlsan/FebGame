using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FebEngine.GUI
{
  public class GUIBar : GUIElement
  {
    private GUIText barText = new GUIText();

    public string Title
    {
      get { return barText.message.ToString(); }
      set { barText.SetMessage(value); }
    }

    public override void Init()
    {
      base.Init();

      heightScalingType = ScalingType.absolute;
      barText = AddText(Title);
      barText.alignment = TextAlignment.TopLeft;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(Canvas.Theme, bounds, new Rectangle(0, 146, 28, 28), Color.White);

      base.Draw(spriteBatch);
    }
  }
}