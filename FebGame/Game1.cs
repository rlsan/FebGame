using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using FebEngine;
using FebGame.States;

namespace FebGame
{
  public class Game1 : MainGame
  {
    protected override void Initialize()
    {
      stateManager.AddState("TestState", new TestState());
      stateManager.AddState("Editor", new Editor());

      stateManager.LoadState("Editor", true);

      IsMouseVisible = true;

      worldManager.bounds = new Rectangle(0, 0, 2000, 2000);

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