using FebEngine.Tilemaps;
using FebEngine.Tiles;
using FebEngine.UI;
using Microsoft.Xna.Framework;
using System;

namespace FebGame.Editor
{
  public class TileEditorWindow : UIContainer
  {
    public Tileset tileset;

    private UITilePalette tileBrowser;
    private UITilePalette tilePicker;
    public TileList brushView;

    public TileBrush defaultTile;

    public TileBrush selectedTile;
    public TileBrush selectedTileBrush;

    public UITextField nameField;

    public override void Init()
    {
      Action a;

      //defaultTile = tileset.Brushes[0];
      division = Division.horizontal;

      var menu = AddPanel(0.5f);
      menu.division = Division.vertical;

      menu.AddElement(new TilesetInfo(this), 1.5f);
      menu.AddText("Menu", 0.2f);

      menu.AddButton("Load Tileset...", onClick: LoadTileset);
      a = () => { TilesetIO.Export(tileset); };
      menu.AddButton("Save Tileset...", onClick: a);
      a = () => { tileset.AddBrush(defaultTile); };
      menu.AddButton("Add Tile", onClick: a);
      a = () => { tileset.AddBrush(new RowBrush("Row", defaultTile, defaultTile, defaultTile, defaultTile)); };
      menu.AddButton("Add Row", onClick: a);
      a = () => { tileset.AddBrush(new ColumnBrush("Column", defaultTile, defaultTile, defaultTile, defaultTile)); };
      menu.AddButton("Add Column", onClick: a);
      a = () => { tileset.AddBrush(new RandomBrush("Random", defaultTile)); };
      menu.AddButton("Add Random", onClick: a);
      a = () => { tileset.AddBrush(new AnimatedBrush(12, false, defaultTile)); };
      menu.AddButton("Add Animated", onClick: a);
      menu.AddButton("Remove Tile", onClick: RemoveTile);

      var tilesetView = AddPanel(1.2f);
      tilesetView.division = Division.vertical;

      tilesetView.AddText("Tileset", 0.1f);
      tileBrowser = (UITilePalette)tilesetView.AddElement(new UITilePalette(), 2f);
      tileBrowser.SetTileSet(tileset);
      tileBrowser.hideBaseTiles = true;

      tilesetView.AddText("Tile Picker", 0.12f);
      tilePicker = (UITilePalette)tilesetView.AddElement(new UITilePalette());
      tilePicker.SetTileSet(tileset);

      var tileInfo = AddPanel();
      tileInfo.division = Division.vertical;

      tileInfo.AddElement(new TileInfo(this), 0.8f);

      var tileOptions = tileInfo.AddPanel(0.1f);
      tileOptions.division = Division.horizontal;
      tileOptions.AddButton("Set", onClick: SetSubtile);
      tileOptions.AddButton("Add", onClick: AddSubtile);
      tileOptions.AddButton("Edit", onClick: EditTile);
      tileOptions.AddButton("Rename", onClick: RenameTile);
      tileOptions.AddButton("Remove", onClick: RemoveSubtile);

      nameField = tileInfo.AddTextField("", 0.08f);

      brushView = (TileList)tileInfo.AddElement(new TileList(this), 1.5f);

      tileInfo.AddText("", 0.7f).SetMessage(
        "H - Toggle Hidden",
        "1 - Set None",
        "2 - Set Solid",
        "3 - Set One-way",
        "4 - Set Ladder",
        "5 - Set Ladder-top"
        );

      base.Init();
    }

    private void LoadTileset()
    {
      tileset = TilesetIO.Import("Tileset.ats");

      tileBrowser.SetTileSet(tileset);
      tilePicker.SetTileSet(tileset);
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
      selectedTile.Name = nameField.text;
    }

    private void RemoveSubtile()
    {
      if (selectedTile.Inputs.Count <= 1) return;

      if (selectedTile.brushType == TileBrushType.Random || selectedTile.brushType == TileBrushType.Animated)
      {
        selectedTile.Inputs.Remove(brushView.selectedTile);
      }
    }

    private void EditTile()
    {
      tileBrowser.selectedBrush = brushView.selectedTile;
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
      selectedTile = tileBrowser.selectedBrush;

      if (tileset != null)
      {
        defaultTile = tileset.GetBrushFromIndex(0);
      }

      base.Update(gameTime);
    }
  }
}