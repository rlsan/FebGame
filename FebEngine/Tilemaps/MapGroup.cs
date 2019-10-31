using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using FebEngine.Utility;

namespace FebEngine.Tiles
{
  public class MapGroup : Entity
  {
    public List<Tilemap> Tilemaps { get; } = new List<Tilemap>();
    public Tilemap CurrentMap { get; set; }

    public int scale = 6;

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
        if (map.Name == name)
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
      CurrentMap = Tilemaps[id];
    }

    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
      if (CurrentMap != null)
      {
        CurrentMap.Draw(spriteBatch, gameTime);
      }
    }
  }
}