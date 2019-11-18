using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebEngine.Entities
{
  public class Sprite : Entity
  {
    public Texture2D Texture { get; set; }
    public Color Tint { get; set; }
    public Vector2 Origin { get; set; }
    public Rectangle Bounds { get; set; }

    public Sprite(Texture2D texture)
    {
      Tint = Color.White;

      Texture = texture;

      Bounds = texture.Bounds;
    }

    public void CenterOrigin()
    {
      Origin = new Vector2(0.5f, 0.5f);
    }

    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
      spriteBatch.Draw(Texture, Position - (Origin * Bounds.Size.ToVector2()), Tint);
    }

    public override void Update(GameTime gameTime)
    {
    }
  }
}