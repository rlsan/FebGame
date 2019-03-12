using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FebEngine;
using FebEngine.UI;
using FebEngine.Tiles;

namespace FebGame
{
  public class Game1 : Game
  {
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;

    private Texture2D tilesetTexture;
    private Texture2D selector;

    private List<TileBrushSwatch> swatches;
    private Tile selectedTile;

    private Tileset tileset;
    private Tilemap tilemap;

    private RenderTarget2D renderTarget;

    private const int VIRTUAL_WIDTH = 1400 / 2;
    private const int VIRTUAL_HEIGHT = 900 / 2;

    private TextField textField = new TextField("TestTilemap");

    private int selectedLayerIndex = 1;

    private UICanvas canvas;
    private TilemapXML tilemapXML;

    public Game1()
    {
      Window.Title = "Level Editor";

      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";

      graphics.PreferredBackBufferWidth = 1400;
      graphics.PreferredBackBufferHeight = 900;
    }

    protected override void Initialize()
    {
      swatches = new List<TileBrushSwatch>();
      selectedTile = new Tile();
      //tiles = new List<Tile>();
      tilemap = new Tilemap(40, 25, 16, 16);

      tilemap.SetLayers("BG", "FG", "D");

      tilemap.GetLayer(2).X = 8;
      tilemap.GetLayer(2).Y = 8;

      IsMouseVisible = true;

      PresentationParameters pp = GraphicsDevice.PresentationParameters;

      renderTarget = new RenderTarget2D(GraphicsDevice, VIRTUAL_WIDTH, VIRTUAL_HEIGHT, false,
      SurfaceFormat.Color, DepthFormat.None, pp.MultiSampleCount, RenderTargetUsage.DiscardContents);

      Texture2D pixelTexture = new Texture2D(GraphicsDevice, 4, 4);

      Color[] colorData = new Color[16];
      for (int i = 0; i < 16; i++)
      {
        colorData[i] = new Color(255, 255, 255, 1f);
      }
      pixelTexture.SetData(colorData);

      Debug.pixelTexture = pixelTexture;

      canvas = new UICanvas();
      canvas.AddElement("Palette", new TextWindow(true), new Rectangle(0, 0, 128, 300));
      canvas.AddElement("Tilemap Info", new TextWindow(false), new Rectangle(128, 0, 128, 300));
      canvas.AddElement("Tile Info", new TextWindow(false), new Rectangle(256, 0, 128, 300));
      canvas.AddElement("Shortcuts", new TextWindow(false,
        "Left M - Paint",
        "Right M - Dropper",
        "E - Erase",
        "K - Clear",
        "D - Show indices",
        "F - Show properties",
        "1-3 - Layers"
        ), new Rectangle(128 + 256, 0, 128, 300));

      canvas.AddElement("Save Tilemap", new TextButton(), new Rectangle(512, 0, 90, 30));
      canvas.AddElement("Save Tileset", new TextButton(), new Rectangle(512 + 90, 0, 90, 30));

      /*
      canvas.AddElement("New Tile", new TextButton(), new Rectangle(512 + 200, 0, 70, 30));
      canvas.AddElement("Solid", new TextButton(), new Rectangle(512 + 200, 30, 70, 30));
      canvas.AddElement("One Way Up", new TextButton(), new Rectangle(512 + 200, 60, 70, 30));
      canvas.AddElement("Ladder", new TextButton(), new Rectangle(512 + 200, 90, 70, 30));
      canvas.AddElement("Water", new TextButton(), new Rectangle(512 + 200, 120, 70, 30));
      canvas.AddElement("Breakable", new TextButton(), new Rectangle(512 + 200, 150, 70, 30));
      */

      tilemap.name = textField.text;

      //canvas.AddElement("Properties", new TextWindow(false,
      //  Enum.GetNames(typeof(TileType))
      //  ), new Rectangle(256 + 128, 0, 128, 300));

      base.Initialize();
    }

