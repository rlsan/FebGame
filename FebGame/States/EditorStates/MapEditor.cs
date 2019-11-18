using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using FebEngine;
using FebEngine.GUI;
using FebEngine.Tiles;
using FebGame.Editor;
using Microsoft.Xna.Framework.Input;

namespace FebGame.States
{
  internal enum Tool
  {
    Draw, Erase, Object
  }

  public class MapEditor : GameState
  {
    internal MainEditor editor;
    internal GUIMapView mapView;
    internal Tool tool = Tool.Draw;

    internal Tilemap map;
    internal TilemapLayer currentLayer;

    private int layerID = 1;

    public override void Start()
    {
      mapView = canvas.AddElement(new GUIMapView(this), 0, 30, 1920, 1080 - 30) as GUIMapView;
      mapView.anchorPosition = AnchorPosition.Bottom;
    }

    public override void Update(GameTime gameTime)
    {
      Vector2 mpos = Camera.ToWorld(canvas.mouse.Position);

      if (canvas.IsKeyDown(Keys.D)) tool = Tool.Draw;
      if (canvas.IsKeyDown(Keys.E)) tool = Tool.Erase;
      if (canvas.IsKeyDown(Keys.B)) tool = Tool.Object;

      if (canvas.IsKeyDown(Keys.D1)) layerID = 0;
      if (canvas.IsKeyDown(Keys.D2)) layerID = 1;
      if (canvas.IsKeyDown(Keys.D3)) layerID = 2;

      if (editor.panning) return;
      if (editor.activeTilemap == null) return;

      map = editor.mapGroup.CurrentMap;
      currentLayer = map.GetLayer(layerID);

      if (tool == Tool.Draw)
      {
        if (canvas.mouse.RightButton == ButtonState.Pressed)
        {
          mapView.selectedBrush = currentLayer.GetTile(mpos);
        }
        if (canvas.MouseDown)
        {
          currentLayer.PutTile(mapView.selectedBrush, mpos);
        }
      }
      if (tool == Tool.Erase)
      {
        if (canvas.MouseDown)
        {
          currentLayer.EraseTile(mpos);
        }
      }
      if (tool == Tool.Object)
      {
        if (canvas.MousePress)
        {
          map.ObjectLayer.Add(mpos, RNG.RandIntRange(0, 99));
        }
      }
    }

    public override void Draw(RenderManager renderer, GameTime gameTime)
    {
      var sb = renderer.SpriteBatch;

      if (editor.activeTilemap == null) return;

      var color = Color.White;
      var gridColor = new Color(0.1f, 0.1f, 0.1f);
      var warpColor = Color.Magenta;
      int offset = 10;

      var map = editor.activeTilemap;

      var Bounds = new Rectangle(0, 0, map.Width, map.Height);

      foreach (var item in map.ObjectLayer.Objects)
      {
        sb.DrawString(canvas.Font, item.id.ToString(), item.position, Color.White);
      }

      // Draw Grid
      /*
      for (int i = 0; i < map.Width; i++)
      {
        sb.Draw(rectTexture, new Rectangle(
          Bounds.Left - offset + i * map.TileWidth,
          Bounds.Top,
          20,
          Bounds.Height * map.TileWidth
          ), new Rectangle(0, 30, 20, 40), gridColor);
      }
      for (int i = 0; i < map.Height; i++)
      {
        sb.Draw(rectTexture, new Rectangle(
          Bounds.Left,
          Bounds.Top - offset + i * map.TileHeight,
          Bounds.Width * map.TileHeight,
          20
          ), new Rectangle(30, 0, 40, 20), gridColor);
      }
      */

      // Draw Sides

      //top
      sb.Draw(editor.rectTexture, new Rectangle(Bounds.Left, Bounds.Top - offset, Bounds.Width * map.TileHeight, 20), new Rectangle(30, 0, 40, 20), color);
      //left
      sb.Draw(editor.rectTexture, new Rectangle(Bounds.Left - offset, Bounds.Top, 20, Bounds.Height * map.TileWidth), new Rectangle(0, 30, 20, 40), color);
      //right
      sb.Draw(editor.rectTexture, new Rectangle(Bounds.Right * map.TileWidth - offset, Bounds.Top, 20, Bounds.Height * map.TileWidth), new Rectangle(0, 30, 20, 40), color);
      //bottom
      sb.Draw(editor.rectTexture, new Rectangle(Bounds.Left, Bounds.Bottom * map.TileHeight - offset, Bounds.Width * map.TileHeight, 20), new Rectangle(30, 0, 40, 20), color);

      // Draw Side Warps
      foreach (var warp in map.sideWarps)
      {
        int rangeMin = (int)warp.RangeMin * map.TileWidth;
        int rangeMax = (int)warp.RangeMax * map.TileHeight;

        int size = Math.Abs(rangeMin - rangeMax);

        if (warp.Direction == WarpDirection.Left)
        {
          sb.Draw(editor.rectTexture, new Rectangle(Bounds.Left - offset, Bounds.Top + rangeMin, 20, size), new Rectangle(0, 30, 20, 40), warpColor);
          sb.DrawString(canvas.Font, warp.DestinationMapName, new Vector2(Bounds.Left, Bounds.Top + rangeMin), Color.White);
        }
        else if (warp.Direction == WarpDirection.Right)
        {
          sb.Draw(editor.rectTexture, new Rectangle(Bounds.Right * map.TileWidth - offset, Bounds.Top + rangeMin, 20, size), new Rectangle(0, 30, 20, 40), warpColor);
          sb.DrawString(canvas.Font, warp.DestinationMapName, new Vector2(Bounds.Right * map.TileWidth - offset, Bounds.Top + rangeMin), Color.White);
        }
        if (warp.Direction == WarpDirection.Up)
        {
          sb.Draw(editor.rectTexture, new Rectangle(Bounds.Left + rangeMin, Bounds.Top - offset, size, 20), new Rectangle(30, 0, 40, 20), warpColor);
          sb.DrawString(canvas.Font, warp.DestinationMapName, new Vector2(Bounds.Left + rangeMin, Bounds.Top - offset), Color.White);
        }
        else if (warp.Direction == WarpDirection.Down)
        {
          sb.Draw(editor.rectTexture, new Rectangle(Bounds.Left + rangeMin, Bounds.Bottom * map.TileHeight - offset, size, 20), new Rectangle(30, 0, 40, 20), warpColor);
          sb.DrawString(canvas.Font, warp.DestinationMapName, new Vector2(Bounds.Left + rangeMin, Bounds.Bottom * map.TileHeight - offset), Color.White);
        }
      }
    }
  }
}