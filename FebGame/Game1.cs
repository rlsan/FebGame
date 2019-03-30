using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FebEngine;
using FebEngine.UI;
using FebEngine.Tiles;

using FebGame.States;
using Microsoft.Xna.Framework.Content;

namespace FebGame
{
  public class Game1 : MainGame
  {
    protected override void Initialize()
    {
      stateManager.AddState("Level Editor", new LevelEditorState());
      stateManager.SetActiveState("Level Editor");

      IsMouseVisible = true;

      // Setup debug
      Texture2D pixelTexture = new Texture2D(GraphicsDevice, 4, 4);

      Color[] colorData = new Color[16];
      for (int i = 0; i < 16; i++)
      {
        colorData[i] = new Color(255, 255, 255, 1f);
      }
      pixelTexture.SetData(colorData);

      Debug.pixelTexture = pixelTexture;

      base.Initialize();
    }

    protected override void LoadContent()
    {
      var t = Content.Load<Texture2D>("foods");
      t.Name = "foods";

      base.LoadContent();
    }

    protected override void UnloadContent()
    {
      base.UnloadContent();
    }

    protected override void Update(GameTime gameTime)
    {
      if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      base.Draw(gameTime);
    }
  }
}