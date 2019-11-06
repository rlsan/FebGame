using FebEngine;
using FebEngine.Tiles;
using FebEngine.UI;
using FebEngine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace FebGame.States
{
  internal partial class GroupEditor : GameState
  {
    public EditorState editor;

    private int GridSize { get; set; } = 12;

    private GroupEditorTool selectedTool = GroupEditorTool.Draw;
    private UITextBox selectedToolUI;
    private MapInfoPanel mapInfoPanel;

    private List<MapThumbnail> thumbnails = new List<MapThumbnail>();
    private MapThumbnail selectedThumbnail;

    private RectTransform rectTransform;
    private AnchorPoint selectedAnchor;

    private bool drawing;
    private bool resizing;

    private Rectangle drawingRect;
    private Texture2D drawingTex;
    private Texture2D rectTexture;
    private SpriteFont font;

    private TileBrush grohBrush;

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
      canvas.bounds.Y = 30;

      grohBrush = new TileBrush("groh", 0);

      canvas.AddElement("Shortcuts", new UITextBox(
        "Q - Select",
        "D - Draw",
        "Space - Pan"
        ), 1920 - 300, 0, 0, 0);
      selectedToolUI = canvas.AddElement("SelectedTool", new UITextBox(""), 1920 - 300, 200, 0, 0) as UITextBox;

      mapInfoPanel = canvas.AddElement("MapInfo", new MapInfoPanel()) as MapInfoPanel;
      canvas.AddElement("ImportMap", new UIButton("Import Map..."), 0, 30, 150, 30);
      canvas.AddElement("RemoveMap", new UIButton("Remove Map", onClick: RemoveMap), 0, 60, 150, 30);
      canvas.AddElement("Compile", new UIButton("Compile", onClick: Compile), 0, 90, 150, 30);
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
        //var t = new MapThumbnail(map.Bounds, map, GridSize);

        var scaledRect = new Rectangle(map.X * GridSize, map.Y * GridSize, map.Width * GridSize, map.Height * GridSize);

        var m = Create.Entity(new MapThumbnail(scaledRect, map, GridSize), map.Name + "_thumb") as MapThumbnail;
        m.font = font;
        m.texture = rectTexture;
        m.tilemap = map;

        thumbnails.Add(m);
      }

      Compile();
    }

    private void Compile()
    {
      if (editor.mapGroup != null)
      {
        MapCompiler.Compile(editor.mapGroup, thumbnails);
      }
    }

    private void EditMap()
    {
      if (editor.mapGroup != null && selectedThumbnail != null)
      {
        Compile();

        editor.mapGroup.ChangeMap(selectedThumbnail.tilemap.Name);
        editor.activeTilemap = selectedThumbnail.tilemap;

        editor.ActivateMapEditor();
      }
    }

    private void RemoveMap()
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

    private void AddMap(Rectangle area)
    {
      var t = new Tilemap(area.Width / GridSize, area.Height / GridSize, 64, 64);
      t.Name = "m" + thumbnails.Count;

      t.AddLayer("Background");
      t.AddLayer("Foreground");
      t.AddLayer("Detail");

      var m = Create.Entity(new MapThumbnail(area, t, GridSize), t.Name + "_thumb") as MapThumbnail;
      m.font = font;
      m.texture = rectTexture;
      m.tilemap = t;

      m.RefreshTilemap();

      thumbnails.Add(m);
    }

    public override void Update(GameTime gameTime)
    {
      var mpos = world.camera.ScreenToWorldTransform(canvas.mouse.Position.ToVector2());

      if (canvas.keyboard.IsKeyDown(Keys.A)) selectedTool = GroupEditorTool.Select;
      else if (canvas.keyboard.IsKeyDown(Keys.Q)) selectedTool = GroupEditorTool.Transform;
      else if (canvas.keyboard.IsKeyDown(Keys.D)) selectedTool = GroupEditorTool.Draw;

      selectedToolUI.SetMessage(selectedTool);

      if (selectedThumbnail != null)
      {
        selectedThumbnail.tilemap.Name = mapInfoPanel.mapNameField.text;
      }

      if (editor.panning) return;

      if (selectedTool == GroupEditorTool.Select)
      {
        if (canvas.DoubleMousePress)
        {
          EditMap();
        }
        if (canvas.MousePress)
        {
          //var m = rectTransform.TestInput(mpos, 10);
          //if (m != null)
          //{
          //  selectedAnchor = m.AnchorPoint;
          //  resizing = true;
          //}
          //else
          //{
          bool selectedSomething = false;
          foreach (var thumbnail in thumbnails)
          {
            if (thumbnail.Bounds.Contains(mpos.ToPoint()))
            {
              selectedThumbnail = thumbnail;
              rectTransform.SetHandles(thumbnail.Bounds);
              selectedSomething = true;
              mapInfoPanel.mapNameField.SetMessage(selectedThumbnail.tilemap.Name);

              break;
            }
          }
          if (!selectedSomething)
          {
            selectedThumbnail = null;
          }
          //}
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
                  mapInfoPanel.mapNameField.SetMessage(selectedThumbnail.tilemap.Name);

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
      var mpos = world.camera.ScreenToWorldTransform(canvas.mouse.Position.ToVector2());

      renderer.SpriteBatch.Draw(drawingTex, drawingRect, Color.White);

      if (selectedThumbnail != null)
      {
        var pos = world.camera.WorldToScreenTransform(selectedThumbnail.Position);
        var rect = new Rectangle(pos.ToPoint(), new Point(selectedThumbnail.Bounds.Width * GridSize, selectedThumbnail.Bounds.Height * GridSize));

        rectTransform.Draw(renderer.SpriteBatch);
      }
    }

    private enum GroupEditorTool
    {
      Select, Transform, Draw
    }
  }
}