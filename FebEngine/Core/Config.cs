using System;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace FebEngine
{
  public class Config
  {
    private MainGame Game { get; }
    private string[] configFile;

    public Config(MainGame Game)
    {
      this.Game = Game;
    }

    /// <summary>
    /// Sets the specified property using a special syntax.
    /// Example A: "Resolution 1920 1080"
    /// Example B: "HideCursor True"
    /// </summary>
    public void SetProperty(string property)
    {
      // Return the parameter as a string.
      string GetParam(string matchCase)
      {
        if (property.StartsWith(matchCase))
        {
          string parameter = property.Replace(matchCase + " ", "");
          return parameter.Split('"', '"')[1];
        }

        return string.Empty;
      }

      // Return the parameters as a string array.
      string[] GetParams(string matchCase)
      {
        if (property.StartsWith(matchCase))
        {
          string parameter = property.Replace(matchCase + " ", "");
          return parameter.Split('"', '"')[1].Split(' ');
        }

        return new string[0];
      }

      // Return the parameter as an int.
      int GetIntParam(string matchCase)
      {
        return int.Parse(GetParam(matchCase));
      }

      // Return the parameters as an int array.
      int[] GetIntParams(string matchCase)
      {
        return GetParams(matchCase).Select(int.Parse).ToArray();
      }

      // Return the parameter as a boolean.
      bool GetBoolParam(string matchCase)
      {
        return bool.Parse(GetParam(matchCase));
      }

      // Lines that begin with double slashes are comments.
      if (property.StartsWith("//")) return;
      // Ignore lines that are empty.
      if (string.IsNullOrEmpty(property)) return;

      // Chop off the latter part of the property string to extract the name.
      string propertyName = property.Substring(0, property.IndexOf(" "));

      // Determine what to do with the property depending on its name and parameter(s).
      switch (propertyName)
      {
        // The resolution of the game window.
        case "Resolution":
          {
            int[] size = GetIntParams("Resolution");

            RenderManager.Instance.ScreenWidth = size[0];
            RenderManager.Instance.ScreenHeight = size[1];

            Game.Graphics.ApplyChanges();
            break;
          }
        // The way that the game will be displayed: window, borderless, or fullscreen.
        case "DisplayType":
          {
            string displayType = GetParam("DisplayType");

            if (displayType == "Window")
            {
              Game.Graphics.IsFullScreen = false;
              Game.Window.IsBorderless = false;
            }
            else if (displayType == "BorderlessWindow")
            {
              Game.Graphics.IsFullScreen = false;
              Game.Window.IsBorderless = true;
            }
            else if (displayType == "Fullscreen")
            {
              Game.Graphics.IsFullScreen = true;
            }
            break;
          }
        // If the cursor should be hidden or not.
        case "HideCursor":
          {
            Game.IsMouseVisible = !GetBoolParam("HideCursor");
            break;
          }
        // Binds an input to a specific command.
        case "Bind":
          {
            string[] controls = GetParams("Bind");

            string inputString = controls[0];
            string commandString = controls[1];

            if (Game.inputManager.ControlMapping.TryGetValue(commandString, out Command c))
            {
              Game.inputManager.SingleBindings.Add((Keys)Enum.Parse(typeof(Keys), inputString), c);
            }
            break;
          }
      }
    }

    /// <summary>
    /// Sets the properties found in "config.cfg".
    /// </summary>
    public void RefreshConfig()
    {
      // Read the file at the local directory.
      string current = Directory.GetCurrentDirectory();
      configFile = File.ReadAllLines(current + @"\config.cfg");

      // Check if the file contains at least one "bind" command.
      // If it does, clear the input bindings as they're going to be reset.
      string bindString = configFile.ToList().Find(s => s.StartsWith("Bind"));
      if (bindString != null) Game.inputManager.SingleBindings.Clear();

      // Set the property on each line.
      foreach (var property in configFile) SetProperty(property);
    }
  }
}