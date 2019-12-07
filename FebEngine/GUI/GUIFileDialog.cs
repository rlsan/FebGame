using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FebEngine.GUI
{
  public class GUIFileDialog : GUIWindow
  {
    public string currentDir;
    public GUIWindow window;
    private GUIText directoryBox;
    private GUIScrollWindow<string> fileBox;

    public GUIContainer options;
    public GUITextField nameField;

    public override void Init()
    {
      base.Init();

      title = "File Browser";

      var navBar = main.AddPanel(1, 30, ScalingType.percentage, ScalingType.absolute);
      navBar.division = Division.Horizontal;

      navBar.AddButton("<", NavBack, 30, 30, ScalingType.absolute, ScalingType.absolute);

      currentDir = @"C:\Users\Public\Test";
      directoryBox = navBar.AddText(currentDir);
      directoryBox.alignment = TextAlignment.TopLeft;
      fileBox = main.AddChild("ScrollWindow", new GUIScrollWindow<string>(20, NavTo), 5, 55, 600, 400) as GUIScrollWindow<string>;
      fileBox.percent = 1.9f;

      options = main.AddPanel();
      options.division = Division.Horizontal;

      nameField = options.AddTextField();

      Refresh();
    }

    public override void OnKey(Keys key)
    {
      if (key == Keys.Escape)
      {
        Disable();
        Canvas.locked = false;
      }
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

    public void Refresh()
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

      directoryBox.SetMessage(currentDir.Replace(Directory.GetCurrentDirectory(), ""));
      fileBox.SetItems(directoriesAndFiles);
    }
  }
}