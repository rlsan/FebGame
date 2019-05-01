using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebEngine
{
  public class Sprite : Entity
  {
    public Transform transform = new Transform();

    public Texture2D Texture { get; set; }

    public string TexturePath { get; set; }

    public bool isVisible = true;
    public bool isDead = false;

    public void Kill()
    {
      isVisible = false;
      isDead = true;
    }

    public void Revive()
    {
      isVisible = true;
      isDead = false;
    }

    public override void Update(GameTime gt)
    {
    }

    public override void Draw(SpriteBatch sb, GameTime gt)
    {
      if (Texture != null && isVisible)
      {
        sb.Draw(Texture, transform.Position, Color.White);
      }
    }
  }
}