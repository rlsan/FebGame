using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;

namespace FebEngine.UI
{
  public class UIFileDialog : UIContainer
  {
    public string currentDir;
    public UIWindow window;
    private UITextBox directoryBox;
    private UIScrollWindow fileBox;
    public UITextField nameField;

    public override void Init()
    {
      window = AddChild("File Browser", new UIWindow(title: "File Browser", isDraggable: true), 0, 0, 640, 500) as UIWindow;

      window.AddChild("Back", new UIButton(title: "<", onClick: NavBack), 5, 30, 25, 25);

      //currentDir = Directory.GetCurrentDirectory();
      currentDir = @"C:\Users\Public\Test";

      directoryBox = window.AddChild("PathText", new UITextBox(currentDir), 35, 30, 600, 25) as UITextBox;
      fileBox = window.AddChild("ScrollWindow", new UIScrollWindow(20, NavTo), 5, 55, 600, 400) as UIScrollWindow;

      Refresh();

      nameField = window.AddChild("NameField", new UITextField(), 5, 460, 550, 25) as UITextField;

      base.Init();
    }

    public void NavTo(string item)
    {
      var path = currentDir + item;

      if (Directory.Exists(path))
      {
        currentDir = path;
        Refresh();
      }
      else
      {
        string file = Path.GetFileNameWithoutExtension(currentDir + "\\" + item);
        nameField.text = file;
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