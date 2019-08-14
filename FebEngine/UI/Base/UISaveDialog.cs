using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FebEngine.UI
{
  public class UISaveDialog : UIFileDialog
  {
    private Action<object> onSave;
    public string textToSave;
    public string extension;

    public UISaveDialog(string extension, Action<object> onSave = null)
    {
      this.extension = extension;
      this.onSave = onSave;
    }

    public override void Init()
    {
      base.Init();

      window.AddChild("SaveButton", new UIButton(title: "Save", onClick: CheckFile), 555, 460, 50, 25);
    }

    private void CheckFile()
    {
      string path = currentDir + "\\" + nameField.text + "." + extension;
      if (File.Exists(path))
      {
        Canvas.ShowPrompt("Warning", "This file exists. Overwrite?", new UIButton("Save", Save), new UIButton("Cancel", Canvas.ClosePrompt));
      }
      else
      {
        Save();
        Disable();
        Canvas.ClosePrompt();
      }
    }

    private void Save()
    {
      string path = currentDir + "\\" + nameField.text + "." + extension;

      //File.WriteAllText(path, textToSave);

      onSave.DynamicInvoke(path);

      Disable();
      Canvas.ClosePrompt();
    }
  }
}