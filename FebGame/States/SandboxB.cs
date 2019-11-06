using FebEngine;
using FebEngine.Tiles;
using FebEngine.UI;
using FebGame.Editor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebGame.States
{
  internal class SandboxB : GameState
  {
    private Tileset tileset;
    private UIElement a;

    public override void Load(ContentManager content)
    {
      tileset = new Tileset(content.Load<Texture2D>("tilesets/ts_test"), 64, 64);
      base.Load(content);
    }

    public override void Start()
    {
      canvas.bounds.Width = 2000;
      canvas.bounds.Height = 2000;

      a = canvas.AddElement("a", new TileEditorWindow(), 100, 100, 1200, 900);
    }

    public override void Unload(ContentManager content)
    {
    }
  }
}