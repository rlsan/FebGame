using System;
using System.IO;

namespace FebEngine.UI
{
  public class UILoadDialog : UIFileDialog
  {
    private Action<string> onLoad;
    public string textToSave;
    public string extension;

    public UILoadDialog(string extension, Action<string> onLoad = null)
    {
      this.extension = extension;
      this.onLoad = onLoad;
    }

    public override void Init()
    {
      base.Init();

      window.AddChild("LoadButton", new UIButton(title: "Load", onClick: CheckFile), 555, 460, 50, 25);
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
        Canvas.ShowPrompt("Error", "This file does not exist.", new UIButton("OK", Canvas.ClosePrompt));
      }
    }

    private void Load(string path)
    {
      string text = File.ReadAllText(path);
      onLoad.DynamicInvoke(text);

      Disable();
    }
  }
}