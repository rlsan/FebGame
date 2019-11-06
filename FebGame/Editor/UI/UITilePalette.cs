﻿using FebEngine.Tiles;
using FebEngine.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebGame
{
  public class UITilePalette : UIElement
  {
    private Tileset tileSet;

    private int rows = 10;
    private int scale = 1;
    private int scaleMax = 5;
    private int scaleMin = 1;

    public bool hideBaseTiles = false;

    public TileBrush selectedBrush = new TileBrush();

    /*
    public UITilePalette(string title = "", bool isDraggable = false, bool isCloseable = true) : base(title, isDraggable, isCloseable)
    {
      this.title = title;
      this.isDraggable = isDraggable;
      this.isCloseable = isCloseable;
    }
    */

    public override void Init()
    {
      division = Division.horizontal;

      AddPanel(1.95f);

      var scrollBar = AddPanel();

      scrollBar.division = Division.vertical;

      scrollBar.AddButton("+", onClick: ZoomIn);
      scrollBar.AddButton("-", onClick: ZoomOut);
      scrollBar.AddButton("^");
      scrollBar.AddButton("v");
      //AddChild("ZoomOut", new UIButton("-", onClick: ZoomOut), Width - 30, menuBarHeight, 30, 30);
      //AddChild("ZoomIn", new UIButton("+", onClick: ZoomIn), Width - 30, menuBarHeight + 30, 30, 30);

      base.Init();
    }

    public void SetTileSet(Tileset tileSet)
    {
      this.tileSet = tileSet;
    }

    private void ZoomIn()
    {
      if (scale < scaleMax) scale++;
    }

    private void ZoomOut()
    {
      if (scale > scaleMin) scale--;
    }

    public override void OnPress(Point mousePos)
    {
      if (mousePos.Y > Y && mousePos.X < X + Width - 30)
      {
        int swatchX = (mousePos.X - X) / (tileSet.TileWidth * scale);
        int swatchY = (mousePos.Y - Y) / (tileSet.TileHeight * scale);

        int id = swatchX + (swatchY * rows);

        selectedBrush = tileSet.GetBrushFromIndex(id);
      }

      base.OnPress(mousePos);
    }

    public override void Update(GameTime gameTime)
    {
      if (tileSet != null)
      {
        rows = (Width) / (tileSet.TileWidth * scale);
      }

      base.Update(gameTime);
    }

    public override void Draw(SpriteBatch sb)
    {
      base.Draw(sb);

      if (tileSet != null)
      {
        Rectangle sourceRect = new Rectangle(0, 0, tileSet.TileWidth, tileSet.TileHeight);
        Rectangle destRect = new Rectangle(X, Y, tileSet.TileWidth * scale, tileSet.TileHeight * scale);

        for (int i = 0; i < tileSet.BrushCount; i++)
        {
          var brush = tileSet.Brushes[i];
          var color = Color.White;

          if (brush.isHidden)
          {
            continue;
          }

          if (hideBaseTiles && brush.brushType == TileBrushType.Tile)
          {
            color = Color.Gray;
            //continue;
          }

          destRect.X = X + i % rows * tileSet.TileWidth * scale;
          destRect.Y = Y + i / rows * tileSet.TileHeight * scale;

          sourceRect.X = brush.GetFirstFrame() % tileSet.Rows * tileSet.TileWidth;
          sourceRect.Y = brush.GetFirstFrame() / tileSet.Rows * tileSet.TileHeight;

          if (i == selectedBrush.id)
          {
            sb.Draw(tileSet.Texture, destRect, sourceRect, Color.Green);
          }
          else
          {
            sb.Draw(tileSet.Texture, destRect, sourceRect, color);
          }
        }
      }
    }
  }
}