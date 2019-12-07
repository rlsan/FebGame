using System;
using System.IO;

namespace FebEngine
{
  public class Config
  {
    private MainGame Game { get; }

    public Config(MainGame Game)
    {
      this.Game = Game;
    }

    /// <summary>
    /// Sets the specified property using the given parameters.
    /// </summary>
    public void SetProperty(string property, params string[] parameters)
    {
      string parameter = parameters[0];

      // Determine what to do with the property depending on its name and parameter(s).
      switch (property)
      {
        // The resolution of the game window.
        case "Resolution":
          {
            if (int.TryParse(parameters[0], out int w) && int.TryParse(parameters[1], out int h))
            {
              Game.renderManager.ScreenWidth = w;
              Game.renderManager.ScreenHeight = h;

              Game.Graphics.ApplyChanges();
            }
            break;
          }
        // The way that the game will be displayed: in a window, a borderless window, or fullscreen.
        case "DisplayType":
          {
            string displayType = parameter;

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
            if (bool.TryParse(parameter, out bool b)) Game.IsMouseVisible = !b;

            break;
          }
        // The volume (0-100) of all audio playback.
        case "MasterVolume":
          {
            if (int.TryParse(parameter, out int v)) ;//Game.Audio.MasterVolume = v;

            break;
          }
        // The volume (0-100) of music playback.
        case "MusicVolume":
          {
            if (int.TryParse(parameter, out int v)) ;//Game.Audio.MusicVolume = v;

            break;
          }
        // The volume (0-100) of sound effect playback.
        case "SoundEffectVolume":
          {
            if (int.TryParse(parameter, out int v)) ;//Game.Audio.SoundEffectVolume = v;

            break;
          }
      }
    }

    /// <summary>
    /// Sets the properties from the config file at the path.
    /// </summary>
    public void OpenConfigFile(string localPath)
    {
      // Read the file at the local directory.
      string current = Directory.GetCurrentDirectory();
      string[] configFile = File.ReadAllLines(current + localPath);

      // Set the property on each line.
      foreach (var line in configFile)
      {
        if (line.StartsWith("//")) continue;
        if (string.IsNullOrEmpty(line)) continue;

        string property = line.Substring(0, line.IndexOf(" "));
        string[] parameters = line.Replace(property + " = ", "").Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

        SetProperty(property, parameters);
      }
    }

    /// <summary>
    /// Sets the inputs from the config file at the path.
    /// </summary>
    public void OpenInputFile(string localPath)
    {
      // Read the file at the local directory.
      string currentDir = Directory.GetCurrentDirectory();
      string[] inputConfigFile = File.ReadAllLines(currentDir + localPath);

      Game.inputManager.ClearBindings();

      // Set the input on each line.
      foreach (var line in inputConfigFile)
      {
        if (line.StartsWith("//")) continue;
        if (string.IsNullOrEmpty(line)) continue;

        string command = line.Substring(0, line.IndexOf(" "));
        string[] inputs = line.Replace(command + " = ", "").Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var input in inputs)
        {
          Game.inputManager.AddBinding(command, input);
        }
      }
    }
  }
}