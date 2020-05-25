using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Fubar
{
  public class InputManager : Manager
  {
    private Command MoveUp { get; } = new Commands.Move(0, -1);
    private Command MoveLeft { get; } = new Commands.Move(-1, 0);
    private Command MoveDown { get; } = new Commands.Move(0, 1);
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

    public void AddKeyBinding(string commandString, string inputString)
    {
      Aliases.TryGetValue(commandString, out Command c);

      if (Enum.TryParse(inputString, out Keys k))
      {
        var bind = new InputBinding(k, c);
        Bindings.Add(bind);
      }
    }

    public void AddPadBinding(string commandString, string inputString)
    {
      Aliases.TryGetValue(commandString, out Command c);

      if (Enum.TryParse(inputString, out Buttons b))
      {
        var bind = new InputBinding(b, c);
        Bindings.Add(bind);
      }
    }

    public void ClearBindings()
    {
      Bindings.Clear();
    }

    /// <summary>
    /// Returns a list of commands that match the keyboard state.
    /// </summary>
    private List<Command> GetCommands(List<InputBinding> bindings, KeyboardState k, GamePadState g)
    {
      var commands = new List<Command>();

      foreach (var binding in bindings)
      {
        if (!commands.Contains(binding.command))
        {
          if (k.IsKeyDown(binding.keyInput)) commands.Add(binding.command);

          if (binding.buttonInput != 0)
          {
            if (g.IsButtonDown(binding.buttonInput))
            {
              commands.Add(binding.command);
            }
          }
        }
      }

      return commands;
    }

    /// <summary>
    /// Gets a list of valid commands and executes them on the given actor.
    /// </summary>
    private void HandleInput(Actor a, KeyboardState k, GamePadState g)
    {
      var commands = GetCommands(Bindings, k, g);
      commands.ForEach(c => c.Execute(a));
    }

    public override void Update(GameTime gameTime)
    {
      KeyboardState k = Keyboard.GetState();

      GamePadState g1 = GamePad.GetState(0);

      if (player1 != null) HandleInput(player1, k, g1);
    }

    private class InputBinding
    {
      public Keys keyInput;
      public Buttons buttonInput;

      public Command command;

      public InputBinding(Keys input, Command command)
      {
        this.keyInput = input;
        this.command = command;
      }

      public InputBinding(Buttons input, Command command)
      {
        this.buttonInput = input;
        this.command = command;
      }
    }
  }
}