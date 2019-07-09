using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using FebEngine;
using FebEngine.UI;
using FebEngine.Utility;

namespace FebGame.States
{
  internal class MapEditor : GameState
  {
    private UICanvas canvas;

    public override void Load(ContentManager content)
    {
      canvas = new UICanvas(game.Window.ClientBounds.Width, game.Window.ClientBounds.Height);

      canvas.ThemeTexture = content.Load<Texture2D>("theme");
    }

    public override void Start()
    {
      var saveDialog = canvas.AddElement("FileSave", new UISaveDialog("txt"), startInvisible: true);
      var loadDialog = canvas.AddElement("FileLoad", new UILoadDialog("txt", onLoad: LoadMap), startInvisible: true);

      canvas.AddElement("SaveButton", new UIButton("Save...", onClick: saveDialog.Enable), 0, 0, 100, 30);
      canvas.AddElement("LoadButton", new UIButton("Load...", onClick: loadDialog.Enable), 100, 0, 100, 30);
    }

    public void LoadMap(string map)
    {
      Console.WriteLine(map);
    }

    public override void Update(GameTime gameTime)
    {
      canvas.Update(gameTime);
    }

    public override void Draw(RenderManager renderer, GameTime gameTime)
    {
      canvas.DrawElements(renderer.SpriteBatch);
    }
  }
}