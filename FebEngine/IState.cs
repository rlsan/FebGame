using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace FebEngine
{
  public interface IState
  {
    void Load();

    void Unload();

    void Update(GameTime gameTime);

    void Draw(Renderer renderer);
  }
}