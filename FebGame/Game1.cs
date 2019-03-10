using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FebEngine;
using FebEngine.UI;
using FebGame.States;

namespace FebGame
{
  public class Game1 : Game
  {
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;

    private Texture2D tilesetTexture;
    private Texture2D selector;

    private List<TileBrushSwatch> swatches;
    private TileBrush selectedBrush;
    private List<Tile> tiles;

    private Tileset tileset;
    private Tilemap tilemap;

    private Rectangle paintableArea;

    private RenderTarget2D renderTarget;

    private const int VIRTUAL_WIDTH = 1400 / 2;
    private const int VIRTUAL_HEIGHT = 900 / 2;

    private TextField textField = new TextField("TILEMAP.ATM");

    private int selectedLayerIndex = 1;

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
      selectedBrush = new TileBrush { id = -1, frames = new int[] { -1 }, properties = new TileType[] { TileType.None } };
      tiles = new List<Tile>();
      tilemap = new Tilemap(40, 25, 16, 16);

      tilemap.AddLayer("Foreground");
      tilemap.AddLayer("Background");

      paintableArea = new Rectangle(200, 0, 600, 800);

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

      base.Initialize();
    }

    protected override void LoadContent()
    {
      spriteBatch = new SpriteBatch(GraphicsDevice);

      tilesetTexture = Content.Load<Texture2D>("tileset");
      selector = Content.Load<Texture2D>("selector");

      tileset = new Tileset(tilesetTexture, 16, 16);
      tileset.AddBrush(new int[] { 0, 1, 2 }, TileType.Breakable);

      Debug.fontTexture = Content.Load<Texture2D>("debug1");

      int size = 16;

      int unitsX = tilesetTexture.Width / size;
      int unitsY = tilesetTexture.Height / size;

      int paletteWidth = 16;

      for (int i = 0; i < tileset.Brushes; i++)
      {
        var brush = tileset.brushPalette[i];
        swatches.Add(new TileBrushSwatch(brush, new Vector2((i % paletteWidth) * size, i / paletteWidth * size), size));
      }

      tilemap.texture = tilesetTexture;
    }

    protected override void UnloadContent()
    {
    }

    protected override void Update(GameTime gameTime)
    {
      string hoveredTileString = "";
      string hoveredTilePropertiesString = "";
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

      selectedLayerIndex = selectedLayerIndex % tilemap.LayerCount;

      TilemapLayer selectedLayer = tilemap.GetLayer(selectedLayerIndex);

      bool hoverSwatches = false;

      for (int i = 0; i < swatches.Count; i++)
      {
        var swatch = swatches[i];

        if (swatch.Hover)
        {
          hoverSwatches = true;
          hoveredTileString = swatch.brush.id.ToString();
          hoveredTilePropertiesString = swatch.brush.PropertiesToString;
        }

        if (swatch.Pressed)
        {
          //this.selectedBrush.Clear();
          selectedBrush = swatches[i].brush;
          selectedTileString = selectedBrush.id.ToString();
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
            selectedLayer.PutTile(selectedBrush, posX, posY);
          }
        }
        if (mouse.RightButton == ButtonState.Pressed)
        {
          var tile = selectedLayer.GetTileXY(posX, posY);

          selectedBrush = new TileBrush { id = tile.id, frames = tile.frames, properties = tile.properties };
          selectedTileString = selectedBrush.id.ToString();
        }
        //}

        var hoveredTile = selectedLayer.GetTile(mousePosition);
        hoveredTileString = hoveredTile.id.ToString();
        hoveredTilePropertiesString = hoveredTile.PropertiesToString;
      }

      if (keyboard.IsKeyDown(Keys.D))
      {
        selectedLayer.ShowTileIndices();
      }
      if (keyboard.IsKeyDown(Keys.K))
      {
        selectedLayer.Clear();
      }

      selectedTileString = selectedBrush.id.ToString();

      //textField.Update(gameTime, keyboard);

      int offsetX = 260;
      int offsetY = 4;

      string[] displayText = new string[]{
        "Name: " + textField.text,
        "Coords: " + ((int)mousePosition.X/16).ToString() + ", " + ((int)mousePosition.Y/16).ToString(),
        "Layer: " + selectedLayerIndex + " (" + selectedLayer.Name + ")",
        "",
        "Current: " + hoveredTileString,
        "Properties: " + hoveredTilePropertiesString,
        "Selected: " + selectedTileString,
      };

      for (int i = 0; i < displayText.Length; i++)
      {
        Debug.Text(displayText[i], offsetX, offsetY + i * 10);
      }

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

      // Draw tilemap
      for (int layerID = 0; layerID < tilemap.LayerCount; layerID++)
      {
        var layer = tilemap.GetLayer(layerID);

        var color = Color.White;

        if (layerID != selectedLayerIndex)
        {
          color = new Color(Color.LightGray, 1f);
        }

        for (int i = 0; i < layer.tileArray.Length; i++)
        {
          int tileX = i % tilemap.width;
          int tileY = i / tilemap.width;
          var tile = layer.tileArray[tileX, tileY];

          int timeFrame = (int)(gameTime.TotalGameTime.TotalSeconds * 12) % tile.frames.Length;
          var frame = tile.frames[timeFrame];

          Vector2 framePosition = tileset.GetTileFromIndex(frame);
          int x = (int)framePosition.X;
          int y = (int)framePosition.Y;

          if (tile.id >= 0)
          {
            spriteBatch.Draw(
              tilemap.texture,
              new Rectangle(tileX * size, tileY * size, size, size),
              new Rectangle(x * size, y * size, size, size),
              color
              );
          }
        }
      }

      // Draw brush swatch palette
      for (int i = 0; i < swatches.Count; i++)
      {
        var brush = tileset.brushPalette[i];
        var frame = brush.frames[0];

        if (brush.IsAnimated)
        {
          int timeFrame = (int)(gameTime.TotalGameTime.TotalSeconds * 12) % brush.frames.Length;
          frame = brush.frames[timeFrame];
        }

        Vector2

          framePosition = tileset.GetTileFromIndex(frame);

        int x = (int)framePosition.X;
        int y = (int)framePosition.Y;

        spriteBatch.Draw(
            tileset.Texture,
            swatches[i].Bounds,
            new Rectangle(x * 16, y * 16, 16, 16),
            Color.White
            );
      }

      // Draw selected tile

      if (selectedBrush != null)
      {
        if (selectedBrush.id != -1)
        {
          for (int i = 0; i < selectedBrush.frames.Length; i++)
          {
            var frame = selectedBrush.frames[i];

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
        }
      }

      // Draw selector
      Vector2 mousePosition = Mouse.GetState().Position.ToVector2() / 2;
      int posX = (int)Math.Round((mousePosition.X - 8) / 16);
      int posY = (int)Math.Round((mousePosition.Y - 8) / 16);

      spriteBatch.Draw(selector, new Vector2(posX * size, posY * size), Color.White);

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