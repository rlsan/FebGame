using FebEngine;
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
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using ChaiFoxes.FMODAudio;

namespace FebGame.States
{
  public class Sandbox : GameState
  {
    private Texture2D texture;
    private SpriteSheet spriteSheet;
    private SpriteSheet effectSheet;
    private ParticleEmitter emitter;
    private List<SoundEffect> soundEffects = new List<SoundEffect>();

    private MapGroup mapGroup;
    private Tileset tileset;

    private Sound music;
    private Sound sound;
    private Sound shootSound;

    private Sprite spriteA;

    private List<Sprite> bullets = new List<Sprite>();

    private Timer shootTimer;

    public override void Load(ContentManager content)
    {
      tileset = TilesetIO.Import("Tileset.ats", content);

      var spriteSheetLoader = new SpriteSheetLoader(content, RenderManager.Instance.GraphicsDevice);

      //texture = content.Load<Texture2D>("particles/Particle_DenseRain");
      spriteSheet = spriteSheetLoader.Load("sp1");
      effectSheet = spriteSheetLoader.Load("ef1");

      //soundEffects.Add(content.Load<SoundEffect>("sfx/explosion"));

      music = AudioMgr.LoadStreamedSound("sfx/Valley-of-April.wav");
      sound = AudioMgr.LoadStreamedSound("sfx/Explosion.wav");

      shootSound = AudioMgr.LoadStreamedSound("sfx/Explosion.wav");
      //var channel = music.Play();

      //channel.Looping = true;

      base.Load(content);
    }

    public override void Start()
    {
      mapGroup = Create.MapGroup("MapGroup");
      mapGroup.Load("C:\\Users\\Public\\Test\\Group1.amg");
      mapGroup.ChangeMap(0);

      //mapGroup.IsVisible = false;

      foreach (var map in mapGroup.Tilemaps)
      {
        map.Tileset = tileset;
      }

      shootTimer = Create.Timer("Timer");

      spriteA = Create.Sprite("SpriteA", "Sprite/Jewel");
      spriteA.Body.hasGravity = false;
      spriteA.tag = "Player";
      spriteA.Body.maxVelocity = Vector2.One * 5f;
      spriteA.Body.SetBounds(0, 0, 8, 8);
      //spriteA.SetTexture(spriteSheet);
      //spriteA.Animations.Add("Basic", "Sprite/Fruit", 12, true);
      //spriteA.Body.isTrigger = true;

      for (int i = 0; i < 40; i++)
      {
        var spriteB = Create.Sprite("SpriteB " + i, "Sprite/Coin");
        spriteB.Position = new Vector2(RNG.RandRange(-2000, 2000), RNG.RandRange(-2000, 2000));
        spriteB.Body.isDynamic = false;
        spriteB.Body.SetLayers(0, 1);

        //spriteB.Body.SetBounds(0, 0, RNG.RandIntRange(30, 150), RNG.RandIntRange(30, 150));
        spriteB.Scale = new Vector2(3, 3);

        spriteB.tag = "Coin";
        //spriteB.Body.isTrigger = true;

        //spriteB.Tint = Color.Blue;

        //spriteB.Body.collisionLayer = 0;
      }

      //spriteA.Body.Collision += Collect;

      for (int i = 0; i < 500; i++)
      {
        /*
        var bullet = Create.Sprite("bullet" + i);

        bullet.Body.SetLayers(1);
        bullet.Kill();
        //bullet.Body.hasGravity = false;
        bullet.tag = "bullet";

        bullet.Body.isTrigger = true;
        bullet.Body.Collision += Foo;

        bullet.Tint = Color.Red;
        bullets.Add(bullet);
        */
      }

      //spriteA.Body.collisionLayer = 0;

      //world.physics.collide(spriteA, spriteB);

      //thing.Animations.Add("Test", "Effect/ExplosionB");

      var anim = new Animation("Explosion", "Effect/ExplosionB", 12, true, effectSheet);

      emitter = Create.Emitter("Emitter", "Effect/ExplosionB", 200, false, EmitterShape.Circle);
      emitter.texture = texture;
      //emitter.width = 2000;
      emitter.height = 0;
      emitter.radius = 0;
      emitter.velocity = new Vector2(0, -200);
      emitter.randomAmount = 0.5f;
      emitter.emissionRate = 0.01f;
      emitter.animationSpeed = 1;
      emitter.maxSpeed = 1.2f;
      emitter.endAlpha = 1f;
      emitter.maxLifetime = 0.1f;
      emitter.minLifetime = 0.5f;

      emitter.gravity = 200;

      emitter.SetTexture(effectSheet);
      emitter.SetFrames(anim.frames);

      //spriteA.Body.Collision += Foo;
    }

