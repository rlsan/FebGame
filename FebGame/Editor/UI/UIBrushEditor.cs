using FebEngine.Tiles;
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
  public class UIBrushEditor : UIWindow
  {
    public TileBrush SelectedBrush { get; set; }
    public UITextBox BrushInfo { get; set; }

    public UIBrushEditor(string title = "", bool isDraggable = false, bool isCloseable = true) : base(title, isDraggable, isCloseable)
    {
      this.title = title;
      this.isDraggable = isDraggable;
      this.isCloseable = isCloseable;
    }

    public override void Init()
    {
      BrushInfo = AddChild("BrushInfo", new UITextBox(), 5, menuBarHeight + 74, 200, 200) as UITextBox;

      //for (int i = 0; i < 8; i++)
      //{
      //  AddChild("box" + i, new UITextField(), 5, 200 + (25 * i), 200, 25);
      //}
    }

    public void SetBrush(TileBrush brush)
    {
      SelectedBrush = brush;
    }

    public override void Update(GameTime gameTime)
    {
      if (SelectedBrush != null)
      {
        string s = "None";

        if (SelectedBrush.HasInputs)
        {
          s = "";

          foreach (var childBrush in SelectedBrush.Inputs)
          {
            if (childBrush == null)
            {
              break;
            }

            s += childBrush.Name + " ";
          }
        }

        BrushInfo.SetMessage(
          "Name: " + SelectedBrush.Name,
          "ID: " + SelectedBrush.id,
          "Type: " + SelectedBrush.GetType().Name,
          "Children: " + s
          );
      }

      base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      base.Draw(spriteBatch);

      if (SelectedBrush != null)
      {
        if (SelectedBrush.tileset != null)
        {
          if (SelectedBrush.HasInputs)
          {
            for (int i = 0; i < SelectedBrush.Inputs.Length; i++)
            {
              var childBrush = SelectedBrush.Inputs[i];

              Rectangle sourceRect = new Rectangle(0, 0, SelectedBrush.tileset.TileWidth, SelectedBrush.tileset.TileHeight);
              Rectangle destRect = new Rectangle((X + 5) + (i * 70), Y + menuBarHeight + 5, SelectedBrush.tileset.TileWidth, SelectedBrush.tileset.TileHeight);

              sourceRect.X = childBrush.GetFirstFrame() % SelectedBrush.tileset.Rows * SelectedBrush.tileset.TileWidth;
              sourceRect.Y = childBrush.GetFirstFrame() / SelectedBrush.tileset.Rows * SelectedBrush.tileset.TileHeight;

              spriteBatch.Draw(SelectedBrush.tileset.Texture, destRect, sourceRect, Color.White);
            }
          }
          else
          {
            Rectangle sourceRect = new Rectangle(0, 0, SelectedBrush.tileset.TileWidth, SelectedBrush.tileset.TileHeight);
            Rectangle destRect = new Rectangle(X + 5, Y + menuBarHeight + 5, SelectedBrush.tileset.TileWidth, SelectedBrush.tileset.TileHeight);

            sourceRect.X = SelectedBrush.GetFirstFrame() % SelectedBrush.tileset.Rows * SelectedBrush.tileset.TileWidth;
            sourceRect.Y = SelectedBrush.GetFirstFrame() / SelectedBrush.tileset.Rows * SelectedBrush.tileset.TileHeight;

            spriteBatch.Draw(SelectedBrush.tileset.Texture, destRect, sourceRect, Color.White);
          }
        }
      }
    }
  }
}