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
    private UITextBox mapProperties;
    private UITileMapSetList mapList;
    private UIBrushEditor brushEditor;

    private Tileset tileset;
    private MapGroup tileMapSet;

    private Vector2 prevCamPos;
    private bool camHasMoved;

    private int previousScroll;

    public override void Start()
    {
      base.Start();

      previousScroll = canvas.mouse.ScrollWheelValue;

      LoadTileSet("tilesets/ts_test");
      LoadTileMapSet();

      InitGUI();

      TilemapXML.ExportTileset(tileset);
    }

    public void LoadMap<T>(T map)
    {
      Console.WriteLine(map);
    }

    public void LoadTileSet(string tileSetName)
    {
      var tex = game.Content.Load<Texture2D>(tileSetName);

      tileset = new Tileset(tex, 64, 64);

      TileBrush logEnd = tileset.AddBrush(new RandomBrush("LogEnd", tileset.GetBrushFromIndex(11), tileset.GetBrushFromIndex(12)), isHidden: true);

      TileBrush logMiddle = tileset.AddBrush(new RandomBrush("LogMiddle", tileset.GetBrushFromIndex(8), tileset.GetBrushFromIndex(9), tileset.GetBrushFromIndex(10)), isHidden: true);

      tileset.AddBrush(new RowBrush("Log", tileset.GetBrushFromIndex(7), logMiddle, logEnd, tileset.GetBrushFromIndex(6)));
    }

    public void LoadTileMapSet()
    {
      tileMapSet = Create.MapGroup("MapGroup");
    }

    public void AddTileMap()
    {
      var map = new Tilemap(32, 32, 64, 64);
      map.Name = "Map" + tileMapSet.tilemaps.Count;
      map.Tileset = tileset;
      map.SetLayers("BG", "FG");

      tileMapSet.AddMap(map);

      tileMapSet.ChangeMap(map.Name);

      RefreshMapList();
    }

    public void ExportTilemap<T>(T path)
    {
      string document = TilemapXML.ExportMap(tileMapSet.currentMap);

      File.WriteAllText(path.ToString(), document);
    }

    public void ImportTilemap<T>(T document)
    {
      Tilemap importedTilemap = TilemapXML.ImportMap(document.ToString());

      tileMapSet.AddMap(importedTilemap);
      tileMapSet.ChangeMap(importedTilemap.Name);

      RefreshMapList();
    }

    public void InitGUI()
    {
      tilePalette = canvas.AddElement("TilePalette", new UITilePalette(title: "Palette", isDraggable: true, isCloseable: false), 800, 30, 400, 400) as UITilePalette;
      tilePalette.SetTileSet(tileset);

      var saveDialog = canvas.AddElement("FileSave", new UISaveDialog("atm", onSave: ExportTilemap), startInvisible: true);
      var loadDialog = canvas.AddElement("FileLoad", new UILoadDialog("atm", onLoad: ImportTilemap), startInvisible: true);

      canvas.AddElement("SaveButton", new UIButton("Save...", onClick: saveDialog.Enable), 0, 30, 100, 30);
      canvas.AddElement("LoadButton", new UIButton("Load...", onClick: loadDialog.Enable), 100, 30, 100, 30);

      //canvas.AddElement("SaveTilesetButton", new UIButton("Save Tileset...", onClick: saveDialog.Enable), s0, 60, s0, 30);
      canvas.AddElement("SetTilesetButton", new UIButton("Set Tileset...", onClick: loadDialog.Enable), 200, 30, 200, 30);

      //canvas.AddElement("AddMapButton", new UIButton("Add Map", onClick: AddTileMap), s0, 30, 100, 30);

      mapProperties = canvas.AddElement("MapProperties", new UITextBox(), 1400, 30, 200, 200) as UITextBox;

      //mapList = canvas.AddElement("MapList", new UITileMapSetList(title: "Maps", isDraggable: true, isCloseable: false), 300, 30, 160, 270) as UITileMapSetList;

      brushEditor = canvas.AddElement("BrushEditor", new UIBrushEditor(title: "Brush", isDraggable: true, isCloseable: false), 1000, 300, 400, 200) as UIBrushEditor;
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
      brushEditor.SelectedBrush = tilePalette.selectedBrush;

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

            tileMapSet.currentMap.GetLayer(0).PutTile(tilePalette.selectedBrush, pos);
          }
        }
      }

      if (canvas.keyboard.IsKeyDown(Keys.Space) && canvas.mouse.LeftButton == ButtonState.Pressed)
      {
        if (!camHasMoved)
        {
          prevCamPos = (world.camera.Position - canvas.mouse.Position.ToVector2()) * 1;
          camHasMoved = true;
        }

        world.camera.Position = (canvas.mouse.Position.ToVector2() * 1 + prevCamPos);
      }
      else
      {
        camHasMoved = false;
      }

      if (canvas.mouse.ScrollWheelValue < previousScroll && world.camera.scaleFactor > 0.001f)
      {
        world.camera.scaleFactor--;
      }
      else if (canvas.mouse.ScrollWheelValue > previousScroll && world.camera.scaleFactor < 4)
      {
        world.camera.scaleFactor++;
      }
      previousScroll = canvas.mouse.ScrollWheelValue;
    }
  }
}