    private void Collect(object sender, CollisionArgs e)
    {
      if (e.Other.tag == "Coin")
      {
        e.Other.Kill();
      }
    }

    private void Foo(object sender, CollisionArgs e)
    {
      //CollisionArgs c = e as CollisionArgs;

      if (e.Other.tag != "bullet")
      {
        emitter.Position = e.Other.Position;
        emitter.Emit(25);

        var channel = sound.Play();
        channel.Channel.setPan((e.Other.Position.X - world.camera.Position.X) / 1000);
        channel.Pitch = 1 - RNG.Normal() * 0.2f;

        e.Other.Kill();
        e.Primary.Kill();
        world.camera.Shake(20);
        //Console.WriteLine(e.Primary.isAlive);

        //spriteA.Kill();
      }
    }

    public override void Update(GameTime gameTime)
    {
      var mpos = Camera.ToWorld(canvas.mouse.Position);

      world.camera.Position = spriteA.Position;

      float speed = 5 * Time.DeltaTime;

      Debug.DrawRect(spriteA.Body.Bounds);

      foreach (var item in bullets)
      {
        if (item.Position.X > 1000)
        {
          item.Kill();
        }
        //item.Body.velocity.X = 300 * Time.DeltaTime;
      }

      if (canvas.MouseDown)
      {
        Vector2 vel = Vector2.Normalize(mpos - world.camera.Position);
        //if (!shootTimer.isRunning)
        //{
        //Debug.DrawRay(spriteA.Position, vel * 100);
        if (Physics.Raycast(spriteA.Position, vel, out RaycastHit hit, 1000, "Player"))
        {
          //emitter.Position = hit.Point;
          emitter.Emit(1, hit.Point);

          var channel = sound.Play();

          channel.Pitch = 0.8f + RNG.Normal() * 0.2f;

          //world.camera.Shake(20);

          //hit.Collider.Parent.Kill();
        }
        //shootTimer.Start(0.08f, null);

        //shootTimer.Start(0.08f, null);

        //var bullet = bullets.Find(s => !s.isAlive);
        //if (bullet != null)
        //{
        //  bullet.Revive();

        //  bullet.Position = spriteA.Position;
        //bullet.Body.velocity.X = 800 * Time.DeltaTime;
        //bullet.Body.velocity.Y = RNG.Normal() * 15 * Time.DeltaTime;

        //  vel += RNG.PointOnUnitCircle() * 0.02f;
        //  bullet.Body.velocity = vel * 1800 * Time.DeltaTime;

        //spriteA.Body.velocity += -vel * 20 * Time.DeltaTime;

        //shootSound.Play();
        //shootSound.Pitch = 6f;
        //shootSound.LowPass = 0.4f;
        //}
        //}
      }

      if (canvas.IsKeyDown(Keys.W))
      {
        spriteA.Body.velocity.Y += -speed;
      }
      else if (canvas.IsKeyDown(Keys.S))
      {
        spriteA.Body.velocity.Y += speed;
      }
      else
      {
        spriteA.Body.velocity.Y += 0;
      }
      if (canvas.IsKeyDown(Keys.A))
      {
        spriteA.Body.velocity.X += -speed;
      }
      else if (canvas.IsKeyDown(Keys.D))
      {
        spriteA.Body.velocity.X += speed;
      }
      else
      {
        spriteA.Body.velocity.X += 0;
      }

      //emitter.IsEmitting = canvas.MouseDown;
    }

    public override void Draw(RenderManager renderer, GameTime gameTime)
    {
      //var spriteRender = new SpriteRender(renderer.SpriteBatch);
      //spriteRender.Draw(spriteSheet.Sprite(TexturePackerMonoGameDefinitions.Helix.H0000), canvas.mouse.Position.ToVector2());
    }
  }
}