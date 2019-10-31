using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FebEngine;
using FebEngine.UI;
using FebEngine.Utility;
using FebEngine.Tiles;
using System.IO;

namespace FebGame.States
{
  internal class MapEditor : GameState
  {
    public Editor editor;

    private UITilePalette tilePalette;
    private UITextBox mapProperties;
    private UIBrushEditor brushEditor;

    private Tileset tileset;

    private Texture2D rectTexture;
    private SpriteFont font;

    public override void Load(ContentManager content)
    {
      rectTexture = content.Load<Texture2D>("recthandles");
      font = content.Load<SpriteFont>("default");

      base.Load(content);
    }

    public override void Start()
    {
      canvas.bounds.Width = 1920;
      canvas.bounds.Height = 1080;
      canvas.bounds.Y = 30;

      LoadTileSet("tilesets/ts_test");

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

    public void ExportTilemap<T>(T path)
    {
      string document = TilemapXML.ExportMap(editor.mapGroup.CurrentMap);

      File.WriteAllText(path.ToString(), document);
    }

    public void ImportTilemap<T>(T document)
    {
      Tilemap importedTilemap = TilemapXML.ImportMap(document.ToString());

      editor.mapGroup.AddMap(importedTilemap);
      editor.mapGroup.ChangeMap(importedTilemap.Name);
    }

    public void InitGUI()
    {
      tilePalette = canvas.AddElement("TilePalette", new UITilePalette(title: "Palette", isDraggable: true, isCloseable: false), 800, 30, 400, 400) as UITilePalette;
      tilePalette.SetTileSet(tileset);

      var saveDialog = canvas.AddElement("FileSave", new UISaveDialog("atm", onSave: ExportTilemap), startInvisible: true);
      var loadDialog = canvas.AddElement("FileLoad", new UILoadDialog("atm", onLoad: ImportTilemap), startInvisible: true);

      canvas.AddElement("SaveButton", new UIButton("Save...", onClick: saveDialog.Enable), 0, 30, 100, 30);
      canvas.AddElement("LoadButton", new UIButton("Load...", onClick: loadDialog.Enable), 100, 30, 100, 30);

      canvas.AddElement("SetTilesetButton", new UIButton("Set Tileset...", onClick: loadDialog.Enable), 200, 30, 200, 30);

      //mapProperties = canvas.AddElement("MapProperties", new UITextBox(), 1400, 30, 200, 200) as UITextBox;

      //brushEditor = canvas.AddElement("BrushEditor", new UIBrushEditor(title: "Brush", isDraggable: true, isCloseable: false), 1000, 300, 400, 200) as UIBrushEditor;
    }

    public override void Update(GameTime gameTime)
    {
      //brushEditor.SelectedBrush = tilePalette.selectedBrush;

      /*
      if (editor.mapGroup.CurrentMap != null)
      {
        mapProperties.SetMessage(
          "Map Name: " + editor.mapGroup.CurrentMap.Name,
          "Width: " + editor.mapGroup.CurrentMap.Width,
          "Height: " + editor.mapGroup.CurrentMap.Height,
          "Current Layer: " + editor.mapGroup.CurrentMap.GetLayer(0).Name
          );
      }
      */
      if (editor.mapGroup.CurrentMap != null)
      {
        var map = editor.mapGroup.CurrentMap;
      }
    }

    public override void Draw(RenderManager renderer, GameTime gameTime)
    {
      var sb = renderer.SpriteBatch;

      if (editor.mapGroup.CurrentMap != null)
      {
        var color = Color.White;
        var gridColor = new Color(0.1f, 0.1f, 0.1f);
        var warpColor = Color.Magenta;
        int offset = 10;

        var map = editor.mapGroup.CurrentMap;

        var Bounds = map.Bounds;

        for (int i = 0; i < map.Width; i++)
        {
          sb.Draw(rectTexture, new Rectangle(
            Bounds.Left - offset + i * map.TileWidth,
            Bounds.Top,
            20,
            Bounds.Height * map.TileWidth
            ), new Rectangle(0, 30, 20, 40), gridColor);
        }
        for (int i = 0; i < map.Height; i++)
        {
          sb.Draw(rectTexture, new Rectangle(
            Bounds.Left,
            Bounds.Top - offset + i * map.TileHeight,
            Bounds.Width * map.TileHeight,
            20
            ), new Rectangle(30, 0, 40, 20), gridColor);
        }

        //top
        sb.Draw(rectTexture, new Rectangle(Bounds.Left, Bounds.Top - offset, Bounds.Width * map.TileHeight, 20), new Rectangle(30, 0, 40, 20), color);

        //left
        sb.Draw(rectTexture, new Rectangle(Bounds.Left - offset, Bounds.Top, 20, Bounds.Height * map.TileWidth), new Rectangle(0, 30, 20, 40), color);

        //right
        sb.Draw(rectTexture, new Rectangle(Bounds.Right * map.TileWidth - offset, Bounds.Top, 20, Bounds.Height * map.TileWidth), new Rectangle(0, 30, 20, 40), color);

        //bottom
        sb.Draw(rectTexture, new Rectangle(Bounds.Left, Bounds.Bottom * map.TileHeight - offset, Bounds.Width * map.TileHeight, 20), new Rectangle(30, 0, 40, 20), color);

        foreach (var warp in map.sideWarps)
        {
          int rangeMin = (int)warp.RangeMin * map.TileWidth;
          int rangeMax = (int)warp.RangeMax * map.TileHeight;

          int size = Math.Abs(rangeMin - rangeMax);

          if (warp.Direction == WarpDirection.Left)
          {
            sb.Draw(rectTexture, new Rectangle(Bounds.Left - offset, Bounds.Top + rangeMin, 20, size), new Rectangle(0, 30, 20, 40), warpColor);
            sb.DrawString(font, warp.DestinationMapName, new Vector2(Bounds.Left, Bounds.Top + rangeMin), Color.White);
          }
          else if (warp.Direction == WarpDirection.Right)
          {
            sb.Draw(rectTexture, new Rectangle(Bounds.Right * map.TileWidth - offset, Bounds.Top + rangeMin, 20, size), new Rectangle(0, 30, 20, 40), warpColor);
            sb.DrawString(font, warp.DestinationMapName, new Vector2(Bounds.Right * map.TileWidth - offset, Bounds.Top + rangeMin), Color.White);
          }
          if (warp.Direction == WarpDirection.Up)
          {
            sb.Draw(rectTexture, new Rectangle(Bounds.Left + rangeMin, Bounds.Top - offset, size, 20), new Rectangle(30, 0, 40, 20), warpColor);
            sb.DrawString(font, warp.DestinationMapName, new Vector2(Bounds.Left + rangeMin, Bounds.Top - offset), Color.White);
          }
          else if (warp.Direction == WarpDirection.Down)
          {
            sb.Draw(rectTexture, new Rectangle(Bounds.Left + rangeMin, Bounds.Bottom * map.TileHeight - offset, size, 20), new Rectangle(30, 0, 40, 20), warpColor);
            sb.DrawString(font, warp.DestinationMapName, new Vector2(Bounds.Left + rangeMin, Bounds.Bottom * map.TileHeight - offset), Color.White);
          }
        }
      }
    }
  }
}