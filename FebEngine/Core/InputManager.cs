using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FebEngine
{
  public class InputManager : Manager
  {
    private KeyboardState oldState;

    private Command Jump { get; } = new Commands.Jump();
    private Command MoveLeft { get; } = new Commands.Move(-1, 0);
    private Command MoveRight { get; } = new Commands.Move(1, 0);

    public Dictionary<Keys, Command> SingleBindings { get; set; } = new Dictionary<Keys, Command>();
    public Dictionary<Keys, Command> ConstantBindings { get; set; } = new Dictionary<Keys, Command>();

    public Dictionary<string, Command> ControlMapping
    {
      get { return InitControls(); }
    }

    public static Actor actor;

    public InputManager(MainGame game) : base(game)
    {
    }

    public Dictionary<string, Command> InitControls()
    {
      return new Dictionary<string, Command>
      {
        { "Jump", Jump },
        { "MoveLeft", MoveLeft },
        { "MoveRight", MoveRight },
      };
    }

    /// <summary>
    /// Returns a list of commands that match the keyboard state.
    /// </summary>
    private List<Command> GetCommands(Dictionary<Keys, Command> b, KeyboardState k)
    {
      List<Command> commands = b.Where(
        x => k.IsKeyDown(x.Key))
        .Select(x => x.Value)
        .ToList();

      return commands;
    }

    /// <summary>
    /// Gets a list of valid commands and executes them on the given actor.
    /// </summary>
    private void HandleInput(Actor a)
    {
      KeyboardState newState = Keyboard.GetState();

      var commands = GetCommands(SingleBindings, newState);
      commands.ForEach(c => c.Execute(a));
    }

    public override void Update(GameTime gameTime)
    {
      HandleInput(actor);
    }
  }
}