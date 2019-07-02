using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;

namespace FebEngine.UI
{
  public class UIFileDialog : UIContainer
  {
    private string currentDir;
    private UITextBox directoryBox;
    private UIScrollWindow fileBox;
    private UIPrompt savePrompt;

    public override void Init()
    {
      var window = AddChild("File Browser", new UIWindow(title: "File Browser", isDraggable: true), 0, 0, 640, 500);

      window.AddChild("Back", new UIButton(title: "<", onClick: NavBack), 5, 30, 25, 25);

      currentDir = Directory.GetCurrentDirectory();

      directoryBox = window.AddChild("PathText", new UITextBox(currentDir), 35, 30, 600, 25) as UITextBox;
      fileBox = window.AddChild("ScrollWindow", new UIScrollWindow(20, NavTo), 5, 55, 600, 400) as UIScrollWindow;

      Refresh();

      window.AddChild("SaveFile", new UITextField(), 5, 460, 550, 25);
      window.AddChild("SaveButton", new UIButton(title: "Save", onClick: Foo), 555, 460, 50, 25);

      //savePrompt = AddChild("Prompt", new UIPrompt("Warning", "This file exists. Overwrite?", new UIButton("Save over", Disable), new UIButton("Cancel", ClosePrompt))) as UIPrompt;

      base.Init();
    }

    public void Foo()
    {
      Canvas.ShowPrompt(title: "hello", message: "this is message");
    }

    public void ClosePrompt()
    {
      savePrompt.Disable();
    }

    public void NavTo(string directory)
    {
      var subdirectory = currentDir + directory;

      if (Directory.Exists(subdirectory))
      {
        currentDir = subdirectory;
        Refresh();
      }
    }

    public void NavBack()
    {
      var parent = Directory.GetParent(currentDir);

      if (parent != null)
      {
        currentDir = parent.ToString();
        Refresh();
      }
    }

    private void Refresh()
    {
      string[] subDirectories = Directory.GetDirectories(currentDir);
      string[] subDirectoryNames = new string[subDirectories.Length];

      for (int i = 0; i < subDirectoryNames.Length; i++)
      {
        subDirectoryNames[i] = "\\" + Path.GetFileName(subDirectories[i]);
      }

      string[] files = Directory.GetFiles(currentDir);
      string[] fileNames = new string[files.Length];

      for (int i = 0; i < fileNames.Length; i++)
      {
        fileNames[i] = Path.GetFileName(files[i]);
      }

      string[] directoriesAndFiles = subDirectoryNames.Concat(fileNames).ToArray();

      directoryBox.message = currentDir;
      fileBox.SetItems(directoriesAndFiles);
    }
  }
}