using FebEngine;
using FebEngine.Tiles;
using FebEngine.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace FebGame.Editor
{
  public class GUITileView : GUIContainer
  {
    public Tileset tileset;

    private GUITilePalette tileBrowser;
    private GUITilePalette tilePicker;
    public TileList brushView;

    public TileBrush defaultTile;

    public TileBrush selectedTile;
    public TileBrush selectedTileBrush;

    public TilesetInfo tilesetInfo;

    public GUITextField nameField;

    public GUITileView(Tileset tileset)
    {
      this.tileset = tileset;
    }

    public override void Init()
    {
      base.Init();

      Action a;

      anchorPosition = AnchorPosition.Center;

      //defaultTile = tileset.Brushes[0];
      division = Division.Horizontal;

      var menu = AddPanel(200, Height, ScalingType.absolute);
      menu.division = Division.Vertical;

      tilesetInfo = menu.AddElement(new TilesetInfo(), 2f) as TilesetInfo;
      //menu.AddText("Menu", 0.2f);

      menu.AddButton("New...", NewTileset);
      menu.AddButton("Save...", SaveTileset);
      a = () => { Canvas.OpenLoadPrompt(LoadTileset, "ats"); };
      menu.AddButton("Load...", a);

      menu.AddButton("Set Texture...");
      menu.AddButton("Rename...");

      var tilesetView = AddPanel(1200, Canvas.bounds.Height, ScalingType.absolute);
      tilesetView.division = Division.Vertical;

      tilesetView.drawBackground = true;

      tilesetView.AddBar("Brush Palette");

      var tileOptions = tilesetView.AddPanel(400, 40, ScalingType.percentage, ScalingType.absolute);
      tileOptions.division = Division.Horizontal;
      a = () => { tileset.AddBrush(new TileBrush("Tile", 0)); };
      tileOptions.AddButton("Tile", a);
      a = () => { tileset.AddBrush(new RowBrush("Row", defaultTile, defaultTile, defaultTile, defaultTile)); };
      tileOptions.AddButton("Row", a);
      a = () => { tileset.AddBrush(new ColumnBrush("Column", defaultTile, defaultTile, defaultTile, defaultTile)); };
      tileOptions.AddButton("Column", a);
      tileOptions.AddButton("Net", a);
      a = () => { tileset.AddBrush(new RandomBrush("Random", defaultTile)); };
      tileOptions.AddButton("Random", a);
      a = () => { tileset.AddBrush(new AnimatedBrush(12, false, defaultTile)); };
      tileOptions.AddButton("Animated", a);
      tileOptions.AddButton("Rename", RenameTile);
      tileOptions.AddButton("Remove", RemoveTile);

      var selectedBrushInfo = tilesetView.AddElement(new BrushInfo()) as BrushInfo;
      selectedBrushInfo.heightScalingType = ScalingType.absolute;
      selectedBrushInfo.realHeight = 100;

      tileBrowser = (GUITilePalette)tilesetView.AddElement(new GUITilePalette());
      tileBrowser.SetTileset(tileset);
      tileBrowser.hideBaseTiles = true;

      tileBrowser.heightScalingType = ScalingType.absolute;
      tileBrowser.realHeight = 500;

      a = () => { selectedBrushInfo.Brush = tileBrowser.selectedBrush; };
      tileBrowser.OnBrushSelect = a;

      tilesetView.AddBar("Brush Picker");
      var pickedBrushInfo = tilesetView.AddElement(new BrushInfo()) as BrushInfo;
      pickedBrushInfo.heightScalingType = ScalingType.absolute;
      pickedBrushInfo.realHeight = 100;

      tilePicker = (GUITilePalette)tilesetView.AddElement(new GUITilePalette());
      tilePicker.SetTileset(tileset);

      a = () => { pickedBrushInfo.Brush = tilePicker.selectedBrush; };
      tilePicker.OnBrushSelect = a;

      var tileInfo = AddPanel();
      tileInfo.division = Division.Vertical;
      tileInfo.drawBackground = true;

      tileInfo.AddBar("Subtile Editor");

      var subtileOptions = tileInfo.AddPanel(400, 40, ScalingType.percentage, ScalingType.absolute);
      subtileOptions.division = Division.Horizontal;
      subtileOptions.AddButton("Set", SetSubtile);
      subtileOptions.AddButton("Add", AddSubtile);

      a = () =>
      {
        tileBrowser.selectedBrush = brushView.selectedBrush;
        selectedBrushInfo.Brush = tileBrowser.selectedBrush;
      };

      subtileOptions.AddButton("Edit", a);
      subtileOptions.AddButton("Remove", RemoveSubtile);

      brushView = (TileList)tileInfo.AddElement(new TileList(this));

      a = () =>
      {
        tilePicker.selectedBrush = brushView.selectedBrush;
        pickedBrushInfo.Brush = brushView.selectedBrush;
      };
      brushView.OnBrushSelect = a;

      tileInfo.AddBar("Key Commands");
      var commands = tileInfo.AddText();
      commands.SetMessage(
        "Shortcuts:",
        "X - Remove Brush",
        "Visualization:",
        "I - Show/Hide Indexes",
        "P - Show/Hide Properties",
        "Properties:",
        "1 - Set None",
        "2 - Set Solid",
        "3 - Set One-way",
        "4 - Set Ladder",
        "5 - Set Ladder-top"
        );

      commands.alignment = TextAlignment.TopLeft;

      commands.Padding = 8;
    }

    private void SaveTileset()
    {
      if (tileset != null)
      {
        Canvas.OpenSavePrompt(TilesetIO.Export(tileset), "ats", tileset.name);
      }
    }

    private void NewTileset()
    {
      //tileset = new Tileset(texture, 64, 64);

      //tileBrowser.SetTileSet(tileset);
      //tilePicker.SetTileSet(tileset);
    }

    private void LoadTileset(string tilesetFile)
    {
      tileset = TilesetIO.Import(tilesetFile, RenderManager.Instance.Content);

      tileBrowser.SetTileset(tileset);
      tilePicker.SetTileset(tileset);
    }

    private void RemoveTile()
    {
      if (selectedTile.brushType != TileBrushType.Tile)
      {
        tileset.Brushes.Remove(selectedTile);
      }
    }

    private void RenameTile()
    {
      if (selectedTile != null)
      {
        var s = selectedTile.Name;
        Canvas.RenamePrompt(ref s);
      }
      //selectedTile.Name = nameField.text;
    }

    private void RemoveSubtile()
    {
      if (selectedTile.Inputs.Count <= 1) return;

      if (selectedTile.brushType == TileBrushType.Random || selectedTile.brushType == TileBrushType.Animated)
      {
        selectedTile.Inputs.Remove(brushView.selectedBrush);
      }
    }

    private void SetSubtile()
    {
      selectedTile.Inputs[brushView.selectedTileNumber] = tilePicker.selectedBrush;
    }

    private void AddSubtile()
    {
      if (selectedTile == null) return;

      if (selectedTile.brushType == TileBrushType.Random || selectedTile.brushType == TileBrushType.Animated)
      {
        selectedTile.Inputs.Insert(brushView.selectedTileNumber + 1, tilePicker.selectedBrush);
      }
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);

      selectedTile = tileBrowser.selectedBrush;

      if (tileset != null)
      {
        tilesetInfo.Tileset = tileset;
      }

      if (tileset != null)
      {
        defaultTile = tileset.GetBrushFromIndex(0);
      }
    }
  }
}