using FebEngine.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebEngine.UI
{
  public class UITilePalette : UIWindow
  {
    private Tileset tileSet;

    private int rows = 10;
    private int scale = 2;
    private int scaleMax = 5;
    private int scaleMin = 1;

    public TileBrush selectedTile = new TileBrush();

    public UITilePalette(string title = "", bool isDraggable = false, bool isCloseable = true) : base(title, isDraggable, isCloseable)
    {
      this.title = title;
      this.isDraggable = isDraggable;
      this.isCloseable = isCloseable;
    }

    public override void Init()
    {
      AddChild("ZoomOut", new UIButton("-", onClick: ZoomOut), Width - 30, menuBarHeight, 30, 30);
      AddChild("ZoomIn", new UIButton("+", onClick: ZoomIn), Width - 30, menuBarHeight + 30, 30, 30);

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
      if (mousePos.Y > Y + menuBarHeight && mousePos.X < X + Width - 30)
      {
        int swatchX = (mousePos.X - X) / (tileSet.TileWidth * scale);
        int swatchY = (mousePos.Y - (Y + menuBarHeight)) / (tileSet.TileHeight * scale);

        int id = swatchX + (swatchY * rows);

        selectedTile = tileSet.GetBrushFromIndex(id);
      }

      base.OnPress(mousePos);
    }

    public override void Update(GameTime gameTime)
    {
      rows = (Width - 30) / (tileSet.TileWidth * scale);

      base.Update(gameTime);
    }

    public override void Draw(SpriteBatch sb)
    {
      base.Draw(sb);

      if (tileSet != null)
      {
        Rectangle sourceRect = new Rectangle(0, 0, tileSet.TileWidth, tileSet.TileHeight);
        Rectangle destRect = new Rectangle(X, Y, tileSet.TileWidth * scale, tileSet.TileHeight * scale);

        for (int i = 0; i < tileSet.SwatchCount; i++)
        {
          destRect.X = X + i % rows * tileSet.TileWidth * scale;
          destRect.Y = Y + menuBarHeight + i / rows * tileSet.TileHeight * scale;

          sourceRect.X = i % tileSet.rows * tileSet.TileWidth;
          sourceRect.Y = i / tileSet.rows * tileSet.TileHeight;

          if (i == selectedTile.FrameId)
          {
            sb.Draw(tileSet.Texture, destRect, sourceRect, Color.Green);
          }
          else
          {
            sb.Draw(tileSet.Texture, destRect, sourceRect, Color.White);
          }
        }
      }
    }
  }
}