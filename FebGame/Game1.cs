using Microsoft.Xna.Framework;
using FebGame.States;
using FebEngine;
using ChaiFoxes.FMODAudio;

namespace FebGame
{
  public class Game1 : MainGame
  {
    protected override void Initialize()
    {
      AudioMgr.Init("Content");

      stateManager.AddState("Sandbox", new Sandbox());
      stateManager.AddState("Editor", new MainEditor());
      stateManager.ChangeState("Sandbox");

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
      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      base.Draw(gameTime);
    }
  }
}