    protected override void LoadContent()
    {
      spriteBatch = new SpriteBatch(GraphicsDevice);

      tilesetTexture = Content.Load<Texture2D>("tileset");
      selector = Content.Load<Texture2D>("selector");
      canvas.ThemeTexture = Content.Load<Texture2D>("theme");

      tileset = new Tileset(tilesetTexture, 16, 16);
      tilemapXML = new TilemapXML(tilemap, tileset);
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

      Debug.fontTexture = Content.Load<Texture2D>("debug1");

      int size = 16;

      int unitsX = tilesetTexture.Width / size;
      int unitsY = tilesetTexture.Height / size;

      int paletteWidth = 8;

      for (int i = 0; i < tileset.Tiles; i++)
      {
        var brush = tileset.TilePalette[i];

        //if (!brush.hidden)
        //{
        swatches.Add(new TileBrushSwatch(brush, new Vector2((i % paletteWidth) * size, 16 + i / paletteWidth * size), size));
        //}
      }

      tilemap.tileset = tileset;
    }

    protected override void UnloadContent()
    {
    }

    protected override void Update(GameTime gameTime)
    {
      string hoveredTileString = "";
      string selectedTilePropertiesString = "";
      string selectedTileString = "";

      if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

      MouseState mouse = Mouse.GetState();
      KeyboardState keyboard = Keyboard.GetState();

      Vector2 mousePosition = mouse.Position.ToVector2() / 2;

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

      selectedLayerIndex = selectedLayerIndex % tilemap.LayerCount;

      TilemapLayer selectedLayer = tilemap.GetLayer(selectedLayerIndex);

      bool hoverSwatches = false;

      for (int i = 0; i < swatches.Count; i++)
      {
        var swatch = swatches[i];

        if (swatch.Hover)
        {
          hoverSwatches = true;
          hoveredTileString = swatch.tile.id.ToString();
          selectedTilePropertiesString = swatch.tile.PropertiesToString;
        }

        if (swatch.Pressed)
        {
          //this.selectedBrush.Clear();
          selectedTile = swatches[i].tile;
          selectedTileString = selectedTile.id.ToString();
        }
      }

      if (!hoverSwatches)
      {
        //if (this.selectedBrush.Count > 0)
        //{
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
          selectedTileString = selectedTile.id.ToString();
        }
        //}

        var hoveredTile = selectedLayer.GetTile(mousePosition);
        hoveredTileString = hoveredTile.id.ToString();
      }

      if (keyboard.IsKeyDown(Keys.D))
      {
        selectedLayer.ShowTileIndices();
        //selectedLayer.IsVisible = false;
      }
      else
      {
        //selectedLayer.IsVisible = true;
      }
      if (keyboard.IsKeyDown(Keys.K))
      {
        selectedLayer.Clear();
      }
      if (keyboard.IsKeyDown(Keys.F))
      {
        tilemap.showTileProperties = true;
      }
      else
      {
        tilemap.showTileProperties = false;
      }

      if (keyboard.IsKeyDown(Keys.W))
      {
        if (selectedTile.id != -1)
        {
          selectedTile.properties[0] = TileType.Breakable;
        }
      }
      if (keyboard.IsKeyDown(Keys.M))
      {
        if (selectedTile.id != -1)
        {
          selectedTile.properties[0] = TileType.Solid;
        }
      }

      foreach (var tile in selectedLayer.tileArray)
      {
        if (tile.id != -1)
        {
          if (selectedLayer.GetTileXY(tile.X, tile.Y - 1).id == -1)
          {
            //selectedLayer.PutTile(tileset.brushPalette[0], tile.X, tile.Y);
            //Debug.Text("X", tile.X * tilemap.tileWidth, tile.Y * tilemap.tileHeight);
          }
        }
      }

      foreach (var layer in tilemap.Layers)
      {
        if (layer != selectedLayer)
        {
          layer.tint = Color.LightGray;
        }
        else
        {
          layer.tint = Color.White;
        }
      }

      selectedTileString = selectedTile.id.ToString();
      selectedTilePropertiesString = selectedTile.PropertiesToString;

      //textField.Update(gameTime, keyboard);

      TextWindow tilemapInfo = canvas.GetElement("Tilemap Info") as TextWindow;

      tilemapInfo.SetLines(
        "Name: " + textField.text,
        "Coords: " + ((int)mousePosition.X / 16).ToString() + ", " + ((int)mousePosition.Y / 16).ToString(),
        "Layer: " + selectedLayerIndex + " (" + selectedLayer.Name + ")",
        "Current: " + hoveredTileString
        );

      TextWindow tileInfo = canvas.GetElement("Tile Info") as TextWindow;

      tileInfo.SetLines(
        "Selected: " + selectedTileString,
        "Name: " + selectedTile.Name,
        "Type: " + selectedTile.GetType().Name,
        "Properties: " + selectedTilePropertiesString
        );

