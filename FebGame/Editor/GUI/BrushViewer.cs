using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FebEngine.Tiles;
using FebEngine.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FebGame.Editor
{
  public class TileList : GUIContainer
  {
    private GUITileView tileEditor;
    public TileBrush selectedBrush;
    public int selectedTileNumber;

    private int rows = 10;

    public TileList(GUITileView tileEditor)
    {
      this.tileEditor = tileEditor;

      drawBackground = true;
    }

    public Action OnBrushSelect;

    public void OnBrushSelected()
    {
      if (selectedBrush != null && OnBrushSelect != null)
      {
        OnBrushSelect.DynamicInvoke();
      }
    }

    public override void OnPress(Point mousePos)
    {
      var tileset = tileEditor.tileset;
      var brush = tileEditor.selectedTile;

      if (tileset != null)
      {
        int idSelector = (mousePos.X - X) / tileset.TileWidth;

        if (brush != null)
        {
          if (idSelector < brush.Inputs.Count)
          {
            selectedBrush = brush.Inputs[idSelector];
            selectedTileNumber = idSelector;

            OnBrushSelected();
            //tileEditor.selectedTileBrush = brush.Inputs[idSelector];
          }
        }

        if (mousePos.Y > Y && mousePos.X < X + Width)
        {
          int swatchX = (mousePos.X - X) / (tileset.TileWidth);
          int swatchY = (mousePos.Y - Y - tileset.TileHeight) / (tileset.TileHeight);

          int id = swatchX + (swatchY * rows);

          if (id < brush.Inputs.Count)
          {
            selectedBrush = brush.Inputs[id];
            selectedTileNumber = id;
          }
          //selectedBrush = tileSet.GetBrushFromIndex(id);
        }
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

      Rectangle sourceRect = new Rectangle(0, 0, tileset.TileWidth, tileset.TileHeight);
      Rectangle destRect = new Rectangle(X, Y, tileset.TileWidth, tileset.TileHeight);

      sourceRect.X = brush.GetPreviewFrame() % tileset.Rows * tileset.TileWidth;
      sourceRect.Y = brush.GetPreviewFrame() / tileset.Rows * tileset.TileHeight;

      sb.Draw(tileset.Texture, destRect, sourceRect, Color.White);

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

          sourceRect = new Rectangle(0, 0, tileset.TileWidth, tileset.TileHeight);
          destRect = new Rectangle(X + i * tileset.TileWidth, Y, tileset.TileWidth, tileset.TileHeight);

          sourceRect.X = childBrush.GetPreviewFrame() % tileset.Rows * tileset.TileWidth;
          sourceRect.Y = childBrush.GetPreviewFrame() / tileset.Rows * tileset.TileHeight;

          destRect.X = X + i % rows * tileset.TileWidth;
          destRect.Y = Y + tileset.TileHeight + i / rows * tileset.TileHeight;

          //sourceRect.X = brush.GetFirstFrame() % tileset.Rows * tileset.TileWidth;
          //sourceRect.Y = brush.GetFirstFrame() / tileset.Rows * tileset.TileHeight;

          sb.Draw(tileset.Texture, destRect, sourceRect, color);
        }
      }
      else
      {
      }
    }
  }
}