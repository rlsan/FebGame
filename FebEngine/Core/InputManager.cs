using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FebEngine
{
  public class InputManager : Manager
  {
    private Command MoveUp { get; } = new Commands.Interact();
    private Command MoveLeft { get; } = new Commands.Move(-1, 0);
    private Command MoveDown { get; } = new Commands.Duck();
    private Command MoveRight { get; } = new Commands.Move(1, 0);
    private Command ActionA { get; } = new Commands.Jump();
    private Command ActionB { get; } = new Commands.Jump();
    private Command ActionX { get; } = new Commands.Attack();
    private Command ActionY { get; } = new Commands.Attack();
    private Command ActionL { get; } = new Commands.Empty();
    private Command ActionR { get; } = new Commands.Empty();
    private Command Pause { get; } = new Commands.Empty();

    private List<InputBinding> Bindings { get; set; } = new List<InputBinding>();

    private Dictionary<string, Command> Aliases { get { return GetAliases(); } }

    public static Actor player1;
    public static Actor player2;
    public static Actor player3;
    public static Actor player4;

    public InputManager(MainGame game) : base(game)
    {
    }

    private Dictionary<string, Command> GetAliases()
    {
      return new Dictionary<string, Command>
      {
        { "MoveUp", MoveUp },
        { "MoveLeft", MoveLeft },
        { "MoveDown", MoveDown },
        { "MoveRight", MoveRight },
        { "ActionA", ActionA },
        { "ActionB", ActionB },
        { "ActionX", ActionX },
        { "ActionY", ActionY },
        { "ActionL", ActionL },
        { "ActionR", ActionR },
        { "Pause", Pause }
      };
    }

    public void AddBinding(string commandString, string inputString)
    {
      Aliases.TryGetValue(commandString, out Command c);

      if (Enum.TryParse(inputString, out Keys k))
      {
        var b = new InputBinding(k, c);
        Bindings.Add(b);
      }
    }

    public void ClearBindings()
    {
      Bindings.Clear();
    }

    /// <summary>
    /// Returns a list of commands that match the keyboard state.
    /// </summary>
    private List<Command> GetCommands(List<InputBinding> bindings, KeyboardState k)
    {
      var commands = new List<Command>();

      foreach (var binding in bindings)
      {
        if (k.IsKeyDown(binding.input)) commands.Add(binding.command);
      }

      return commands;
    }

    /// <summary>
    /// Gets a list of valid commands and executes them on the given actor.
    /// </summary>
    private void HandleInput(Actor a, KeyboardState k)
    {
      var commands = GetCommands(Bindings, k);
      commands.ForEach(c => c.Execute(a));
    }

    public override void Update(GameTime gameTime)
    {
      KeyboardState k = Keyboard.GetState();

      if (player1 != null) HandleInput(player1, k);
    }

    private class InputBinding
    {
      public Keys input;
      public Command command;

      public InputBinding(Keys input, Command command)
      {
        this.input = input;
        this.command = command;
      }
    }
  }
}