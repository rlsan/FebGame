using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FebEngine;
using FebEngine.UI;
using FebEngine.Utility;
using FebEngine.Tiles;

namespace FebGame.States
{
  internal class MapEditor : GameState
  {
    private UITilePalette tilePalette;
    private UITextBox tileProperties;
    private UITextBox mapProperties;

    private TileSet tileSet;
    private TilemapSet tileMapSet;

    private Vector2 prevCamPos;
    private bool camHasMoved;

    private int previousScroll;

    public override void Load(ContentManager content)
    {
    }

    public override void Start()
    {
      previousScroll = canvas.mouse.ScrollWheelValue;

      tilePalette = canvas.AddElement("TilePalette", new UITilePalette(title: "Palette", isDraggable: true, isCloseable: false), 0, 30, 400, 800) as UITilePalette;

      var saveDialog = canvas.AddElement("FileSave", new UISaveDialog("txt"), startInvisible: true);
      var loadDialog = canvas.AddElement("FileLoad", new UILoadDialog("txt", onLoad: LoadMap), startInvisible: true);

      canvas.AddElement("SaveButton", new UIButton("Save...", onClick: saveDialog.Enable), 0, 0, 100, 30);
      canvas.AddElement("LoadButton", new UIButton("Load...", onClick: loadDialog.Enable), 100, 0, 100, 30);

      canvas.AddElement("AddMapButton", new UIButton("Add Map", onClick: AddTileMap), 200, 0, 100, 30);

      tileProperties = canvas.AddElement("TileProperties", new UITextBox(), 1000, 0, 200, 200) as UITextBox;
      mapProperties = canvas.AddElement("MapProperties", new UITextBox(), 1400, 0, 200, 200) as UITextBox;

      LoadTileSet("tileset");
      LoadTileMapSet();
    }

    public void LoadMap<T>(T map)
    {
      Console.WriteLine(map);
    }

    public void LoadTileSet(string tileSetName)
    {
      var tex = game.Content.Load<Texture2D>(tileSetName);

      tileSet = new TileSet(tex, 16, 16);
      tilePalette.SetTileSet(tileSet);
    }

    public void LoadTileMapSet()
    {
      tileMapSet = new TilemapSet();
    }

    public void AddTileMap()
    {
      var map = new Tilemap(32, 32, 16, 16);
      map.name = "Map" + tileMapSet.tilemaps.Count;
      map.tileset = tileSet;
      map.SetLayers("BG", "FG");

      tileMapSet.AddMap(map);

      tileMapSet.ChangeMap(map.name);
    }

    public override void Update(GameTime gameTime)
    {
      tileProperties.SetMessage(
        "Selected Tile: " + tilePalette.selectedTile.id
        );

      if (tileMapSet.currentMap != null)
      {
        mapProperties.SetMessage(
          "Map Name: " + tileMapSet.currentMap.name,
          "Current Layer: " + tileMapSet.currentMap.GetLayer(0).Name
          );
      }
      if (tileMapSet.currentMap != null)
      {
        if (canvas.mouse.LeftButton == ButtonState.Pressed && canvas.keyboard.IsKeyUp(Keys.Space))
        {
          if (canvas.activeElement == null)
          {
            var pos = world.camera.WorldToScreenTransform(canvas.mouse.Position.ToVector2());

            //var ha = tileMapSet.currentMap.GetLayer(0).GetTileXY((int)pos.X, (int)pos.Y);

            //Console.WriteLine(ha.frame);

            tileMapSet.currentMap.GetLayer(0).PutTile(tilePalette.selectedTile,
              (int)pos.X,
              (int)pos.Y
              );
          }
        }
      }

      if (canvas.keyboard.IsKeyDown(Keys.Space) && canvas.mouse.LeftButton == ButtonState.Pressed)
      {
        if (!camHasMoved)
        {
          prevCamPos = world.camera.Transform.Position - canvas.mouse.Position.ToVector2() * 2;
          camHasMoved = true;
        }

        world.camera.Transform.Position = canvas.mouse.Position.ToVector2() * 2 + prevCamPos;
      }
      else
      {
        camHasMoved = false;
      }

      if (canvas.mouse.ScrollWheelValue < previousScroll && world.camera.scaleFactor > 0.5f)
      {
        world.camera.scaleFactor /= 2;
      }
      else if (canvas.mouse.ScrollWheelValue > previousScroll && world.camera.scaleFactor < 16)
      {
        world.camera.scaleFactor *= 2;
      }
      previousScroll = canvas.mouse.ScrollWheelValue;

      if (canvas.keyboard.IsKeyDown(Keys.Down))
      {
      }
    }

    public override void Draw(RenderManager renderer, GameTime gameTime)
    {
      if (tileMapSet.currentMap != null)
      {
        tileMapSet.currentMap.Draw(renderer.SpriteBatch, gameTime);
      }
    }
  }
}