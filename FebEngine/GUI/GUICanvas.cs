using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebEngine.GUI
{
  public class GUICanvas : Entity
  {
    private List<GUIElement> elements;

    public Texture2D Theme { get; set; }
    public SpriteFont Font { get; set; }

    public ContentManager contentManager;

    public Rectangle bounds;

    public MouseState mouse;
    public KeyboardState keyboard;

    public bool MousePress { get; set; }
    public bool MouseRelease { get; set; }
    public bool DoubleMousePress { get; set; }
    public bool MouseDown { get; set; }
    public bool MouseUp { get; set; }

    public bool KeyboardAccept { get; set; }
    public bool KeyboardCancel { get; set; }

    public bool KeyDown { get; set; }
    public bool KeyUp { get; set; }
    public bool KeyPress { get; set; }

    public GUIElement activeElement;

    private bool hasClicked;
    private bool hasKey;
    private float doubleClickTimer = 0;
    private float doubleClickDelay = .3f;

    private GUIPrompt GlobalPrompt { get; }
    private GUITextPrompt TextPrompt { get; }
    private GUISaveDialog SavePrompt { get; }
    private GUILoadDialog LoadPrompt { get; }
    public bool locked = false;

    public GUICanvas(int width, int height)
    {
      elements = new List<GUIElement>();
      bounds = new Rectangle(0, 0, width, height);

      GlobalPrompt = AddElement(new GUIPrompt(), 0, 0, 400, 300) as GUIPrompt;
      GlobalPrompt.Disable();
      TextPrompt = AddElement(new GUITextPrompt(), 600, 600, 400, 200) as GUITextPrompt;
      TextPrompt.Disable();
      SavePrompt = AddElement(new GUISaveDialog(), 0, 0, 600, 600) as GUISaveDialog;
      SavePrompt.Disable();
      LoadPrompt = AddElement(new GUILoadDialog(), 0, 0, 600, 600) as GUILoadDialog;
      LoadPrompt.Disable();
    }

    public void OpenSavePrompt<T>(T export, string extension, string text = "")
    {
      locked = true;
      SavePrompt.Enable();
      SetActiveElement(SavePrompt);

      SavePrompt.Refresh();

      SavePrompt.extension = extension;
      SavePrompt.nameField.SetMessage(text);

      SavePrompt.fileToSave = export;
    }

    public void OpenLoadPrompt(Action<string> action, string extension)
    {
      locked = true;
      LoadPrompt.Enable();
      SetActiveElement(LoadPrompt);

      LoadPrompt.Refresh();

      LoadPrompt.extension = extension;
      LoadPrompt.onLoad = action;
    }

    public void ShowPrompt(string title, string message, Action action = null)
    {
      locked = true;

      GlobalPrompt.Enable();
      SetActiveElement(GlobalPrompt);
      GlobalPrompt.action = action;
      GlobalPrompt.Message = message;
      GlobalPrompt.title = title;
    }

    public void RenamePrompt(ref StringBuilder stringRef)
    {
      locked = true;

      TextPrompt.Enable();
      SetActiveElement(TextPrompt);
      SetActiveElement(TextPrompt.textField);

      TextPrompt.SetString(ref stringRef);
    }

    public void AddChild(GUIElement e)
    {
      elements.Add(e);
    }

    public GUIElement AddElement(GUIElement element, int x = 0, int y = 0, int width = 0, int height = 0, bool startInvisible = false)
    {
      GUIElement e = element;

      e.offsetX = x;
      e.offsetY = y;
      e.bounds = new Rectangle(x, y, width, height);

      e.Canvas = this;

      elements.Add(e);

      e.Init();

      if (startInvisible)
      {
        e.Disable();
      }

      return e;
    }

    public void SetActiveElement(GUIElement elem)
    {
      activeElement = elem;
      BringToTop(elem);
    }

    public void HandleMouse()
    {
      mouse = Mouse.GetState();

      MouseDown = mouse.LeftButton == ButtonState.Pressed;
      MouseUp = !MouseDown;
      MousePress = false;
      MouseRelease = false;
      DoubleMousePress = false;

      doubleClickTimer -= Time.DeltaTime;

      if (MouseDown)
      {
        if (!hasClicked)
        {
          hasClicked = true;
          MousePress = true;
        }
      }
      else
      {
        if (hasClicked)
        {
          hasClicked = false;
          MouseRelease = true;
        }
      }

      if (MousePress)
      {
        if (doubleClickTimer > 0) DoubleMousePress = true;
        else doubleClickTimer = doubleClickDelay;
      }
    }

    private void HandleKeyboard()
    {
      keyboard = Keyboard.GetState();

      KeyDown = keyboard.GetPressedKeys().Length > 0;
      KeyUp = !KeyDown;
      KeyPress = false;

      if (KeyDown)
      {
        if (!hasKey)
        {
          hasKey = true;
          KeyPress = true;
        }
      }
      else
      {
        if (hasKey)
        {
          hasKey = false;
        }
      }

      KeyboardAccept = false;
      KeyboardCancel = false;

      if (keyboard.IsKeyDown(Keys.Enter))
      {
        KeyboardAccept = true;
      }
      if (keyboard.IsKeyDown(Keys.Escape))
      {
        KeyboardCancel = true;
      }
    }

    public bool IsKeyDown(Keys key)
    {
      return Keyboard.GetState().IsKeyDown(key);
    }

    public override void Update(GameTime gameTime)
    {
      HandleMouse();
      HandleKeyboard();

      var list = elements.ToList();
      list.Reverse();

      bool nothingSelected = true;

      foreach (var element in list)
      {
        element.isPressed = false;
        element.isHolding = false;
        element.isReleased = false;

        if (element.bounds.Contains(mouse.Position) && element.isVisible)
        {
          if (MouseDown)
          {
            element.isHolding = true;
            element.OnHold();
          }

          if (MouseRelease)
          {
            element.isReleased = true;
            element.OnRelease();
          }

          if (MousePress)
          {
            element.isPressed = true;
            element.OnPress(mouse.Position);
            SetActiveElement(element);

            nothingSelected = false;

            break;
          }
        }
      }
      foreach (var element in list)
      {
        if (element.isVisible)
        {
          if (KeyPress)
          {
            element.OnKey(keyboard.GetPressedKeys()[0]);
          }

          element.Update(gameTime);
        }
      }

      if (MousePress && nothingSelected) activeElement = null;
    }

    public void BringToTop(GUIElement element)
    {
      if (elements.Contains(element))
      {
        elements.Remove(element);
        elements.Add(element);
      }
    }

    public void Clear()
    {
      elements.Clear();
    }

    public override void Draw(SpriteBatch sb, GameTime gameTime)
    {
      foreach (var element in elements)
      {
        if (element.isVisible)
        {
          element.Draw(sb);
        }
      }

      if (activeElement != null)
      {
        //Debug.DrawRect(activeElement.bounds);
      }
    }
  }
}