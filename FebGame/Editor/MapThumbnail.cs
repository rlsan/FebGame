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
    private Point mouseOffset;
    private bool isDragging;

    public Texture2D texture;
    public SpriteFont font;

    public int width;
    public int height;

    public Point TopLeft;
    public Point BottomRight;

    public Rectangle Bounds
    {
      get { return new Rectangle(TopLeft.X, TopLeft.Y, BottomRight.X - TopLeft.X, BottomRight.Y - TopLeft.Y); }
      //get { return new Rectangle((int)Position.X, (int)Position.Y, width, height); }
    }

    public MapThumbnail(Rectangle area, Tilemap tilemap, int gridSize)
    {
      this.tilemap = tilemap;
      this.gridSize = gridSize;

      TopLeft = area.Location;
      BottomRight = area.Location + area.Size;

      Position = tilemap.Position * gridSize;
      width = tilemap.Width * gridSize;
      height = tilemap.Height * gridSize;
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
      TopLeft.Y = Mathf.FloorToGrid(newPos.Y, gridSize);
    }

    private void StretchLeft(Vector2 newPos)
    {
      TopLeft.X = Mathf.FloorToGrid(newPos.X, gridSize);
    }

    private void StretchRight(Vector2 newPos)
    {
      BottomRight.X = Mathf.FloorToGrid(newPos.X, gridSize);
    }

    private void StretchDown(Vector2 newPos)
    {
      BottomRight.Y = Mathf.FloorToGrid(newPos.Y, gridSize);
    }

    public void Drag(Vector2 newPos)
    {
      var width = Bounds.Width;
      var height = Bounds.Height;

      TopLeft = new Point(
      Mathf.FloorToGrid(newPos.X - width / 2, gridSize),
      Mathf.FloorToGrid(newPos.Y - height / 2, gridSize)
      );

      BottomRight = new Point(
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
      int offset = 10;
      Vector2 nameOffset = new Vector2(3, 3);

      //spriteBatch.Draw(texture, Bounds, new Rectangle(2, 2, 16, 16), Color.Gray);

      //top
      spriteBatch.Draw(texture, new Rectangle(Bounds.Left, Bounds.Top - offset, Bounds.Width, 20), new Rectangle(30, 0, 40, 20), color);

      //left
      spriteBatch.Draw(texture, new Rectangle(Bounds.Left - offset, Bounds.Top, 20, Bounds.Height), new Rectangle(0, 30, 20, 40), color);

      //right
      spriteBatch.Draw(texture, new Rectangle(Bounds.Right - offset, Bounds.Top, 20, Bounds.Height), new Rectangle(0, 30, 20, 40), color);

      //bottom
      spriteBatch.Draw(texture, new Rectangle(Bounds.Left, Bounds.Bottom - offset, Bounds.Width, 20), new Rectangle(30, 0, 40, 20), color);

      //name
      spriteBatch.DrawString(font, tilemap.Name + ": " + Bounds.Width / gridSize + ", " + Bounds.Height / gridSize, TopLeft.ToVector2() + nameOffset, Color.White);
    }
  }
}