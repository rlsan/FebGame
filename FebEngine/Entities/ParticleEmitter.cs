using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexturePackerLoader;

namespace FebEngine
{
  public class ParticleEmitter : Entity
  {
    public int Capacity { get; }
    private List<Particle> Particles { get; }
    public Texture2D texture;

    public SpriteSheet spriteSheet;

    // Shape

    public EmitterShape emitterShape;
    public float radius = 200;
    public float width = 200;
    public float height = 200;
    public Rectangle Bounds { get { return new Rectangle((int)(Position.X - width / 2), (int)(Position.Y - height / 2), (int)width, (int)height); } }

    // Emission

    public float emissionRate = 0.01f;
    public int emissionAmount = 1;
    public float minLifetime = 1f;
    public float maxLifetime = 1f;

    // Forces

    public Vector2 velocity;
    public float gravity;
    public float randomAmount = 0.1f;
    public float minSpeed = 1;
    public float maxSpeed = 1;

    // Properties

    public float startScale = 1;
    public float endScale = 1;
    public float rotation = 0;
    public Color startColor = Color.White;
    public Color endColor = Color.White;
    public float startAlpha = 1f;
    public float endAlpha = 1f;
    public float animationSpeed = 1;

    // Flags

    public bool IsEmitting { get; set; }

    private float emissionTimer;

    public ParticleEmitter(int capacity, bool startEmitting, EmitterShape emitterShape)
    {
      Capacity = capacity;
      Particles = new List<Particle>(Capacity);

      this.emitterShape = emitterShape;

      for (int i = 0; i < Capacity; i++)
      {
        var p = new Particle();
        Particles.Add(p);

        p.Kill();
      }

      if (startEmitting)
      {
        Start();
      }
    }

    public void SetTexture(SpriteSheet texture)
    {
      spriteSheet = texture;

      foreach (var item in spriteSheet.spriteList)
      {
      }
    }

    public void Start()
    {
      IsEmitting = true;
      emissionTimer = 0;
    }

    public void Stop()
    {
      IsEmitting = false;
    }

    public void Reset()
    {
      foreach (var particle in Particles)
      {
        particle.Kill();
        particle.position = Position;
      }

      Start();
    }

    /// <summary>
    /// Returns the first dead particle found in the system.
    /// </summary>
    private Particle GetFirstDead()
    {
      var particle = Particles.Find(p => !p.isAlive);

      if (particle == null) return Particle.nullParticle;
      else return particle;
    }

    /// <summary>
    /// Emits a specific amount of particles.
    /// </summary>
    public void Emit(int amount = 1)
    {
      for (int i = 0; i < amount; i++)
      {
        var p = GetFirstDead();

        p.Revive();

        // Move the particle to the back of the list to be drawn last.
        Particles.Remove(p);
        Particles.Add(p);

        // Reset the particle's lifetime and lerp it's velocity between the velocity and a random one.
        p.lifetime = RNG.RandRange(minLifetime, maxLifetime);
        p.totalLifetime = p.lifetime;

        var v = velocity * RNG.RandRange(minSpeed, maxSpeed);

        p.velocity = Vector2.Lerp(v, RNG.PointInsideUnitCircle() * velocity.Length(), randomAmount);
        p.rotationRate = RNG.Normal() * rotation;

        if (emitterShape == EmitterShape.Circle)
        {
          p.position = Position + RNG.PointInsideUnitCircle() * radius;
        }
        if (emitterShape == EmitterShape.Rectangle)
        {
          p.position = RNG.PointInRectangle(Bounds);
        }
      }
    }

    public override void Update(GameTime gameTime)
    {
      if (IsEmitting)
      {
        emissionTimer -= Time.DeltaTime;

        if (emissionTimer <= 0)
        {
          emissionTimer = emissionRate;

          Emit(emissionAmount);
        }
      }

      foreach (var particle in Particles)
      {
        if (particle.isAlive)
        {
          particle.velocity += Vector2.UnitY * gravity * Time.DeltaTime;
          particle.position += particle.velocity * Time.DeltaTime;

          particle.lifetime -= Time.DeltaTime;

          particle.scale = Mathf.lerp(endScale, startScale, particle.NormalizedLifetime);
          particle.rotation += particle.rotationRate * Time.DeltaTime;

          Color start = new Color(startColor, startAlpha);
          Color end = new Color(endColor, endAlpha);

          particle.color = Color.Lerp(end, start, particle.NormalizedLifetime);

          if (particle.lifetime <= 0)
          {
            particle.Kill();
          }
        }
      }
    }

    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
      int framerate = 30;
      var spriteRender = new SpriteRender(spriteBatch);

      int frames = (int)(Time.CurrentTime * framerate % spriteSheet.spriteList.Count);

      foreach (var particle in Particles)
      {
        if (particle.isAlive)
        {
          //int time = (int)(particle.NormalizedLifetime * animationSpeed * (spriteSheet.spriteList.Count - 1)) % spriteSheet.spriteList.Count;
          //spriteRender.Draw(spriteSheet.spriteList.ElementAt(time).Value, particle.position, particle.color, particle.rotation, particle.scale);
          spriteBatch.Draw(texture, particle.position, particle.color);
        }
      }
    }

    private class Particle
    {
      public bool isAlive;
      public Vector2 position;
      public Vector2 velocity;

      public float scale;
      public float rotation;
      public float rotationRate;
      public Color color;

      public float lifetime;
      public float totalLifetime;
      public float NormalizedLifetime { get { return lifetime / totalLifetime; } }

      public static Particle nullParticle = new Particle();

      public void Kill()
      {
        isAlive = false;
      }

      public void Revive()
      {
        isAlive = true;
      }
    }
  }

  public enum EmitterShape
  {
    Rectangle, Circle
  }

  public enum EmitterSpace
  {
    World, Local
  }
}