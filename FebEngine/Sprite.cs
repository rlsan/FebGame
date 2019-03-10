using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebEngine
{
  public class Sprite
  {
    Transform Transform { get; set; }

    Texture2D Texture { get; set; }

    string TexturePath { get; set; }
  }
}