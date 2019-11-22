using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebEngine
{
  static public class Debug
  {
    public static Texture2D pixelTexture;
    public static Texture2D fontTexture;

    public static SpriteFont spriteFont;

    private static List<Tuple<Rectangle, Color>> Rectangles = new List<Tuple<Rectangle, Color>>();
    private static List<Line> Lines = new List<Line>();
    private static List<RetroFont> Fonts = new List<RetroFont>();

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

      Fonts.Add(new RetroFont(fontTexture, new Vector2(x, y), 12, 20, message.ToString()));
    }

    public static void Text(object message, Vector2 position)

    {
      if (message == null)
      {
        message = "null";
      }

      Fonts.Add(new RetroFont(fontTexture, position, 12, 20, message.ToString()));
    }

    public static void Text(object message, Point position)

    {
      if (message == null)
      {
        message = "null";
      }

      Fonts.Add(new RetroFont(fontTexture, position.ToVector2(), 12, 20, message.ToString()));
    }

    public static void DrawLine(Vector2 start, Vector2 end)
    {
      Lines.Add(new Line(start, end));
    }

    public static void DrawLine(Line line)
    {
      Lines.Add(line);
    }

    public static void DrawRay(Vector2 origin, Vector2 direction)
    {
      Lines.Add(new Line(origin, origin + direction));
    }

    public static void DrawRect(Rectangle rect, Color? color = null)
    {
      Color c = color ?? Color.LimeGreen;

      Rectangles.Add(Tuple.Create(rect, c));
    }

    public static void DrawPoint(Vector2 position, int size = 4, Color? color = null)
    {
      Color c = color ?? Color.LimeGreen;

      int halfSize = size / 2;
      Rectangle rect = new Rectangle((int)position.X - halfSize, (int)position.Y - halfSize, size, size);

      Rectangles.Add(Tuple.Create(rect, c));
    }

    public static void Clear()
    {
      Rectangles.Clear();
      Lines.Clear();
      Fonts.Clear();
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
      foreach (var rect in Rectangles)
      {
        //spriteBatch.Draw(pixelTexture, rect.Item1, rect.Item2);

        spriteBatch.Draw(pixelTexture, new Rectangle(rect.Item1.Left, rect.Item1.Top, 2, rect.Item1.Height), rect.Item2);
        spriteBatch.Draw(pixelTexture, new Rectangle(rect.Item1.Right, rect.Item1.Top, 2, rect.Item1.Height + 2), rect.Item2);
        spriteBatch.Draw(pixelTexture, new Rectangle(rect.Item1.Left, rect.Item1.Top, rect.Item1.Width, 2), rect.Item2);
        spriteBatch.Draw(pixelTexture, new Rectangle(rect.Item1.Left, rect.Item1.Bottom, rect.Item1.Width, 2), rect.Item2);
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
        spriteBatch.DrawString(spriteFont, font.message, font.position, Color.White);
        //font.Draw(spriteBatch);
      }
    }
  }
}