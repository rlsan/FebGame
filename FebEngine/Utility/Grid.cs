using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fubar
{
  public class Grid<T> : IEnumerable<T>
  {
    public List<List<T>> rows = new List<List<T>>();

    public int Width
    {
      get { return rows[0].Count; }
    }

    public int Height
    {
      get { return rows.Count; }
    }

    public int Count
    {
      get { return Height * Width; }
    }

    private T DefaultValue { get; }

    public Grid(int width, int height, T defaultValue)
    {
      for (int y = 0; y < height; y++)
      {
        var row = new List<T>();
        DefaultValue = defaultValue;

        for (int x = 0; x < width; x++)
        {
          row.Add(DefaultValue);
        }

        rows.Add(row);
      }
    }

    /// <summary>
    /// Place an item at the x and y coordinates.
    /// </summary>
    public void Place(int x, int y, T value)
    {
      if (x < 0 || y < 0 || x > Width || y > Height) return;

      rows[y][x] = value;
    }

    /// <summary>
    /// Place an item at the index.
    /// </summary>
    public void Insert(int index, T value)
    {
      int x = index % Width;
      int y = index / Width;

      if (x < 0 || y < 0 || x > Width || y > Height) return;

      rows[y][x] = value;
    }

    /// <summary>
    /// Get the item at the x and y coordinates.
    /// </summary>
    public T Get(int x, int y)
    {
      if (x < 0 || y < 0 || x > Width || y > Height) throw new OverflowException();

      return rows[y][x];
    }

    public void Fill(T value)
    {
      foreach (var row in rows)
      {
        for (int i = 0; i < row.Count; i++)
        {
          row[i] = value;
        }
      }
    }

    /// <summary>
    /// Extend or contract the top side of the grid.
    /// </summary>
    public void ExtendUp(int amount, T t)
    {
      if (amount == 0) return;
      if (amount > 0)
      {
        for (int i = 0; i < amount; i++)
        {
          var newRow = Enumerable.Repeat(t, Width).ToList();
          rows.Insert(0, newRow);
        }
      }
      else
      {
        for (int i = 0; i < Math.Abs(amount); i++)
        {
          if (Height > 1) rows.RemoveAt(0);
        }
      }
    }

    /// <summary>
    /// Extend or contract the bottom side of the grid.
    /// </summary>
    public void ExtendDown(int amount, T t)
    {
      if (amount == 0) return;
      if (amount > 0)
      {
        for (int i = 0; i < amount; i++)
        {
          var newRow = Enumerable.Repeat(t, Width).ToList();
          rows.Add(newRow);
        }
      }
      else
      {
        for (int i = 0; i < Math.Abs(amount); i++)
        {
          if (Height > 1) rows.RemoveAt(rows.Count - 1);
        }
      }
    }

    /// <summary>
    /// Extend or contract the right side of the grid.
    /// </summary>
    public void ExtendRight(int amount, T t)
    {
      if (amount < 0 && Width <= 1) return;

      foreach (var row in rows)
      {
        if (amount > 0)
        {
          for (int i = 0; i < amount; i++) row.Add(t);
        }
        else
        {
          for (int i = 0; i < Math.Abs(amount); i++) row.RemoveAt(row.Count - 1);
        }
      }
    }

    /// <summary>
    /// Extend or contract the left side of the grid.
    /// </summary>
    public void ExtendLeft(int amount, T t)
    {
      if (amount < 0 && Width <= 1) return;
      foreach (var row in rows)
      {
        if (amount > 0)
        {
          for (int i = 0; i < amount; i++) row.Insert(0, t);
        }
        else
        {
          for (int i = 0; i < Math.Abs(amount); i++) row.RemoveAt(0);
        }
      }
    }

    public override string ToString()
    {
      var s = new StringBuilder();

      foreach (var row in rows)
      {
        foreach (var item in row)
        {
          s.Append(item.ToString());
        }

        s.AppendLine();
      }

      return s.ToString();
    }

    public IEnumerator<T> GetEnumerator()
    {
      foreach (var row in rows)
      {
        foreach (var item in row)
        {
          yield return item;
        }
      }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}