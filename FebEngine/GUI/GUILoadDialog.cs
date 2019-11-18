using System;
using System.IO;

namespace FebEngine.GUI
{
  public class GUILoadDialog : GUIFileDialog
  {
    public Action<string> onLoad;
    public string textToSave;
    public string extension;

    public GUILoadDialog(Action<string> onLoad = null)
    {
      this.onLoad = onLoad;
    }

    public override void Init()
    {
      base.Init();

      options.AddButton("Load", CheckFile, 2);

      //window.AddChild("LoadButton", new UIButton(title: "Load", onClick: CheckFile), 555, 460, 50, 25);
    }

    private void CheckFile()
    {
      string path = currentDir + "\\" + nameField.text + "." + extension;

      if (File.Exists(path))
      {
        Load(path);
      }
      else
      {
        //Canvas.ShowPrompt("Error", "This file does not exist.", new UIButton("OK", Canvas.ClosePrompt));
      }
    }

    private void Load(string path)
    {
      //string text = File.ReadAllText(path);
      string text = path;
      onLoad.DynamicInvoke(text);

      Canvas.locked = false;
      Disable();
    }
  }
}