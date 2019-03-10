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
    private List<Tile> tiles;

    private Tileset tileset;
    private Tilemap tilemap;

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
      selectedTile = new Tile();
      tiles = new List<Tile>();
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

      base.Initialize();
    }

    protected override void LoadContent()
    {
      spriteBatch = new SpriteBatch(GraphicsDevice);

      tilesetTexture = Content.Load<Texture2D>("tileset");
      selector = Content.Load<Texture2D>("selector");

      tileset = new Tileset(tilesetTexture, 16, 16);
      //tileset.AddBrush(new ColumnTile());
      var logMiddle = tileset.AddTile(new RandomTile(
        tileset.GetTileFromIndex(34),
        tileset.GetTileFromIndex(35),
        tileset.GetTileFromIndex(36)
        ));
      var logEnd = tileset.AddTile(new RandomTile(
        tileset.GetTileFromIndex(37),
        tileset.GetTileFromIndex(38)
        ));

      var log = tileset.AddTile(new RowTile(

        tileset.GetTileFromIndex(33),
        logMiddle,
        logEnd
        ));

      //tileset.AddBrush(new int[] { 0, 1, 2 }, TileType.Breakable);

      Debug.fontTexture = Content.Load<Texture2D>("debug1");

      int size = 16;

      int unitsX = tilesetTexture.Width / size;
      int unitsY = tilesetTexture.Height / size;

      int paletteWidth = 16;

      for (int i = 0; i < tileset.Tiles; i++)
      {
        var brush = tileset.TilePalette[i];
        swatches.Add(new TileBrushSwatch(brush, new Vector2((i % paletteWidth) * size, i / paletteWidth * size), size));
      }

      tilemap.tileset = tileset;
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
          hoveredTilePropertiesString = swatch.tile.PropertiesToString;
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
        hoveredTilePropertiesString = hoveredTile.PropertiesToString;
      }

      if (keyboard.IsKeyDown(Keys.D))
      {
        selectedLayer.ShowTileIndices();
        selectedLayer.IsVisible = false;
      }
      else
      {
        selectedLayer.IsVisible = true;
      }
      if (keyboard.IsKeyDown(Keys.K))
      {
        selectedLayer.Clear();
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

      selectedTileString = selectedTile.id.ToString();

      //textField.Update(gameTime, keyboard);

      int offsetX = 260;
      int offsetY = 4;

      string[] displayText = new string[]{
        "Name: " + textField.text,
        "Coords: " + ((int)mousePosition.X/16).ToString() + ", " + ((int)mousePosition.Y/16).ToString(),
        "Layer: " + selectedLayerIndex + " (" + selectedLayer.Name + ")",
        "",
        "Current: " + hoveredTileString,
        "Type: " + selectedTile.Name,
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

      // Draw tilemap layers
      tilemap.Draw(spriteBatch);

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
        int frame = tile.ReturnFirstFrame();
        Vector2 framePosition = tileset.GetTilePositionFromIndex(frame);
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