      TextWindow palette = canvas.GetElement("Palette") as TextWindow;

      palette.bounds.Height = 16 * 10;

      Button saveButton = canvas.GetElement("Save Tilemap") as Button;

      if (saveButton.Click)
      {
        Console.Clear();
        tilemapXML.WriteTilemap();
      }

      Button saveTSButton = canvas.GetElement("Save Tileset") as Button;

      if (saveTSButton.Click)
      {
        Console.Clear();
        tilemapXML.WriteTileset();
      }

      Time.Update(gameTime);
      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.SetRenderTarget(renderTarget);

      GraphicsDevice.Clear(Color.CornflowerBlue);

      spriteBatch.Begin(samplerState: SamplerState.PointClamp);

      int size = 16;
      int unitsX = tilesetTexture.Width / size;
      int unitsY = tilesetTexture.Height / size;

      // Draw tilemap layers
      tilemap.Draw(spriteBatch);

      canvas.DrawElements(spriteBatch);

      // Draw brush swatch palette
      for (int i = 0; i < swatches.Count; i++)
      {
        var tile = tileset.TilePalette[i];
        //var frame = brush.frames[0];

        //if (brush.IsAnimated)
        //{
        //  int timeFrame = (int)(gameTime.TotalGameTime.TotalSeconds * 12) % brush.frames.Length;
        //  frame = brush.frames[timeFrame];
        //}
        int frame = tile.ReturnFrame(tilemap.GetLayer(1), 0, 0);
        Vector2 framePosition = tileset.GetTilePositionFromIndex(frame);
        int x = (int)framePosition.X;
        int y = (int)framePosition.Y;

        spriteBatch.Draw(
            tileset.Texture,
            swatches[i].bounds,
            new Rectangle(x * 16, y * 16, 16, 16),
            Color.White
            );
      }

      // Draw selected tile

      if (selectedTile != null)
      {
        if (selectedTile.id != -1)
        {
          /*
          for (int i = 0; i < selectedTile.frames.Length; i++)
          {
            var frame = selectedTile.frames[i];

            Vector2 framePosition = tileset.GetTileFromIndex(frame);

            int x = (int)framePosition.X;
            int y = (int)framePosition.Y;

            spriteBatch.Draw(
                tileset.Texture,
                new Rectangle(400 + i * 16, 0, 16, 16),
                new Rectangle(x * 16, y * 16, 16, 16),
                Color.White
                );
          }
          */
        }
      }

      // Draw selector
      Vector2 mousePosition = Mouse.GetState().Position.ToVector2() / 2;
      int posX = (int)Math.Round((mousePosition.X - 8) / 16);
      int posY = (int)Math.Round((mousePosition.Y - 8) / 16);

      if (canvas.GetElement("Palette").bounds.Contains(mousePosition))
      {
        spriteBatch.Draw(selector, new Vector2(posX * size, posY * size), Color.White);
      }

      Debug.Draw(spriteBatch);

      spriteBatch.End();

      ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

      float outputAspect = Window.ClientBounds.Width / (float)Window.ClientBounds.Height;
      float preferredAspect = VIRTUAL_WIDTH / (float)VIRTUAL_HEIGHT;

      Rectangle dst;

      if (outputAspect <= preferredAspect)
      {
        // output is taller than it is wider, bars on top/bottom
        int presentHeight = (int)((Window.ClientBounds.Width / preferredAspect) + 0.5f);
        int barHeight = (Window.ClientBounds.Height - presentHeight) / 2;

        dst = new Rectangle(0, barHeight, Window.ClientBounds.Width, presentHeight);
      }
      else
      {
        // output is wider than it is tall, bars left/right
        int presentWidth = (int)((Window.ClientBounds.Height * preferredAspect) + 0.5f);
        int barWidth = (Window.ClientBounds.Width - presentWidth) / 2;

        dst = new Rectangle(barWidth, 0, presentWidth, Window.ClientBounds.Height);
      }

      GraphicsDevice.SetRenderTarget(null);

      // clear to get black bars
      GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 1.0f, 0);

      spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
    SamplerState.PointClamp, DepthStencilState.Default,
    RasterizerState.CullNone);

      spriteBatch.Draw(renderTarget, dst, Color.White);

      spriteBatch.End();

      ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

      base.Draw(gameTime);
    }
  }
}