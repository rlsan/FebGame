using FebEngine;
using FebEngine.Tiles;
using FebEngine.GUI;
using FebGame.Editor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace FebGame.States
{
  public class GroupEditor : GameState
  {
    public MainEditor editor;

    private int GridSize { get; set; } = 12;

    internal GroupEditorTool selectedTool = GroupEditorTool.Select;

    private List<MapThumbnail> thumbnails = new List<MapThumbnail>();
    internal MapThumbnail selectedThumbnail;

    private RectTransform rectTransform;
    private AnchorPoint selectedAnchor;

    private bool drawing;
    private bool resizing;

    private Rectangle drawingRect;
    private Texture2D drawingTex;
    private Texture2D rectTexture;
    private SpriteFont font;

    private GUIGroupView groupView;

    public override void Load(ContentManager content)
    {
      rectTransform = new RectTransform(content.Load<Texture2D>("recthandles"));
      drawingTex = content.Load<Texture2D>("missing");
      font = content.Load<SpriteFont>("default");
      rectTexture = content.Load<Texture2D>("recthandles");

      base.Load(content);
    }

    public override void Start()
    {
      canvas.bounds.Width = 1920;
      canvas.bounds.Height = 1080;

      groupView = canvas.AddElement(new GUIGroupView(this), 0, 30, 1920, 1080 - 30) as GUIGroupView;
      groupView.anchorPosition = AnchorPosition.Bottom;
    }

    public void LoadGroupThumbs()
    {
      foreach (var t in thumbnails)
      {
        t.Destroy();
      }

      thumbnails.Clear();

      foreach (var map in editor.mapGroup.Tilemaps)
      {
        var scaledRect = new Rectangle(map.X * GridSize, map.Y * GridSize, map.Width * GridSize, map.Height * GridSize);

        var m = Create.Entity(new MapThumbnail(scaledRect, map, GridSize), map.name + "_thumb") as MapThumbnail;
        m.font = font;
        m.texture = rectTexture;
        m.tilemap = map;

        thumbnails.Add(m);
      }

      Compile();
    }

    internal void Compile()
    {
      MapCompiler.Compile(editor.mapGroup, thumbnails);
    }

    internal void NewMapGroup()
    {
      if (editor.mapGroup == null)
      {
        editor.mapGroup = editor.Create.MapGroup("NewGroup");
      }
      else
      {
        editor.mapGroup.Reset();
      }

      LoadGroupThumbs();
    }

    internal void SaveGroup()
    {
      string groupFile = GroupIO.Export(editor.mapGroup);
      canvas.OpenSavePrompt(groupFile, "amg", editor.mapGroup.name.ToString());
    }

    internal void LoadGroup(string file)
    {
      editor.mapGroup.Load(file);
      LoadGroupThumbs();
    }

    internal void EditMap()
    {
      if (selectedThumbnail != null)
      {
        Compile();

        editor.mapGroup.ChangeMap(selectedThumbnail.tilemap.name.ToString());
        editor.activeTilemap = selectedThumbnail.tilemap;

        editor.ActivateMapEditor();
      }
    }

    internal void RemoveMap()
    {
      if (selectedThumbnail != null)
      {
        var thumbToRemove = selectedThumbnail;

        thumbnails.Remove(thumbToRemove);
        thumbToRemove.Destroy();
        selectedThumbnail = null;

        Compile();
      }
    }

    internal void AddMap(Rectangle area)
    {
      var t = new Tilemap(area.Width / GridSize, area.Height / GridSize, 64, 64);
      t.name = new StringBuilder("m" + thumbnails.Count);

      t.AddLayer("Background");
      t.AddLayer("Foreground");
      t.AddLayer("Detail");

      t.Tileset = editor.tileset;

      var m = Create.Entity(new MapThumbnail(area, t, GridSize), t.name + "_thumb") as MapThumbnail;
      m.font = font;
      m.texture = rectTexture;
      m.tilemap = t;

      m.RefreshTilemap();

      thumbnails.Add(m);
    }

    public override void Update(GameTime gameTime)
    {
      var mpos = world.camera.ToWorld(canvas.mouse.Position.ToVector2());

      if (canvas.keyboard.IsKeyDown(Keys.A)) selectedTool = GroupEditorTool.Select;
      else if (canvas.keyboard.IsKeyDown(Keys.Q)) selectedTool = GroupEditorTool.Transform;
      else if (canvas.keyboard.IsKeyDown(Keys.D)) selectedTool = GroupEditorTool.Draw;

      if (editor.panning) return;

      if (selectedTool == GroupEditorTool.Select)
      {
        if (canvas.DoubleMousePress)
        {
          if (selectedThumbnail != null && selectedThumbnail.Bounds.Contains(mpos))
          {
            EditMap();
          }
        }
        if (canvas.MousePress)
        {
          bool selectedSomething = false;
          foreach (var thumbnail in thumbnails)
          {
            if (thumbnail.Bounds.Contains(mpos.ToPoint()))
            {
              selectedThumbnail = thumbnail;
              rectTransform.SetHandles(thumbnail.Bounds);
              selectedSomething = true;

              break;
            }
          }
          if (!selectedSomething)
          {
            //selectedThumbnail = null;
          }
        }
      }

      if (selectedTool == GroupEditorTool.Transform)
      {
        if (resizing)
        {
          if (selectedThumbnail != null)
          {
            selectedThumbnail.Transform(selectedAnchor, mpos);
            rectTransform.SetHandles(selectedThumbnail.Bounds);
          }

          if (canvas.MouseUp)
          {
            resizing = false;
            Compile();
          }
        }
        else
        {
          if (canvas.MousePress)
          {
            var m = rectTransform.TestInput(mpos, 10);
            if (m != null)
            {
              selectedAnchor = m.AnchorPoint;
              resizing = true;
            }
            else
            {
              bool selectedSomething = false;
              foreach (var thumbnail in thumbnails)
              {
                if (thumbnail.Bounds.Contains(mpos.ToPoint()))
                {
                  selectedThumbnail = thumbnail;
                  rectTransform.SetHandles(thumbnail.Bounds);
                  selectedSomething = true;

                  break;
                }
              }
              if (!selectedSomething)
              {
                selectedThumbnail = null;
              }
            }
          }
        }
      }

      if (selectedTool == GroupEditorTool.Draw)
      {
        selectedThumbnail = null;
        if (canvas.MousePress)
        {
          if (!drawing)
          {
            drawing = true;

            drawingRect.X = Mathf.FloorToGrid(mpos.X, GridSize);
            drawingRect.Y = Mathf.FloorToGrid(mpos.Y, GridSize);
          }
        }

        if (canvas.MouseDown)
        {
          if (drawing)
          {
            drawingRect.Width = Mathf.RoundToGrid(mpos.X - drawingRect.X, GridSize);
            drawingRect.Height = Mathf.RoundToGrid(mpos.Y - drawingRect.Y, GridSize);
          }
        }
        else
        {
          if (drawing)
          {
            if (drawingRect.Width / GridSize > 0 && drawingRect.Height / GridSize > 0)
            {
              AddMap(drawingRect);
            }

            drawingRect = Rectangle.Empty;

            drawing = false;

            Compile();
          }
        }
      }
    }

    public override void Draw(RenderManager renderer, GameTime gameTime)
    {
      var mpos = world.camera.ToWorld(canvas.mouse.Position.ToVector2());

      renderer.SpriteBatch.Draw(drawingTex, drawingRect, Color.White);

      if (selectedThumbnail != null)
      {
        var pos = world.camera.ToScreen(selectedThumbnail.Position);
        var rect = new Rectangle(pos.ToPoint(), new Point(selectedThumbnail.Bounds.Width * GridSize, selectedThumbnail.Bounds.Height * GridSize));

        rectTransform.Draw(renderer.SpriteBatch);
      }

      base.Draw(renderer, gameTime);
    }

    internal enum GroupEditorTool
    {
      Select, Transform, Draw
    }
  }
}