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
    public MapGroup mapGroup;

    private int GridSize { get; set; } = 12;

    private GroupEditorTool selectedTool = GroupEditorTool.Draw;
    private UITextBox selectedToolUI;
    private MapInfoPanel mapInfoPanel;

    private List<MapThumbnail> thumbnails = new List<MapThumbnail>();
    private MapThumbnail selectedThumbnail;

    private RectTransform rectTransform;
    private AnchorPoint selectedAnchor;

    private Vector2 prevCamPos;
    private bool camHasMoved;

    private bool drawing;
    private bool resizing;

    private Rectangle drawingRect;
    private Texture2D drawingTex;
    private Texture2D rectTexture;
    private SpriteFont font;

    private int previousScroll;

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
      canvas.AddElement("Shortcuts", new UITextBox(
        "Q - Select",
        "D - Draw",
        "Space - Pan"
        ), 1920 - 300, 0, 0, 0);
      selectedToolUI = canvas.AddElement("SelectedTool", new UITextBox(""), 1920 - 300, 200, 0, 0) as UITextBox;

      mapInfoPanel = canvas.AddElement("MapInfo", new MapInfoPanel()) as MapInfoPanel;

      canvas.AddElement("NewGroup", new UIButton("New Group", onClick: CreateGroup), 0, 30, 150, 30);
      canvas.AddElement("LoadGroup", new UIButton("Load Group..."), 0, 60, 150, 30);
      canvas.AddElement("SaveGroup", new UIButton("Save Group..."), 0, 90, 150, 30);
      canvas.AddElement("ImportMap", new UIButton("Import Map..."), 0, 120, 150, 30);
      canvas.AddElement("RemoveMap", new UIButton("Remove Map", onClick: RemoveMap), 0, 150, 150, 30);
      canvas.AddElement("Compile", new UIButton("Compile", onClick: Compile), 0, 190, 150, 30);
    }

    private void Compile()
    {
      if (mapGroup != null)
      {
        Console.WriteLine("Starting compile...");

        mapGroup.tilemaps.Clear();

        foreach (var thumbnail in thumbnails)
        {
          thumbnail.tilemap.sideWarps.Clear();
          int connectedMaps = 0;
          foreach (var other in thumbnails)
          {
            if (other != thumbnail)
            {
              if (other.tilemap.Name == thumbnail.tilemap.Name)
              {
                Console.WriteLine("Identical names detected, aborting.");
                return;
              }
              if (other.Bounds.Intersects(thumbnail.Bounds))
              {
                Console.WriteLine("Maps are intersecting, aborting.");
                return;
              }

              Rectangle inflated = new Rectangle
              {
                Location = thumbnail.Bounds.Location,
                Size = thumbnail.Bounds.Size
              };

              inflated.Inflate(GridSize, GridSize);

              if (other.Bounds.Intersects(inflated))
              {
                connectedMaps++;
                //thumbnail.tilemap.sideWarps.Add(new SideWarp(other.tilemap.Name, WarpDirection.Down));
              }
            }
          }

          if (connectedMaps > 0)
          {
            Console.WriteLine(thumbnail.tilemap.Name + " is connected to: " + connectedMaps);
          }
          else
          {
            Console.WriteLine(thumbnail.tilemap.Name + " is orphaned.");
          }

          thumbnail.RefreshTilemap();

          mapGroup.AddMap(thumbnail.tilemap);
        }

        for (int i = 0; i < mapGroup.tilemaps.Count; i++)
        {
          Console.WriteLine("Added map: " + mapGroup.tilemaps[i].Name);
        }

        Console.WriteLine("Compiled successfully.");
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
      }
    }

    private void CreateGroup()
    {
      if (mapGroup == null)
      {
        mapGroup = new MapGroup();
      }
    }

    private void AddThumb(Rectangle area)
    {
      var t = new Tilemap(0, 0, 64, 64);
      t.Name = "m" + thumbnails.Count;

      var m = Create.Entity(new MapThumbnail(area, t, GridSize), t.Name + "_thumb") as MapThumbnail;
      m.font = font;
      m.texture = rectTexture;
      m.tilemap = t;

      thumbnails.Add(m);
    }

    public override void Update(GameTime gameTime)
    {
      bool panning = false;
      var mpos = world.camera.ScreenToWorldTransform(canvas.mouse.Position.ToVector2());

      if (canvas.keyboard.IsKeyDown(Keys.Q)) selectedTool = GroupEditorTool.Select;
      else if (canvas.keyboard.IsKeyDown(Keys.D)) selectedTool = GroupEditorTool.Draw;

      selectedToolUI.SetMessage(selectedTool);

      if (canvas.MouseDown)
      {
        if (canvas.keyboard.IsKeyDown(Keys.Space))
        {
          DragCamera();
          panning = true;
        }
      }
      else
      {
        camHasMoved = false;
      }

      if (panning) return;

      if (selectedThumbnail != null)
      {
        selectedThumbnail.tilemap.Name = mapInfoPanel.mapNameField.text;
      }

      if (selectedTool == GroupEditorTool.Select)
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
          }
        }
        else
        {
          if (canvas.MousePress)
          {
            var m = rectTransform.TestInput(mpos, 20);
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
              AddThumb(drawingRect);
            }

            drawingRect = Rectangle.Empty;

            drawing = false;
          }
        }
      }

      if (canvas.mouse.ScrollWheelValue < previousScroll && world.camera.scaleFactor > 0.5f)
      {
        world.camera.scaleFactor -= 0.1f;
      }
      else if (canvas.mouse.ScrollWheelValue > previousScroll && world.camera.scaleFactor < 2)
      {
        world.camera.scaleFactor += 0.1f;
      }
      previousScroll = canvas.mouse.ScrollWheelValue;
    }

    private void DragCamera()
    {
      if (!camHasMoved)
      {
        prevCamPos = world.camera.Position + canvas.mouse.Position.ToVector2() / world.camera.scaleFactor;
        camHasMoved = true;
      }

      world.camera.Position = -canvas.mouse.Position.ToVector2() / world.camera.scaleFactor + prevCamPos;
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
      Select, Draw
    }
  }
}