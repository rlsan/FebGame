using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FebEngine.Entities
{
  public class Camera : Entity
  {
    public Matrix TransformMatrix;
    public float scaleFactor = 1;

    public Camera()
    {
      TransformMatrix = new Matrix();
    }

    public Vector2 ScreenToWorldTransform(Vector2 vector)
    {
      return Vector2.Transform(vector, TransformMatrix) / 2;
    }

    public Vector2 WorldToScreenTransform(Vector2 vector)
    {
      return Vector2.Transform(vector, Matrix.Invert(TransformMatrix)) / 8;
    }

    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
    }

    public override void Update(GameTime gameTime)
    {
      var translation = Matrix.CreateTranslation(Transform.Position.X, Transform.Position.Y, 0);
      var scale = Matrix.CreateScale(scaleFactor, scaleFactor, 1);

      TransformMatrix = translation + scale;
    }
  }
}