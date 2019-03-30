using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebEngine
{
  internal class Line
  {
    public Vector2 start;
    public Vector2 end;

    public float Distance
    {
      get
      {
        float p1 = end.X - start.X;
        float p2 = end.Y - start.Y;
        p1 *= p1;
        p2 *= p2;
        return (float)Math.Sqrt(p1 + p2);
      }
    }

    public float Slope
    {
      get
      {
        float p1 = end.X - start.X;
        float p2 = end.Y - start.Y;

        return p2 / p1;
      }
    }

    public float Angle
    {
      get
      {
        float dy = end.Y - start.Y;
        float dx = end.X - start.X;
        float theta = (float)Math.Atan2(dy, dx); // range (-PI, PI]
        //theta *= 180 / (float)Math.PI; // rads to degs, range (-180, 180]

        //if (theta < 0) theta = 360 + theta; // range [0, 360)
        return theta;
      }
    }

    public Line(Vector2 start, Vector2 end)
    {
      this.start = start;
      this.end = end;
    }
  }

  static public class Debug
  {
    public static Texture2D pixelTexture;
    public static Texture2D fontTexture;

    private static List<Tuple<Rectangle, Color>> Rectangles = new List<Tuple<Rectangle, Color>>();
    private static List<Line> Lines = new List<Line>();
    private static List<SpriteFont> Fonts = new List<SpriteFont>();

    public static void Instantiate()
    {
    }

    public static void Log(object o)
    {
      string message = o.ToString();
      Console.WriteLine(message);
    }

    public static void Text(object message, int x = 0, int y = 0)

    {
      if (message == null)
      {
        message = "null";
      }

      Fonts.Add(new SpriteFont(fontTexture, new Vector2(x, y), 12, 20, message.ToString()));
    }

    public static void Text(object message, Vector2 position)

    {
      if (message == null)
      {
        message = "null";
      }

      Fonts.Add(new SpriteFont(fontTexture, position, 12, 20, message.ToString()));
    }

    public static void Text(object message, Point position)

    {
      if (message == null)
      {
        message = "null";
      }

      Fonts.Add(new SpriteFont(fontTexture, position.ToVector2(), 12, 20, message.ToString()));
    }

    public static void DrawLine(Vector2 start, Vector2 end)
    {
      Lines.Add(new Line(start, end));
    }

    public static void DrawRect(Rectangle rect, Color? color = null)
    {
      Color c = color ?? Color.LimeGreen;

      Rectangles.Add(Tuple.Create(rect, c));
    }

    public static void DrawPoint(Vector2 position, int size = 4)
    {
      int halfSize = size / 2;
      Rectangle rect = new Rectangle((int)position.X - halfSize, (int)position.Y - halfSize, size, size);

      //Rectangles.Add(rect);
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
      foreach (var rect in Rectangles)
      {
        spriteBatch.Draw(pixelTexture, rect.Item1, rect.Item2);

        /*
        spriteBatch.Draw(pixelTexture, new Rectangle(rect.Left, rect.Top, 1, rect.Height), Color.LimeGreen);
        spriteBatch.Draw(pixelTexture, new Rectangle(rect.Right, rect.Top, 1, rect.Height + 1), Color.LimeGreen);
        spriteBatch.Draw(pixelTexture, new Rectangle(rect.Left, rect.Top, rect.Width, 1), Color.LimeGreen);
        spriteBatch.Draw(pixelTexture, new Rectangle(rect.Left, rect.Bottom, rect.Width, 1), Color.LimeGreen);
        */
      }
      foreach (var line in Lines)
      {
        Rectangle rect = new Rectangle((int)line.start.X, (int)line.start.Y, (int)line.Distance, 1);
        spriteBatch.Draw(
          texture: pixelTexture,
          destinationRectangle: rect,
          rotation: line.Angle,
          color: Color.LimeGreen
        );
      }
      foreach (var font in Fonts)
      {
        font.Draw(spriteBatch);
      }

      Rectangles.Clear();
      Lines.Clear();
      Fonts.Clear();
    }
  }
}