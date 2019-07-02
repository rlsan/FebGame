using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace FebEngine
{
  public abstract class GameState
  {
    public string name;
    public bool isActive;

    public World world;

    public void Activate()
    {
      isActive = true;
      //Load();
      //Start();
    }

    public void Deactivate()
    {
      isActive = false;
    }

    public virtual void Load(ContentManager content)
    {
    }

    public virtual void Unload(ContentManager content)
    {
    }

    public virtual void Start()
    {
    }

    public virtual void Update(GameTime gameTime)
    {
    }

    public virtual void Draw(RenderManager renderer, GameTime gameTime)
    {
    }
  }
}