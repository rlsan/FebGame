using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TexturePackerLoader;

namespace FebEngine
{
  public class MapGroup : Entity
  {
    public List<Tilemap> Tilemaps { get; set; } = new List<Tilemap>();
    public Tilemap CurrentMap { get; set; }

    public void Load(string path)
    {
      Tilemaps = GroupIO.Import(path);

      ChangeMap(0);
    }

    public Tilemap AddMap(Tilemap tilemap)
    {
      Tilemaps.Add(tilemap);

      return tilemap;
    }

    public void Reset()
    {
      Tilemaps.Clear();
    }

    /// <summary>
    /// Changes the current map to one of the given name.
    /// </summary>
    /// <param name="name"></param>
    public void ChangeMap(string name)
    {
      foreach (var map in Tilemaps)
      {
        if (map.name.ToString() == name)
        {
          CurrentMap = map;

          break;
        }
      }
    }

    /// <summary>
    /// Changes the current map to one of the given index.
    /// </summary>
    /// <param name="id"></param>
    public void ChangeMap(int id)
    {
      if (id > Tilemaps.Count - 1 || id < 0) return;

      CurrentMap = Tilemaps[id];
    }

    public override void Draw(RenderManager renderer, GameTime gameTime)
    {
      if (CurrentMap != null)
      {
        CurrentMap.Draw(renderer, gameTime);
      }
    }
  }
}