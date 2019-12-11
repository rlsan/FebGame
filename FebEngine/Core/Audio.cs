using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace FebEngine
{
  public class Audio : Manager
  {
    private static Dictionary<string, SoundEffect> soundEffects = new Dictionary<string, SoundEffect>();
    private static Dictionary<string, Song> songs = new Dictionary<string, Song>();

    private static Song currentSong;

    public static AudioMixer MasterMixer = new AudioMixer();
    public static AudioMixer MusicMixer = new AudioMixer();
    public static AudioMixer SoundEffectMixer = new AudioMixer();

    private static bool isMusicFading = false;
    private static float fadingRate = 1f;
    private static float targetFadeVolume = 0f;

    public Audio(MainGame game) : base(game)
    {
    }

    public override void LoadContent(ContentManager content)
    {
      songs.Add("voa", content.Load<Song>("sfx/Valley-of-April"));

      soundEffects.Add("giddup", content.Load<SoundEffect>("sfx/giddup"));
      soundEffects.Add("jump", content.Load<SoundEffect>("sfx/jump"));
      soundEffects.Add("fire", content.Load<SoundEffect>("sfx/fire"));
      soundEffects.Add("explosion", content.Load<SoundEffect>("sfx/explosion"));
      soundEffects.Add("collect", content.Load<SoundEffect>("sfx/collect"));
      soundEffects.Add("superjump", content.Load<SoundEffect>("sfx/superjump"));
      soundEffects.Add("bounce", content.Load<SoundEffect>("sfx/bounce"));
      soundEffects.Add("land", content.Load<SoundEffect>("sfx/land"));
      soundEffects.Add("flee", content.Load<SoundEffect>("sfx/flee"));
      soundEffects.Add("hitCeiling", content.Load<SoundEffect>("sfx/ceilingHit"));
    }

    public override void Update(GameTime gameTime)
    {
      if (isMusicFading)
      {
        if (MediaPlayer.Volume > targetFadeVolume)
        {
          MediaPlayer.Volume -= Time.DeltaTime / fadingRate;

          if (MediaPlayer.Volume <= targetFadeVolume)
          {
            isMusicFading = false;
          }
        }
        else
        {
          MediaPlayer.Volume += Time.DeltaTime / fadingRate;

          if (MediaPlayer.Volume >= targetFadeVolume)
          {
            isMusicFading = false;
          }
        }
      }
    }

    public static void FadeInMusic(float rate)
    {
      FadeMusic(rate, 1);
    }

    public static void FadeOutMusic(float rate)
    {
      FadeMusic(rate, 0);
    }

    public static void FadeMusic(float rate, float target)
    {
      isMusicFading = true;
      fadingRate = rate;
      targetFadeVolume = target;
    }

    public static void SetMusic(string name)
    {
      if (songs.TryGetValue(name, out Song song))
      {
        if (song == currentSong) return;

        currentSong = song;
        MediaPlayer.Volume = MusicMixer.NormalizedVolume;
        MediaPlayer.Play(song);
      }
    }

    public static void PlaySound(string name, float volume = 1f, float pitch = 0, float pan = 0)
    {
      if (soundEffects.TryGetValue(name, out SoundEffect sfx))
      {
        sfx.Play(volume * MasterMixer.NormalizedVolume * SoundEffectMixer.NormalizedVolume, pitch, pan);
      }
    }

    public static void PlaySpatialSound(string name, float volume = 1f, float pitch = 0)
    {
      PlaySound(name, volume, pitch);
    }

    public class AudioMixer
    {
      private int volume = 100;

      public int Volume
      {
        get { return volume; }
        set { volume = (int)Mathf.Clamp(value, 0, 100); }
      }

      public float NormalizedVolume
      {
        get { return Volume / 100f; }
      }
    }
  }
}