using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Fubar.GUI
{
  public class GUISaveDialog : GUIFileDialog
  {
    private Action<object> onSave;
    public object fileToSave;
    public string extension;

    public GUISaveDialog(Action<object> onSave = null)
    {
      this.onSave = onSave;
    }

    public override void Init()
    {
      base.Init();

      options.AddButton("Save", Save, 2);
      //window.AddChild("SaveButton", new UIButton(title: "Save", onClick: CheckFile), 555, 460, 50, 25);
    }

    private void CheckFile()
    {
      string path = currentDir + "\\" + nameField.text + "." + extension;
      if (File.Exists(path))
      {
        Canvas.ShowPrompt("Warning", "This file exists. Overwrite?", Save);
      }
      else
      {
        Save();
        Disable();
        //Canvas.ClosePrompt();
      }
    }

    private void Save()
    {
      string path = currentDir + "\\" + nameField.text + "." + extension;

      File.WriteAllText(path, fileToSave.ToString());

      if (onSave != null)
      {
        onSave.DynamicInvoke(path);
      }

      Disable();
      Canvas.locked = false;
    }
  }
}