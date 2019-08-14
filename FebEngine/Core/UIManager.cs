using FebEngine.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebEngine.Core
{
  public class UIManager : Manager
  {
    public UICanvas canvas;

    public static UIManager instance;

    public UIManager(MainGame game) : base(game)
    {
      base.Initialize();

      canvas = new UICanvas(Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
    }

    public override void Initialize()
    {
      if (instance == null)
      {
        instance = this;
      }

      base.Initialize();
    }

    public override void LoadContent(ContentManager content)
    {
      canvas.ThemeTexture = content.Load<Texture2D>("theme");
    }

    public override void Update(GameTime gameTime)
    {
      canvas.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
    }
  }
}