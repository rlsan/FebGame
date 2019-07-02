using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebEngine.Entities
{
  public abstract class Entity
  {
    public string name;
    public Transform Transform { get; } = new Transform();

    public abstract void Update(GameTime gt);

    public abstract void Draw(SpriteBatch sb, GameTime gt);
  }
}