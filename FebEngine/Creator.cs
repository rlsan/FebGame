using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FebEngine.Entities;

namespace FebEngine
{
  public class Creator
  {
    private World world;
    private ContentManager content;

    public Creator(World world, ContentManager content)
    {
      this.world = world;
      this.content = content;
    }

    public Sprite Sprite(string path)
    {
      Sprite s = new Sprite();

      s.Texture = content.Load<Texture2D>(path);

      world.entities.Add(s);
      world.sprites.Add(s);

      return s;
    }

    public Timer Timer()
    {
      var n = new Timer();

      world.entities.Add(n);

      return n;
    }
  }
}