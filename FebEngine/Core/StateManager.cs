using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fubar
{
  public class StateManager : Manager
  {
    public static StateManager instance;

    public Dictionary<string, GameState> states = new Dictionary<string, GameState>();

    public GameState[] ActiveStates
    {
      get
      {
        var matches = states.Where(kvp => !kvp.Value.IsActive);

        GameState[] activeStates = new GameState[matches.Count()];

        int i = 0;

        foreach (var state in matches)
        {
          activeStates[i] = state.Value;
          i++;
        }

        return activeStates;
      }
    }

    public StateManager(MainGame game) : base(game)
    {
      if (instance == null)
      {
        instance = this;
      }

      base.Initialize();
    }

    public override void Initialize()
    {
    }

    public override void UnloadContent()
    {
    }

    public override void Update(GameTime gameTime)
    {
      foreach (var state in states.Values.ToList())
      {
        if (state.IsActive)
        {
          state.Update(gameTime);
        }
      }
    }

    public GameState AddState(string name, GameState state)
    {
      state.name = name;
      state.game = Game;
      state.world = Game.worldManager;
      states.Add(name, state);

      state.Initialize();

      return state;
    }

    public void ActivateState(string name)
    {
      states.TryGetValue(name, out var state);

      if (!state.IsActive)
      {
        state.Activate();
      }
    }

    public void ActivateStateAdditive(string name)
    {
      states.TryGetValue(name, out var state);

      if (!state.IsActive)
      {
        state.Activate();
      }
    }

    public void DeactivateState(string name)
    {
      states.TryGetValue(name, out var state);

      if (state.IsActive)
      {
        state.Deactivate();
      }
    }

    /// <summary>
    /// Deactivates all states.
    /// </summary>
    private void DeactivateAllStates(bool ignoreLocked = false)
    {
      foreach (var state in states)
      {
        if (state.Value.IsActive)
        {
          if (!state.Value.isLocked || ignoreLocked)
          {
            state.Value.Deactivate();
          }
        }
      }
    }

    /// <summary>
    /// Unloads all states.
    /// </summary>
    private void UnloadAllStates(bool ignoreLocked = false)
    {
      foreach (var state in states)
      {
        if (state.Value.IsLoaded)
        {
          if (!state.Value.isLocked || ignoreLocked)
          {
            UnloadState(state.Key);
            InputManager.player1 = null;
            InputManager.player2 = null;
            InputManager.player3 = null;
            InputManager.player4 = null;
          }
        }
      }
    }

    /// <summary>
    /// Deactivate any active states and set a new state to be the sole active state.
    /// </summary>
    /// <param loadInactive="loadInactive">If the state should load without becoming active.</param>
    public void ChangeState(string name, bool locked = false, bool loadInactive = false)
    {
      states.TryGetValue(name, out var state);

      if (!state.IsActive)
      {
        UnloadAllStates();

        state.Load(Game.Content);
        if (!loadInactive)
        {
          state.Activate();
        }

        if (locked)
        {
          state.isLocked = true;
        }

        state.Start();
      }
    }

    public void UnloadState(string name)
    {
      states.TryGetValue(name, out var state);

      if (state.IsActive)
      {
        state.Unload(Game.Content);
        state.Deactivate();

        Console.WriteLine("Unloaded state: {0}", name);
      }
      else
      {
        //Console.WriteLine("State \"{0}\" is already inactive.", name);
      }
    }

    /// <summary>
    /// Activates the specified state.
    /// </summary>
    /// <param loadInactive="loadInactive">If the state should load without becoming active.</param>
    public void LoadStateAdditive(string name, bool loadInactive = false)
    {
      states.TryGetValue(name, out var state);

      if (!state.IsActive)
      {
        if (!loadInactive)
        {
          state.Activate();
        }

        state.Load(Game.Content);
        state.Start();
      }
    }
  }
}