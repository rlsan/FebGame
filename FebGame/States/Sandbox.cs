using FebEngine;
using FebEngine.Entities;
using FebEngine.Tiles;
using TexturePackerLoader;
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
  public class Sandbox : GameState
  {
    private Texture2D texture;
    private SpriteSheet spriteSheet;
    private ParticleEmitter emitter;

    public override void Load(ContentManager content)
    {
      var spriteSheetLoader = new SpriteSheetLoader(content, RenderManager.Instance.GraphicsDevice);

      texture = content.Load<Texture2D>("particles/Particle_DenseRain");
      spriteSheet = spriteSheetLoader.Load("helix/helix.png");

      base.Load(content);
    }

    public override void Start()
    {
      emitter = Create.Emitter("Emitter", 200, true, EmitterShape.Rectangle);
      emitter.texture = texture;
      emitter.width = 2000;
      emitter.height = 0;
      emitter.radius = 0;
      emitter.velocity = new Vector2(-200, 1000);
      emitter.randomAmount = 0.05f;
      emitter.emissionRate = 0.02f;
      emitter.maxSpeed = 1.2f;
      emitter.endAlpha = 0f;
      emitter.maxLifetime = 0.5f;
      emitter.minLifetime = 0.5f;
      //emitter.gravity = 1000;

      emitter.SetTexture(spriteSheet);
    }

    public override void Update(GameTime gameTime)
    {
      var mpos = Camera.ToWorld(canvas.mouse.Position);

      emitter.Position = mpos;
      if (canvas.MousePress)
      {
        //emitter.Emit();
      }

      emitter.IsEmitting = canvas.MouseUp;
    }

    public override void Draw(RenderManager renderer, GameTime gameTime)
    {
      //var spriteRender = new SpriteRender(renderer.SpriteBatch);
      //spriteRender.Draw(spriteSheet.Sprite(TexturePackerMonoGameDefinitions.Helix.H0000), canvas.mouse.Position.ToVector2());
    }
  }
}