using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FebEngine.UI
{
  public class TextButton : Button
  {
    public override void Draw(SpriteBatch sb)
    {
      Debug.Text(label, X + 5, Y + 5);

      base.Draw(sb);
    }
  }
}