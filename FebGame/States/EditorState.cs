using FebEngine;
using FebEngine.UI;
using FebEngine.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace FebGame.States
{
  internal class EditorState : GameState
  {
    private class TilemapThumbnail
    {
      public Tilemap parentTilemap;
      public Rectangle bounds;

      public TilemapThumbnail(Tilemap parent, int x, int y, int width, int height)
      {
        parentTilemap = parent;
        bounds = new Rectangle(x, y, width, height);
      }
    }

    private UICanvas canvas;
    private TilemapSet tilemapSet = new TilemapSet();
    private List<TilemapThumbnail> tilemapThumbnails = new List<TilemapThumbnail>();
    private TilemapThumbnail selectedThumbnail;

    private int scale = 8;
    private Point savedMousePosition;

    private enum LevelEditorView
    {
      None,
      Tilemap,
      Chapter
    }

    private LevelEditorView levelEditorView = LevelEditorView.Chapter;

    public override void Load(ContentManager content)
    {
      canvas = new UICanvas(content.Load<Texture2D>("theme"));
    }

    public override void Unload(ContentManager content)
    {
      content.Unload();
    }

    public override void Start()
    {
      // Generate some random tilemaps for testing
      for (int i = 0; i < 12; i++)
      {
        int x = 10 + 20 * i;
        int y = 50;
        int width = RNG.RandIntRange(10, 60);
        int height = RNG.RandIntRange(10, 60);

        var tilemap = new Tilemap(width, height, 16, 16);
        tilemap.name = "Map" + i;

        tilemap.AddWarp(5, 5);

        tilemapSet.Add(tilemap);
        tilemapThumbnails.Add(new TilemapThumbnail(tilemap, x, y, width, height));
      }

      InitUI();
    }

    public override void Update(GameTime gameTime)
    {
      UIButton tilemapButton = canvas.GetElement("Header").childrenElements[0] as UIButton;
      UIButton chapterButton = canvas.GetElement("Header").childrenElements[1] as UIButton;

      if (tilemapButton.Pressed)
      {
        levelEditorView = LevelEditorView.Tilemap;
      }
      if (chapterButton.Pressed)
      {
        levelEditorView = LevelEditorView.Chapter;
      }

      if (levelEditorView == LevelEditorView.Chapter)
      {
        UpdateChapterView();
      }

      UpdateUI();
    }

    private void UpdateChapterView()
    {
      MouseState mouse = Mouse.GetState();
      KeyboardState keyboard = Keyboard.GetState();

      Point scaledMousePosition = new Point(mouse.Position.X / scale, mouse.Position.Y / scale);

      if (keyboard.IsKeyDown(Keys.Down))
      {
        if (scale > 1) scale--;
      }
      if (keyboard.IsKeyDown(Keys.Up))
      {
        if (scale < 20) scale++;
      }

      if (selectedThumbnail != null)
      {
        selectedThumbnail.bounds.Location = scaledMousePosition - savedMousePosition;

        if (mouse.LeftButton == ButtonState.Released)
        {
          selectedThumbnail = null;
        }
      }

      foreach (var thumbnail in tilemapThumbnails)
      {
        if (selectedThumbnail == null)
        {
          if (thumbnail.bounds.Contains(scaledMousePosition))
          {
            if (mouse.LeftButton == ButtonState.Pressed)
            {
              selectedThumbnail = thumbnail;
              savedMousePosition = (scaledMousePosition - thumbnail.bounds.Location);
            }
          }
        }
      }
    }

    private void InitUI()
    {
      canvas.AddElement("Header", new UIContainer());
      canvas.GetElement("Header").Add(new UITextButton("Tilemap View", 0, 0, 300, 20));
      canvas.GetElement("Header").Add(new UITextButton("Chapter View", 300, 0, 300, 20));

      canvas.AddElement("TilemapView", new UIContainer());
      canvas.GetElement("TilemapView").Add(new UITextWindow("Map Info", 0, 20, 200, 200, true));
      canvas.GetElement("TilemapView").Add(new UITextWindow("Tile Info", 200, 20, 200, 200, true));
      canvas.GetElement("TilemapView").Add(new UITextWindow("Shortcuts", 400, 20, 250, 200, true,
        "Left M - Paint",
        "Right M - Dropper",
        "E - Erase",
        "K - Clear",
        "D - Show indices",
        "F - Show properties",
        "1-3 - Layers"
        ));

      canvas.AddElement("ChapterView", new UIContainer());
    }

    private void UpdateUI()
    {
      MouseState mouse = Mouse.GetState();
      KeyboardState keyboard = Keyboard.GetState();

      UITextWindow tilemapInfo = canvas.GetElement("TilemapView").childrenElements[0] as UITextWindow;

      tilemapInfo.SetLines(
        "Name: " + 0,
        "Coords: " + ((int)mouse.Position.X / 16).ToString() + ", " + ((int)mouse.Position.Y / 16).ToString(),
        "Layer: " + 0 + " (" + "null" + ")",
        "Current: " + 0
        );

      UITextWindow tileInfo = canvas.GetElement("TilemapView").childrenElements[1] as UITextWindow;

      tileInfo.SetLines(
        "Selected: " + 0,
        "Name: " + 0,
        "Type: " + 0,
        "Properties: " + 0
        );

      if (levelEditorView == LevelEditorView.Tilemap)
      {
        canvas.GetElement("TilemapView").isVisible = true;
        canvas.GetElement("ChapterView").isVisible = false;
      }
      if (levelEditorView == LevelEditorView.Chapter)
      {
        canvas.GetElement("TilemapView").isVisible = false;
        canvas.GetElement("ChapterView").isVisible = true;
      }
    }

    public override void Draw(RenderManager renderer)
    {
      /*
      var sb = renderer.SpriteBatch;

      renderer.GraphicsDevice.Clear(Color.CornflowerBlue);

      renderer.SpriteBatch.Begin();

      canvas.DrawElements(sb);
      //tilemapSet.Draw(sb);

      if (levelEditorView == LevelEditorView.Chapter)
      {
        foreach (var thumbnail in tilemapThumbnails)
        {
          Debug.Text(thumbnail.parentTilemap.name, thumbnail.bounds.X * scale, thumbnail.bounds.Y * scale);
          Debug.DrawRect(new Rectangle(
            thumbnail.bounds.X * scale,
            thumbnail.bounds.Y * scale,
            thumbnail.bounds.Width * scale,
            thumbnail.bounds.Height * scale
            ),
            Color.Black);

          Debug.Text(thumbnail.parentTilemap.Warps.Count, 20, 20);

          foreach (var warp in thumbnail.parentTilemap.Warps)
          {
            Debug.DrawLine(new Vector2(thumbnail.bounds.X * scale + warp.tileX * scale, thumbnail.bounds.Y * scale + warp.tileY * scale), Vector2.Zero);
          }
        }
      }

      Debug.Draw(sb);

      renderer.SpriteBatch.End();
      */
    }
  }
}