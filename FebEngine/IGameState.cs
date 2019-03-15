using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace FebEngine
{
  public interface IGameState
  {
    void Load();

    void Unload();

    void Update(GameTime gameTime);

    void Draw(RenderManager renderer);
  }
}