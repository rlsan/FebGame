using FebEngine;
using FebEngine.Tiles;
using FebEngine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace FebGame
{
  public class MapThumbnail : Entity
  {
    public int gridSize;
    public Tilemap tilemap;

    public Texture2D texture;
    public SpriteFont font;

    public Point topLeft;
    public Point bottomRight;

    public Point TopRight { get { return new Point(topLeft.X + Width, topLeft.Y); } }
    public Point BottomLeft { get { return new Point(bottomRight.X - Width, bottomRight.Y); } }

    public int Width { get { return bottomRight.X - topLeft.X; } }
    public int Height { get { return bottomRight.Y - topLeft.Y; } }

    public int X { get { return topLeft.X; } }
    public int Y { get { return topLeft.Y; } }

    public Rectangle Bounds
    {
      get { return new Rectangle(topLeft.X, topLeft.Y, bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y); }
    }

    public MapThumbnail(Rectangle area, Tilemap tilemap, int gridSize)
    {
      this.tilemap = tilemap;
      this.gridSize = gridSize;

      topLeft = area.Location;
      bottomRight = area.Location + area.Size;
    }

    public void Transform(AnchorPoint anchor, Vector2 newPos)
    {
      switch (anchor)
      {
        case AnchorPoint.TopLeft:
          StretchUp(newPos);
          StretchLeft(newPos);
          break;

        case AnchorPoint.Top:
          StretchUp(newPos);
          break;

        case AnchorPoint.TopRight:
          StretchUp(newPos);
          StretchRight(newPos);
          break;

        case AnchorPoint.Left:
          StretchLeft(newPos);
          break;

        case AnchorPoint.Middle:
          Drag(newPos);
          break;

        case AnchorPoint.Right:
          StretchRight(newPos);
          break;

        case AnchorPoint.BottomLeft:
          StretchDown(newPos);
          StretchLeft(newPos);
          break;

        case AnchorPoint.Bottom:
          StretchDown(newPos);
          break;

        case AnchorPoint.BottomRight:
          StretchRight(newPos);
          StretchDown(newPos);
          break;

        default:
          break;
      }
    }

    private void StretchUp(Vector2 newPos)
    {
      topLeft.Y = Mathf.FloorToGrid(newPos.Y, gridSize);
    }

    private void StretchLeft(Vector2 newPos)
    {
      topLeft.X = Mathf.FloorToGrid(newPos.X, gridSize);
    }

    private void StretchRight(Vector2 newPos)
    {
      bottomRight.X = Mathf.FloorToGrid(newPos.X, gridSize);
    }

    private void StretchDown(Vector2 newPos)
    {
      bottomRight.Y = Mathf.FloorToGrid(newPos.Y, gridSize);
    }

    public void Drag(Vector2 newPos)
    {
      var width = Bounds.Width;
      var height = Bounds.Height;

      topLeft = new Point(
      Mathf.FloorToGrid(newPos.X - width / 2, gridSize),
      Mathf.FloorToGrid(newPos.Y - height / 2, gridSize)
      );

      bottomRight = new Point(
      Mathf.FloorToGrid(newPos.X + width / 2, gridSize),
      Mathf.FloorToGrid(newPos.Y + height / 2, gridSize)
      );
    }

    public void RefreshTilemap()
    {
      tilemap.X = Bounds.X / gridSize;
      tilemap.Y = Bounds.Y / gridSize;
      tilemap.Width = Bounds.Width / gridSize;
      tilemap.Height = Bounds.Height / gridSize;
    }

    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
      Color color = Color.LimeGreen;
      Color warpColor = Color.DarkGreen;
      int offset = 10;
      int nameOffset = 3;

      //spriteBatch.Draw(texture, Bounds, new Rectangle(2, 2, 16, 16), Color.Gray);

      var tiles = tilemap.GetLayer(1).Tiles;

      int i = 0;
      foreach (var tile in tiles)
      {
        int tileX = i % tiles.Width;
        int tileY = i / tiles.Width;

        if (tile != -1)
        {
          spriteBatch.Draw(texture, new Rectangle(X + tileX * gridSize, Y + tileY * gridSize, gridSize, gridSize), new Rectangle(2, 2, 16, 16), Color.Gray);
        }

        i++;
      }

      //top
      spriteBatch.Draw(texture, new Rectangle(Bounds.Left, Bounds.Top - offset, Bounds.Width, 20), new Rectangle(30, 0, 40, 20), color);

      //left
      spriteBatch.Draw(texture, new Rectangle(Bounds.Left - offset, Bounds.Top, 20, Bounds.Height), new Rectangle(0, 30, 20, 40), color);

      //right
      spriteBatch.Draw(texture, new Rectangle(Bounds.Right - offset, Bounds.Top, 20, Bounds.Height), new Rectangle(0, 30, 20, 40), color);

      //bottom
      spriteBatch.Draw(texture, new Rectangle(Bounds.Left, Bounds.Bottom - offset, Bounds.Width, 20), new Rectangle(30, 0, 40, 20), color);

      //name
      spriteBatch.DrawString(font, tilemap.Name + ": " + Bounds.Width / gridSize + ", " + Bounds.Height / gridSize, topLeft.ToVector2() + Vector2.One * nameOffset, Color.White);

      foreach (var warp in tilemap.sideWarps)
      {
        int rangeMin = (int)warp.RangeMin * gridSize;
        int rangeMax = (int)warp.RangeMax * gridSize;

        int size = Math.Abs(rangeMin - rangeMax);

        if (warp.Direction == WarpDirection.Left)
        {
          spriteBatch.Draw(texture, new Rectangle(Bounds.Left - offset, Bounds.Top + rangeMin, 20, size), new Rectangle(0, 30, 20, 40), warpColor);
        }
        else if (warp.Direction == WarpDirection.Right)
        {
          spriteBatch.Draw(texture, new Rectangle(Bounds.Right - offset, Bounds.Top + rangeMin, 20, size), new Rectangle(0, 30, 20, 40), warpColor);
        }
        if (warp.Direction == WarpDirection.Up)
        {
          spriteBatch.Draw(texture, new Rectangle(Bounds.Left + rangeMin, Bounds.Top - offset, size, 20), new Rectangle(30, 0, 40, 20), warpColor);
        }
        else if (warp.Direction == WarpDirection.Down)
        {
          spriteBatch.Draw(texture, new Rectangle(Bounds.Left + rangeMin, Bounds.Bottom - offset, size, 20), new Rectangle(30, 0, 40, 20), warpColor);
        }
      }

      /*
      spriteBatch.DrawString(font, topLeft.ToString(), topLeft.ToVector2(), Color.White);
      spriteBatch.DrawString(font, TopRight.ToString(), TopRight.ToVector2(), Color.White);
      spriteBatch.DrawString(font, BottomLeft.ToString(), BottomLeft.ToVector2(), Color.White);
      spriteBatch.DrawString(font, bottomRight.ToString(), bottomRight.ToVector2(), Color.White);
      */
    }
  }
}