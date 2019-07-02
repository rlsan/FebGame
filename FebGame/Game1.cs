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
      stateManager.AddState("Gameplay", new GameplayState());
      //stateManager.AddState("Editor", new EditorState());
      stateManager.AddState("MapEditor", new MapEditorState());
      stateManager.SetActiveState("MapEditor");

      IsMouseVisible = true;

      world.bounds = new Rectangle(0, 0, 2000, 2000);

      base.Initialize();
    }

    protected override void LoadContent()
    {
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