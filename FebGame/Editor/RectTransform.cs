using FebEngine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FebGame
{
  public class RectTransform
  {
    public Texture2D texture;

    private Handle[] handles = new Handle[9];

    private Handle TopLeft { get { return handles[0]; } }
    private Handle Top { get { return handles[1]; } }
    private Handle TopRight { get { return handles[2]; } }

    private Handle Left { get { return handles[3]; } }
    private Handle Middle { get { return handles[4]; } }
    private Handle Right { get { return handles[5]; } }

    private Handle BottomLeft { get { return handles[6]; } }
    private Handle Bottom { get { return handles[7]; } }
    private Handle BottomRight { get { return handles[8]; } }

    public RectTransform(Texture2D texture)
    {
      this.texture = texture;

      int i = 0;
      foreach (AnchorPoint anchorPoint in Enum.GetValues(typeof(AnchorPoint)))
      {
        handles[i] = new Handle(anchorPoint);
        i++;
      }
    }

    public void SetHandles(Rectangle rect)
    {
      TopLeft.position = new Vector2(rect.Left, rect.Top);
      Top.position = new Vector2(rect.Center.X, rect.Top);
      TopRight.position = new Vector2(rect.Right, rect.Top);

      Left.position = new Vector2(rect.Left, rect.Center.Y);
      Middle.position = new Vector2(rect.Center.X, rect.Center.Y);
      Right.position = new Vector2(rect.Right, rect.Center.Y);

      BottomLeft.position = new Vector2(rect.Left, rect.Bottom);
      Bottom.position = new Vector2(rect.Center.X, rect.Bottom);
      BottomRight.position = new Vector2(rect.Right, rect.Bottom);
    }

    public Handle TestInput(Vector2 m, float e = 8)
    {
      foreach (var handle in handles)
      {
        if (Mathf.ApproxVec(m, handle.position, e))
        {
          return handle;
        }
      }

      return null;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      Color c = Color.White;

      int s = 20;
      Vector2 o = new Vector2(-s / 2, -s / 2);

      spriteBatch.Draw(texture, TopLeft.position + o, new Rectangle(s, s, s, s), c);
      spriteBatch.Draw(texture, Top.position + o, new Rectangle(s * 2, s, s, s), c);
      spriteBatch.Draw(texture, TopRight.position + o, new Rectangle(s * 3, s, s, s), c);

      spriteBatch.Draw(texture, Left.position + o, new Rectangle(s, s * 2, s, s), c);
      spriteBatch.Draw(texture, Middle.position + o, new Rectangle(s * 2, s * 2, s, s), c);
      spriteBatch.Draw(texture, Right.position + o, new Rectangle(s * 3, s * 2, s, s), c);

      spriteBatch.Draw(texture, BottomLeft.position + o, new Rectangle(s, s * 3, s, s), c);
      spriteBatch.Draw(texture, Bottom.position + o, new Rectangle(s * 2, s * 3, s, s), c);
      spriteBatch.Draw(texture, BottomRight.position + o, new Rectangle(s * 3, s * 3, s, s), c);
    }

    public class Handle
    {
      public AnchorPoint AnchorPoint { get; }
      public Vector2 position;

      public Handle(AnchorPoint anchorPoint)
      {
        AnchorPoint = anchorPoint;
      }
    }
  }

  public enum AnchorPoint
  {
    TopLeft, Top, TopRight, Left, Middle, Right, BottomLeft, Bottom, BottomRight
  }
}