using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using FebEngine;
using FebEngine.Utility;
using FebEngine.UI;
using System.IO;
using System.Linq;

namespace FebGame.States
{
  internal class MapEditorState : GameState
  {
    private UICanvas canvas;
    private UIFileDialog saveDialog;

    public override void Load(ContentManager content)
    {
      canvas = new UICanvas();

      canvas.ThemeTexture = content.Load<Texture2D>("theme");
    }

    public override void Unload(ContentManager content)
    {
      content.Unload();
    }

    public override void Start()
    {
      canvas.AddElement("SaveButton", new UIButton("Save", onClick: OpenSaveDialog), 0, 0, 100, 30);

      saveDialog = canvas.AddElement("File", new UIFileDialog(), 0, 0, 30, 30, startInvisible: true) as UIFileDialog;
    }

    private void OpenSaveDialog()
    {
      saveDialog.Enable();
    }

    public override void Update(GameTime gameTime)
    {
      canvas.Update(gameTime);

      if (canvas.activeElement != null)
        Debug.DrawRect(canvas.activeElement.bounds);
    }

    public override void Draw(RenderManager renderer, GameTime gameTime)
    {
      renderer.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

      renderer.GraphicsDevice.Clear(Color.Black);
      canvas.DrawElements(renderer.SpriteBatch);
      Debug.Draw(renderer.SpriteBatch);

      renderer.SpriteBatch.End();
    }
  }
}