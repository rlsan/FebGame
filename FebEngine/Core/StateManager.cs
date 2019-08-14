using FebEngine.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebEngine
{
  public class StateManager : Manager
  {
    public static StateManager instance;

    public Dictionary<string, GameState> states = new Dictionary<string, GameState>();

    public GameState[] ActiveStates
    {
      get
      {
        var matches = states.Where(kvp => !kvp.Value.isActive);

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
      base.Initialize();
    }

    public override void Initialize()
    {
      if (instance == null)
      {
        instance = this;
      }
    }

    public override void UnloadContent()
    {
    }

    public override void Update(GameTime gameTime)
    {
      Debug.Clear();

      foreach (var state in states.Values)
      {
        if (state.isActive)
        {
          state.Update(gameTime);
        }
      }
    }

    public void AddState(string name, GameState state)
    {
      state.name = name;
      state.game = Game;
      state.world = Game.world;
      state.canvas = Game.uiManager.canvas;
      states.Add(name, state);
    }

    public void SetActiveState(string name)
    {
      states.TryGetValue(name, out var state);

      state.Activate();
      state.Load(Game.Content);
      state.Start();
    }
  }
}