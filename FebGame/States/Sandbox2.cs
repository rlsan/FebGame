using FebEngine;
using FebEngine.Tiles;
using FebGame.Editor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Threading.Tasks;

namespace FebGame.States
{
  public class Sandbox2 : GameState
  {
    private Texture2D rectTexture;
    private SpriteFont font;

    private MapGroup mapGroup;
    private Tileset tileset;

    private Vector2 velocity;

    private Song song1;
    private Song song2;

    public override void Load(ContentManager content)
    {
      rectTexture = content.Load<Texture2D>("recthandles");
      font = content.Load<SpriteFont>("default");
      //tileset = new Tileset(content.Load<Texture2D>("tilesets/ts_test"), 64, 64);

      tileset = TilesetIO.Import("Tileset.ats", content);

      song1 = content.Load<Song>("audio/WL3-day");
      song2 = content.Load<Song>("audio/WL3-night");

      base.Load(content);
    }

    public override void Start()
    {
      //MediaPlayer.Play(song1);
      MediaPlayer.IsRepeating = true;

      mapGroup = Create.MapGroup("MapGroup");
      mapGroup.Load("C:\\Users\\Public\\Test\\Group1.amg");
      mapGroup.ChangeMap(0);

      foreach (var map in mapGroup.Tilemaps)
      {
        map.Tileset = tileset;
      }

      velocity = Vector2.Zero;

      base.Start();
    }

    public override void Update(GameTime gameTime)
    {
      float speed = 0.1f;

      var k = Keyboard.GetState();

      if (k.IsKeyDown(Keys.W))
      {
        velocity += new Vector2(0, -speed);
      }
      if (k.IsKeyDown(Keys.S))
      {
        velocity += new Vector2(0, speed);
      }
      if (k.IsKeyDown(Keys.A))
      {
        velocity += new Vector2(-speed, 0);
      }
      if (k.IsKeyDown(Keys.D))
      {
        velocity += new Vector2(speed, 0);
      }

      velocity *= 0.99f;

      world.camera.Position += velocity;

      if (world.camera.Position.X > mapGroup.CurrentMap.Width * 64)
      {
        foreach (var warp in mapGroup.CurrentMap.sideWarps)
        {
          int pos = (int)world.camera.Position.Y;

          if (warp.Direction == WarpDirection.Right && pos < warp.RangeMax * 64 && pos > warp.RangeMin * 64)
          {
            var map1Y = mapGroup.CurrentMap.Y * 64;

            mapGroup.ChangeMap(warp.DestinationMapName);

            var map2Y = mapGroup.CurrentMap.Y * 64;

            world.camera.Position = new Vector2(0, world.camera.Position.Y + (map1Y - map2Y));
          }
        }
      }
      if (world.camera.Position.X < 0)
      {
        foreach (var warp in mapGroup.CurrentMap.sideWarps)
        {
          int pos = (int)world.camera.Position.Y;

          if (warp.Direction == WarpDirection.Left && pos < warp.RangeMax * 64 && pos > warp.RangeMin * 64)
          {
            var map1Y = mapGroup.CurrentMap.Y * 64;

            mapGroup.ChangeMap(warp.DestinationMapName);

            var map2Y = mapGroup.CurrentMap.Y * 64;

            world.camera.Position = new Vector2(mapGroup.CurrentMap.Width * 64, world.camera.Position.Y + (map1Y - map2Y));
          }
        }
      }

      base.Update(gameTime);
    }

    public override void Draw(RenderManager renderer, GameTime gameTime)
    {
      var sb = renderer.SpriteBatch;

      if (mapGroup.CurrentMap != null)
      {
        var color = Color.White;
        var gridColor = new Color(0.1f, 0.1f, 0.1f);
        var warpColor = Color.Magenta;
        int offset = 10;

        var map = mapGroup.CurrentMap;
        var layer = map.GetLayer(1);

        var Bounds = new Rectangle(0, 0, map.Width, map.Height);

        //top
        sb.Draw(rectTexture, new Rectangle(Bounds.Left, Bounds.Top - offset, Bounds.Width * map.TileHeight, 20), new Rectangle(30, 0, 40, 20), color);

        //left
        sb.Draw(rectTexture, new Rectangle(Bounds.Left - offset, Bounds.Top, 20, Bounds.Height * map.TileWidth), new Rectangle(0, 30, 20, 40), color);

        //right
        sb.Draw(rectTexture, new Rectangle(Bounds.Right * map.TileWidth - offset, Bounds.Top, 20, Bounds.Height * map.TileWidth), new Rectangle(0, 30, 20, 40), color);

        //bottom
        sb.Draw(rectTexture, new Rectangle(Bounds.Left, Bounds.Bottom * map.TileHeight - offset, Bounds.Width * map.TileHeight, 20), new Rectangle(30, 0, 40, 20), color);

        foreach (var warp in map.sideWarps)
        {
          int rangeMin = (int)warp.RangeMin * map.TileWidth;
          int rangeMax = (int)warp.RangeMax * map.TileHeight;

          int size = Math.Abs(rangeMin - rangeMax);

          if (warp.Direction == WarpDirection.Left)
          {
            sb.Draw(rectTexture, new Rectangle(Bounds.Left - offset, Bounds.Top + rangeMin, 20, size), new Rectangle(0, 30, 20, 40), warpColor);
            sb.DrawString(font, warp.DestinationMapName, new Vector2(Bounds.Left, Bounds.Top + rangeMin), Color.White);
          }
          else if (warp.Direction == WarpDirection.Right)
          {
            sb.Draw(rectTexture, new Rectangle(Bounds.Right * map.TileWidth - offset, Bounds.Top + rangeMin, 20, size), new Rectangle(0, 30, 20, 40), warpColor);
            sb.DrawString(font, warp.DestinationMapName, new Vector2(Bounds.Right * map.TileWidth - offset, Bounds.Top + rangeMin), Color.White);
          }
          if (warp.Direction == WarpDirection.Up)
          {
            sb.Draw(rectTexture, new Rectangle(Bounds.Left + rangeMin, Bounds.Top - offset, size, 20), new Rectangle(30, 0, 40, 20), warpColor);
            sb.DrawString(font, warp.DestinationMapName, new Vector2(Bounds.Left + rangeMin, Bounds.Top - offset), Color.White);
          }
          else if (warp.Direction == WarpDirection.Down)
          {
            sb.Draw(rectTexture, new Rectangle(Bounds.Left + rangeMin, Bounds.Bottom * map.TileHeight - offset, size, 20), new Rectangle(30, 0, 40, 20), warpColor);
            sb.DrawString(font, warp.DestinationMapName, new Vector2(Bounds.Left + rangeMin, Bounds.Bottom * map.TileHeight - offset), Color.White);
          }
        }
      }

      base.Draw(renderer, gameTime);
    }
  }
}