using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FebEngine.Utility;

namespace FebEngine.UI
{
  public class UITextButton : UIButton
  {
    private string text;

    public UITextButton(string text, int x, int y, int width, int height)
    {
      this.text = text;
      bounds = new Rectangle(x, y, width, height);
    }

    public override void Draw(SpriteBatch sb)
    {
      Debug.Text(text, X + 2, Y + 2);

      base.Draw(sb);
    }
  }
}