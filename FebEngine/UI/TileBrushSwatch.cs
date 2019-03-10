using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebEngine.UI
{
  public class TileBrushSwatch : Button
  {
    public TileBrush brush;

    public TileBrushSwatch(TileBrush brush, Vector2 position, int size)
    {
      Bounds = new Rectangle(position.ToPoint(), new Point(size));
      this.brush = brush;
    }
  }
}