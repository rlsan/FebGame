using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FebEngine;
using FebEngine.UI;
using FebEngine.UI.Editor;
using FebEngine.Utility;
using FebEngine.Tiles;
using System.IO;

namespace FebGame.States
{
  internal class MapEditor : GameState
  {
    private UITilePalette tilePalette;
    private UITextBox tileProperties;
    private UITextBox mapProperties;
    private UITileMapSetList mapList;

    private Tileset tileSet;
    private MapGroup tileMapSet;

    private TilemapXML tilemapXML;

    private Vector2 prevCamPos;
    private bool camHasMoved;

    private int previousScroll;

    public override void Load(ContentManager content)
    {
    }

    public override void Start()
    {
      previousScroll = canvas.mouse.ScrollWheelValue;

      InitGUI();

      LoadTileSet("tileset");
      LoadTileMapSet();

      tilemapXML = new TilemapXML();
      tilemapXML.ts = tileSet;
    }

    public void LoadMap<T>(T map)
    {
      Console.WriteLine(map);
    }

    public void LoadTileSet(string tileSetName)
    {
      var tex = game.Content.Load<Texture2D>(tileSetName);

      tileSet = new Tileset(tex, 16, 16);
      tileSet.AddBrush(new RowBrush("Thing", tileSet.TileBrushes[0], tileSet.TileBrushes[1], tileSet.TileBrushes[0]));
      tilePalette.SetTileSet(tileSet);
    }

    public void LoadTileMapSet()
    {
      tileMapSet = new MapGroup();
    }

    public void AddTileMap()
    {
      var map = new Tilemap(tileSet, 32, 32);
      map.Name = "Map" + tileMapSet.tilemaps.Count;
      map.Tileset = tileSet;
      map.SetLayers("BG", "FG");

      tileMapSet.AddMap(map);

      tileMapSet.ChangeMap(map.Name);

      RefreshMapList();
    }

    public void ExportTilemap<T>(T path)
    {
      string document = tilemapXML.Export(tileMapSet.currentMap);

      File.WriteAllText(path.ToString(), document);
    }

    public void ImportTilemap<T>(T document)
    {
      Tilemap importedTilemap = tilemapXML.Import(document.ToString());

      tileMapSet.AddMap(importedTilemap);
      tileMapSet.ChangeMap(importedTilemap.Name);

      RefreshMapList();
    }

    public void InitGUI()
    {
      tilePalette = canvas.AddElement("TilePalette", new UITilePalette(title: "Palette", isDraggable: true, isCloseable: false), 0, 30, 400, 800) as UITilePalette;

      var saveDialog = canvas.AddElement("FileSave", new UISaveDialog("atm", onSave: ExportTilemap), startInvisible: true);
      var loadDialog = canvas.AddElement("FileLoad", new UILoadDialog("atm", onLoad: ImportTilemap), startInvisible: true);

      canvas.AddElement("SaveButton", new UIButton("Save...", onClick: saveDialog.Enable), 0, 0, 100, 30);
      canvas.AddElement("LoadButton", new UIButton("Load...", onClick: loadDialog.Enable), 100, 0, 100, 30);

      canvas.AddElement("AddMapButton", new UIButton("Add Map", onClick: AddTileMap), 200, 0, 100, 30);

      tileProperties = canvas.AddElement("TileProperties", new UITextBox(), 1000, 0, 200, 200) as UITextBox;
      mapProperties = canvas.AddElement("MapProperties", new UITextBox(), 1400, 0, 200, 200) as UITextBox;

      mapList = canvas.AddElement("MapList", new UITileMapSetList(title: "Maps", isDraggable: true, isCloseable: false), 300, 30, 160, 270) as UITileMapSetList;
    }

    public void RefreshMapList()
    {
      string[] names = new string[tileMapSet.tilemaps.Count];

      for (int i = 0; i < tileMapSet.tilemaps.Count; i++)
      {
        names[i] = tileMapSet.tilemaps[i].Name;
      }

      mapList.Refresh(names);
    }

    public override void Update(GameTime gameTime)
    {
      tileProperties.SetMessage(
        "Selected Tile: " + tilePalette.selectedTile.id
        );

      if (tileMapSet.currentMap != null)
      {
        mapProperties.SetMessage(
          "Map Name: " + tileMapSet.currentMap.Name,
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
          prevCamPos = (world.camera.Position - canvas.mouse.Position.ToVector2()) * 2;
          camHasMoved = true;
        }

        world.camera.Position = (canvas.mouse.Position.ToVector2() * 2 + prevCamPos);
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