using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FebEngine.Tiles;
using FebGame.Editor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FebEngine.UI
{
  public class TileList : UIWindow
  {
    private TileEditorWindow tileEditor;
    public TileBrush selectedTile;
    public int selectedTileNumber;

    private int rows = 10;

    public TileList(TileEditorWindow tileEditor)
    {
      this.tileEditor = tileEditor;
    }

    public override void OnPress(Point mousePos)
    {
      var tileset = tileEditor.tileset;
      var brush = tileEditor.selectedTile;

      int idSelector = (mousePos.X - X) / tileset.TileWidth;

      if (brush != null)
      {
        if (idSelector < brush.Inputs.Count)
        {
          selectedTile = brush.Inputs[idSelector];
          selectedTileNumber = idSelector;
          //tileEditor.selectedTileBrush = brush.Inputs[idSelector];
        }
      }

      if (mousePos.Y > Y && mousePos.X < X + Width)
      {
        int swatchX = (mousePos.X - X) / (tileset.TileWidth);
        int swatchY = (mousePos.Y - Y) / (tileset.TileHeight);

        int id = swatchX + (swatchY * rows);

        selectedTile = brush.Inputs[id];
        selectedTileNumber = id;
        //selectedBrush = tileSet.GetBrushFromIndex(id);
      }

      base.OnPress(mousePos);
    }

    public override void Update(GameTime gameTime)
    {
      if (tileEditor.tileset != null)
      {
        rows = Width / tileEditor.tileset.TileWidth;
      }

      base.Update(gameTime);
    }

    public override void Draw(SpriteBatch sb)
    {
      base.Draw(sb);

      var tileset = tileEditor.tileset;
      var brush = tileEditor.selectedTile;

      if (brush == null || tileEditor.tileset == null) return;

      if (brush.HasInputs)
      {
        for (int i = 0; i < brush.Inputs.Count; i++)
        {
          var childBrush = brush.Inputs[i];

          var color = Color.White;

          if (selectedTileNumber == i)
          {
            color = Color.Green;
          }

          Rectangle sourceRect = new Rectangle(0, 0, tileset.TileWidth, tileset.TileHeight);
          Rectangle destRect = new Rectangle(X + i * tileset.TileWidth, Y, tileset.TileWidth, tileset.TileHeight);

          sourceRect.X = childBrush.GetFirstFrame() % tileset.Rows * tileset.TileWidth;
          sourceRect.Y = childBrush.GetFirstFrame() / tileset.Rows * tileset.TileHeight;

          destRect.X = X + i % rows * tileset.TileWidth;
          destRect.Y = Y + i / rows * tileset.TileHeight;

          //sourceRect.X = brush.GetFirstFrame() % tileset.Rows * tileset.TileWidth;
          //sourceRect.Y = brush.GetFirstFrame() / tileset.Rows * tileset.TileHeight;

          sb.Draw(tileset.Texture, destRect, sourceRect, color);
        }
      }
      else
      {
        Rectangle sourceRect = new Rectangle(0, 0, tileset.TileWidth, tileset.TileHeight);
        Rectangle destRect = new Rectangle(X, Y, tileset.TileWidth, tileset.TileHeight);

        sourceRect.X = brush.GetFirstFrame() % tileset.Rows * tileset.TileWidth;
        sourceRect.Y = brush.GetFirstFrame() / tileset.Rows * tileset.TileHeight;

        sb.Draw(tileset.Texture, destRect, sourceRect, Color.White);
      }
    }
  }
}