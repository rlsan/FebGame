using FebEngine;
using FebEngine.Tiles;
using FebEngine.UI;
using FebGame.Editor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FebGame.States
{
  internal class TileEditor : GameState
  {
    public EditorState editor;
    public Tileset tileset;
    public List<Texture2D> tilesetTextures = new List<Texture2D>();

    private UITilesetInfo tilesetInfo;

    //private UITileInfo tileInfo;
    private UITilePalette tilePalette;

    public override void Load(ContentManager content)
    {
      LoadTilesetTextures(content, "tilesets");

      base.Load(content);
    }

    private void LoadTilesetTextures(ContentManager content, string tilesetDirectory)
    {
      // Get the current path and navigate to the tileset image directory.
      string currentPath = Directory.GetCurrentDirectory();
      string[] filePaths = Directory.GetFiles(currentPath + "\\Content\\" + tilesetDirectory);

      // Iterate through each file path.
      foreach (var filePath in filePaths)
      {
        // Extract the name from the file path.
        string fileName = Path.GetFileNameWithoutExtension(filePath);

        // Load the texture and add it to the texture list.
        Texture2D texture = content.Load<Texture2D>(tilesetDirectory + "\\" + fileName);
        tilesetTextures.Add(texture);
      }
    }

    public override void Start()
    {
      canvas.bounds.Width = 1920;
      canvas.bounds.Height = 1080;
      canvas.bounds.Y = 30;

      tileset = new Tileset(tilesetTextures[0], 64, 64);

      tilesetInfo = canvas.AddElement("TilesetInfo", new UITilesetInfo(tileset)) as UITilesetInfo;
      //tileInfo = canvas.AddElement("TileInfo", new UITileInfo(tileset)) as UITileInfo;
      //tilePalette = canvas.AddElement("TilePalette", new UITilePalette(title: "Palette", isDraggable: false, isCloseable: false), 150, 30, 810, 960) as UITilePalette;

      tilePalette.SetTileSet(tileset);

      var addTileButton = canvas.AddElement("AddTile", new UIButton("Add Tile", onClick: AddTile), 0, 30, 150, 30);
      var addRowTileButton = canvas.AddElement("AddRowTile", new UIButton("Add Row"), 0, 60, 150, 30);
      var addColumnTileButton = canvas.AddElement("AddColumnTile", new UIButton("Add Column"), 0, 90, 150, 30);
      var addRandomTileButton = canvas.AddElement("AddRandomTile", new UIButton("Add Random"), 0, 120, 150, 30);
      var addAnimatedTileButton = canvas.AddElement("AddAnimatedTile", new UIButton("Add Animated"), 0, 150, 150, 30);
      var removeTileButton = canvas.AddElement("RemoveTile", new UIButton("Remove Tile"), 0, 180, 150, 30);

      //var button1 = canvas.AddButton("Hello", onClick: Foo);
      //var button2 = canvas.AddButton("Hello Again", onClick: Foo);
    }

    private void RemoveTile()
    {
    }

    private void AddTile()
    {
      tileset.AddBrush(new TileBrush());
    }

    public override void Update(GameTime gameTime)
    {
      //tileInfo.SetTileBrush(tilePalette.selectedBrush);
    }

    public override void Draw(RenderManager renderer, GameTime gameTime)
    {
      foreach (var texture in tilesetTextures)
      {
        renderer.SpriteBatch.Draw(texture, Vector2.Zero, Color.White);
      }
    }
  }
}