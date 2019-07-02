using FebEngine;
using FebEngine.UI;
using FebEngine.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using FebEngine.Utility;

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

    //tilemap vars

    private Tileset tileset;
    private Texture2D tilesetTexture;
    private List<TileBrushSwatch> swatches = new List<TileBrushSwatch>();
    private Tile selectedTile = new Tile();
    private TilemapXML tilemapXML;
    private int selectedLayerIndex = 1;
    private TilemapLayer selectedLayer;

    //chapter vars

    private TilemapSet tilemapSet = new TilemapSet();
    private List<TilemapThumbnail> tilemapThumbnails = new List<TilemapThumbnail>();
    private TilemapThumbnail selectedThumbnail;
    private Tilemap activeTilemap;

    private int scale = 8;
    private Point savedMousePosition;

    private enum LevelEditorView
    {
      None,
      Tilemap,
      Chapter
    }

    private LevelEditorView levelEditorView = LevelEditorView.Tilemap;

    public override void Load(ContentManager content)
    {
      canvas = new UICanvas(content.Load<Texture2D>("theme"));
      tilesetTexture = content.Load<Texture2D>("tileset");
      tileset = new Tileset(tilesetTexture, 16, 16);
      //tilemapXML = new TilemapXML(tilemap, tileset);

      var breakableBlock = tileset.AddTile(new RandomTile(false,
        tileset.GetTileFromIndex(24),
        tileset.GetTileFromIndex(25),
        tileset.GetTileFromIndex(26)
        ),
        "BreakableBlock");

      var logEnd = tileset.AddTile(new RandomTile(true,
        tileset.GetTileFromIndex(37),
        tileset.GetTileFromIndex(38)
        ),
        "LogEnd");

      var logMossMiddle = tileset.AddTile(new RandomTile(true,
        tileset.GetTileFromIndex(45),
        tileset.GetTileFromIndex(46),
        tileset.GetTileFromIndex(47)
        ),
        "LogMossMiddle");
      var logMoss = tileset.AddTile(new RowTile(false,
        tileset.GetTileFromIndex(44),
        logMossMiddle,
        tileset.GetTileFromIndex(48)
        ),
        "LogMoss");
      var logMiddle = tileset.AddTile(new RandomTile(true,
        tileset.GetTileFromIndex(34),
        tileset.GetTileFromIndex(35),
        tileset.GetTileFromIndex(36)
        ),
        "LogMiddle");
      var ladder = tileset.AddTile(new RandomTile(false,
        tileset.GetTileFromIndex(39),
        tileset.GetTileFromIndex(49),
        tileset.GetTileFromIndex(59)
        ),
        "Ladder");
      var ladderTop = tileset.AddTile(new ColumnTile(false,
        tileset.GetTileFromIndex(29),
        ladder,
        ladder
        ),
        "LadderTop");
      var poleMiddle = tileset.AddTile(new RandomTile(true,
        tileset.GetTileFromIndex(17),
        tileset.GetTileFromIndex(27)
        ),
        "PoleMiddle");
      var pole = tileset.AddTile(new ColumnTile(false,
        tileset.GetTileFromIndex(7),
        poleMiddle,
        poleMiddle
        ),
        "Pole");

      var animated = tileset.AddTile(new AnimatedTile(false, 12, true,
        ladder,
        logMiddle,
        breakableBlock,
        logMoss
        ),
        "Animated");

      var log = tileset.AddTile(new RowTile(false,

        tileset.GetTileFromIndex(33),
        logMiddle,
        logEnd
        ),
        "Log");

      var breakableLog = tileset.AddTile(new RowTile(false,

        tileset.GetTileFromIndex(33),
        logMiddle,
        logEnd
        ),
        "BreakableLog");

      int size = 16;

      int unitsX = tilesetTexture.Width / size;
      int unitsY = tilesetTexture.Height / size;

      int paletteWidth = 16;

      for (int i = 0; i < tileset.Tiles; i++)
      {
        var brush = tileset.TilePalette[i];
        swatches.Add(new TileBrushSwatch(brush, new Vector2((i % paletteWidth) * size, i / paletteWidth * size), size));
      }
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
        tilemap.SetLayers("BG", "FG", "D");
        tilemap.tileset = tileset;

        //tilemap.AddWarp(5, 5);

        tilemapSet.Add(tilemap);
        tilemapThumbnails.Add(new TilemapThumbnail(tilemap, x, y, width, height));
      }

      activeTilemap = tilemapSet.tilemaps[0];

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

      if (levelEditorView == LevelEditorView.Tilemap)
      {
        UpdateTilemapView();
      }
      if (levelEditorView == LevelEditorView.Chapter)
      {
        UpdateChapterView();
      }

      UpdateUI();
    }

    private void UpdateTilemapView()
    {
      MouseState mouse = Mouse.GetState();
      KeyboardState keyboard = Keyboard.GetState();
      Vector2 mousePosition = mouse.Position.ToVector2();

      selectedLayerIndex = selectedLayerIndex % activeTilemap.LayerCount;

      selectedLayer = activeTilemap.GetLayer(selectedLayerIndex);

      if (keyboard.IsKeyDown(Keys.D1))
      {
        selectedLayerIndex = 0;
      }
      if (keyboard.IsKeyDown(Keys.D2))
      {
        selectedLayerIndex = 1;
      }
      if (keyboard.IsKeyDown(Keys.D3))
      {
        selectedLayerIndex = 2;
      }

      bool hoverSwatches = false;

      for (int i = 0; i < swatches.Count; i++)
      {
        var swatch = swatches[i];

        if (swatch.Hover)
        {
          hoverSwatches = true;
          //hoveredTileString = swatch.tile.id.ToString();
          //selectedTilePropertiesString = swatch.tile.PropertiesToString;
        }

        if (swatch.Pressed)
        {
          selectedTile = swatches[i].tile;
          //selectedTileString = selectedTile.id.ToString();
        }
      }

      if (!hoverSwatches)
      {
        int posX = (int)Math.Round((mousePosition.X - 8) / 16);
        int posY = (int)Math.Round((mousePosition.Y - 8) / 16);

        if (mouse.LeftButton == ButtonState.Pressed)
        {
          if (keyboard.IsKeyDown(Keys.E))
          {
            selectedLayer.EraseTile(posX, posY);
          }
          else
          {
            selectedLayer.PutTile(selectedTile, posX, posY);
          }
        }
        if (mouse.RightButton == ButtonState.Pressed)
        {
          var tile = selectedLayer.GetTileXY(posX, posY);

          selectedTile = tile;
          //selectedTileString = selectedTile.id.ToString();
        }

        var hoveredTile = selectedLayer.GetTile(mousePosition);
        //hoveredTileString = hoveredTile.id.ToString();
      }

      if (keyboard.IsKeyDown(Keys.D))
      {
        selectedLayer.ShowTileIndices();
      }
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
              activeTilemap = thumbnail.parentTilemap;
            }
          }
        }
      }
    }

    private void InitUI()
    {
      canvas.AddElement("Header", new UIContainer());
      canvas.GetElement("Header").Add("a", new UITextButton("Tilemap View", 0, 0, 300, 20));
      canvas.GetElement("Header").Add("a", new UITextButton("Chapter View", 300, 0, 300, 20));

      canvas.AddElement("TilemapView", new UIContainer());
      canvas.GetElement("TilemapView").Add("a", new UITextWindow("Map Info", 0, 20, 200, 200, true));
      canvas.GetElement("TilemapView").Add("a", new UITextWindow("Tile Info", 200, 20, 200, 200, true));
      canvas.GetElement("TilemapView").Add("a", new UITextWindow("Shortcuts", 400, 20, 250, 200, true,
        "Left M - Paint",
        "Right M - Dropper",
        "E - Erase",
        "K - Clear",
        "D - Show indices",
        "F - Show properties",
        "1-3 - Layers"
        ));

      canvas.AddElement("ChapterView", new UIContainer());
      canvas.GetElement("ChapterView").Add("a", new UITextWindow("Set Info", 0, 20, 200, 200, true));
    }

    private void UpdateUI()
    {
      MouseState mouse = Mouse.GetState();
      KeyboardState keyboard = Keyboard.GetState();

      //tilemap view

      UITextWindow tilemapInfo = canvas.GetElement("TilemapView").childrenElements[0] as UITextWindow;

      tilemapInfo.SetLines(
        "Name: " + activeTilemap.name,
        "Coords: " + ((int)mouse.Position.X / 16).ToString() + ", " + ((int)mouse.Position.Y / 16).ToString(),
        "Layer: " + selectedLayerIndex + " (" + selectedLayer.Name + ")",
        "Current: " + 0
        );

      UITextWindow tileInfo = canvas.GetElement("TilemapView").childrenElements[1] as UITextWindow;

      tileInfo.SetLines(
        "Selected: " + selectedTile.id,
        "Name: " + selectedTile.Name,
        "Type: " + 0,
        "Properties: " + 0
        );

      //chapter view

      UITextWindow setInfo = canvas.GetElement("ChapterView").childrenElements[0] as UITextWindow;

      string activeTilemapName = "";

      if (activeTilemap != null)
      {
        activeTilemapName = activeTilemap.name;
      }

      setInfo.SetLines(
        "Selected: " + activeTilemapName
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

    public override void Draw(RenderManager renderer, GameTime gameTime)
    {
      var sb = renderer.SpriteBatch;

      renderer.GraphicsDevice.Clear(Color.CornflowerBlue);

      renderer.SpriteBatch.Begin();

      canvas.DrawElements(sb);

      if (levelEditorView == LevelEditorView.Tilemap)
      {
        // Draw tilemap
        activeTilemap.Draw(sb, gameTime);

        // Draw brush swatch palette
        for (int i = 0; i < swatches.Count; i++)
        {
          var tile = tileset.TilePalette[i];
          int frame = tile.ReturnFrame(activeTilemap.GetLayer(1), 0, 0);
          Vector2 framePosition = tileset.GetTilePositionFromIndex(frame);
          int x = (int)framePosition.X;
          int y = (int)framePosition.Y;

          renderer.SpriteBatch.Draw(
              tileset.Texture,
              swatches[i].bounds,
              new Rectangle(x * 16, y * 16, 16, 16),
              Color.White
              );
        }
      }

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

          //Debug.Text(thumbnail.parentTilemap.Warps.Count, 20, 20);

          foreach (var warp in thumbnail.parentTilemap.Warps)
          {
            //Debug.DrawLine(new Vector2(thumbnail.bounds.X * scale + warp.tileX * scale, thumbnail.bounds.Y * scale + warp.tileY * scale), Vector2.Zero);
          }
        }
      }

      Debug.Draw(sb);

      renderer.SpriteBatch.End();
    }
  }
}