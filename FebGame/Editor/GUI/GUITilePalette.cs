using FebEngine.Tiles;
using FebEngine.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebGame
{
  public class GUITilePalette : GUIContainer
  {
    private Tileset tileset;

    private int rows = 10;
    private int scale = 1;
    private int scaleMax = 5;
    private int scaleMin = 1;

    public bool hideBaseTiles = false;

    public TileBrush selectedBrush = new TileBrush();

    public GUIElement palettePanel;

    public Action OnBrushSelect;

    public void OnBrushSelected()
    {
      if (selectedBrush != null && OnBrushSelect != null)
      {
        OnBrushSelect.DynamicInvoke();
      }
    }

    public override void Init()
    {
      base.Init();

      division = Division.Vertical;
      //AddBar("Tile Palette", 600, 30);

      var content = AddPanel();
      content.division = Division.Horizontal;

      var scrollBar = content.AddPanel(20, 200, ScalingType.absolute);

      scrollBar.division = Division.Vertical;

      scrollBar.AddButton("+", ZoomIn);
      scrollBar.AddButton("-", ZoomOut);
      scrollBar.AddButton("^");
      scrollBar.AddButton("v");

      palettePanel = content.AddPanel();
      //AddChild("ZoomOut", new UIButton("-", onClick: ZoomOut), Width - 30, menuBarHeight, 30, 30);
      //AddChild("ZoomIn", new UIButton("+", onClick: ZoomIn), Width - 30, menuBarHeight + 30, 30, 30);
    }

    public void SetTileset(Tileset tileSet)
    {
      this.tileset = tileSet;
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
      if (palettePanel.bounds.Contains(mousePos) && tileset != null)
      {
        if (mousePos.Y > palettePanel.Y && mousePos.X < palettePanel.X + Width - 30)
        {
          int swatchX = (mousePos.X - palettePanel.X) / (tileset.TileWidth * scale);
          int swatchY = (mousePos.Y - palettePanel.Y) / (tileset.TileHeight * scale);

          int id = swatchX + (swatchY * rows);

          selectedBrush = tileset.GetBrushFromIndex(id);

          OnBrushSelected();
        }
      }

      base.OnPress(mousePos);
    }

    public override void Update(GameTime gameTime)
    {
      if (tileset != null)
      {
        rows = Width / (tileset.TileWidth * scale);
      }

      base.Update(gameTime);
    }

    public override void Draw(SpriteBatch sb)
    {
      base.Draw(sb);

      if (tileset != null)
      {
        Rectangle sourceRect = new Rectangle(0, 0, tileset.TileWidth, tileset.TileHeight);
        Rectangle destRect = new Rectangle(X, Y, tileset.TileWidth * scale, tileset.TileHeight * scale);

        for (int i = 0; i < tileset.BrushCount; i++)
        {
          var brush = tileset.Brushes[i];
          var color = Color.White;

          if (brush.isHidden)
          {
            continue;
          }

          if (hideBaseTiles && brush.isLocked)
          {
            color = Color.Gray;
          }

          destRect.X = palettePanel.X + i % rows * tileset.TileWidth * scale;
          destRect.Y = palettePanel.Y + i / rows * tileset.TileHeight * scale;

          sourceRect.X = brush.GetPreviewFrame() % tileset.Rows * tileset.TileWidth;
          sourceRect.Y = brush.GetPreviewFrame() / tileset.Rows * tileset.TileHeight;

          if (i == selectedBrush.id)
          {
            sb.Draw(tileset.Texture, destRect, sourceRect, Color.Green);
          }
          else
          {
            sb.Draw(tileset.Texture, destRect, sourceRect, color);
          }

          var s = brush.id.ToString();
          sb.DrawString(Canvas.Font, s, destRect.Location.ToVector2() + Vector2.One, Color.Black);
          sb.DrawString(Canvas.Font, s, destRect.Location.ToVector2(), Color.White);
        }
      }
    }
  }
}