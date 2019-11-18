using FebEngine;
using FebEngine.Tiles;
using FebEngine.GUI;
using FebGame.States;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace FebGame.Editor
{
  public class GUIMapView : GUIContainer
  {
    private MapEditor mapEditor;
    private MapInfo mapInfo;
    private GUITilePalette brushSelector;
    private BrushInfo brushInfo;
    private GUIScrollWindow<ObjectID> objectList;

    public bool selectingDocument;
    public int selectedBrush;
    private GUIContainer document;

    public GUIMapView(MapEditor mapEditor)
    {
      this.mapEditor = mapEditor;
    }

    public override void Init()
    {
      Action a;
      division = Division.Horizontal;

      // Menu Panels
      var tools = AddPanel(400, 1f, ScalingType.absolute);
      tools.AddBar("Tools");
      tools.AddElement(new MapEditorInfo(mapEditor), 1, 100, ScalingType.percentage, ScalingType.absolute);

      var menu = tools.AddPanel(1, 200, ScalingType.percentage, ScalingType.absolute);
      a = () => { Canvas.RenamePrompt(ref mapEditor.editor.mapGroup.CurrentMap.Name); };
      menu.AddButton("Rename", a);
      menu.AddButton("Set Tileset...");
      menu.AddButton("Set BG...");
      menu.AddButton("Set Music...");
      a = () => { mapEditor.currentLayer.Clear(); };
      menu.AddButton("Clear Layer", a);
      a = () => { mapEditor.map.ClearLayers(); };
      menu.AddButton("Clear All", a);

      brushInfo = tools.AddElement(new BrushInfo()) as BrushInfo;
      brushInfo.heightScalingType = ScalingType.absolute;
      brushInfo.realHeight = 100;

      a = () => { selectedBrush = brushSelector.selectedBrush.id; };
      brushSelector = (GUITilePalette)tools.AddElement(new GUITilePalette());
      brushSelector.OnBrushSelect = a;

      // Document Panel
      document = AddPanel(1300, 1f, ScalingType.absolute);
      document.AddBar("Document");

      // Info Panel
      var info = AddPanel();
      info.AddBar("Map Info");
      mapInfo = (MapInfo)info.AddElement(new MapInfo(mapEditor.editor.mapGroup));
      info.AddBar("Object List");
      objectList = (GUIScrollWindow<ObjectID>)info.AddElement(new GUIScrollWindow<ObjectID>(10));
      info.AddBar("Key Commands");
      var commands = info.AddText();
      commands.alignment = TextAlignment.TopLeft;
      commands.SetMessage(
        "Tools:",
        "D - Draw",
        "E - Erase",
        "B - Object"
        );
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);

      if (mapEditor.map != null && mapEditor.map.Tileset != null)
      {
        brushSelector.SetTileset(mapEditor.map.Tileset);
        brushInfo.Brush = brushSelector.selectedBrush;

        string[] list = new string[mapEditor.map.ObjectLayer.Objects.Count];
        for (int i = 0; i < mapEditor.map.ObjectLayer.Objects.Count; i++)
        {
          var o = mapEditor.map.ObjectLayer.Objects[i];

          list[i] = o.id.ToString();
        }

        objectList.SetItems(mapEditor.map.ObjectLayer.Objects.ToArray());
      }
    }

    private class MapEditorInfo : GUIContainer
    {
      private MapEditor mapEditor;
      private GUIText text;

      public MapEditorInfo(MapEditor mapEditor)
      {
        this.mapEditor = mapEditor;
      }

      public override void Init()
      {
        base.Init();

        text = AddText();
        text.alignment = TextAlignment.TopLeft;
      }

      public override void Update(GameTime gameTime)
      {
        base.Update(gameTime);

        if (mapEditor.map != null)
        {
          text.SetMessage(
            "Tool: " + mapEditor.tool,
            "Layer: " + mapEditor.currentLayer.Name,
            "Brush: " + mapEditor.mapView.selectedBrush
            );
        }
      }
    }

    private class MapInfo : GUIContainer
    {
      private MapGroup mapGroup;
      private GUIText text;

      public MapInfo(MapGroup mapGroup)
      {
        this.mapGroup = mapGroup;
      }

      public override void Init()
      {
        base.Init();

        text = AddText();
        text.alignment = TextAlignment.TopLeft;
      }

      public override void Update(GameTime gameTime)
      {
        base.Update(gameTime);

        var map = mapGroup.CurrentMap;

        if (map != null)
        {
          var layerString = new StringBuilder();

          for (int i = map.LayerCount - 1; i >= 0; i--)
          {
            layerString.AppendLine("L" + (i + 1) + ": " + map.Layers[i].Name);
          }

          text.SetMessage(
            "Name: " + map.Name,
            "Size: " + map.Width + "x" + map.Height,
            //"Tileset: " + map.Tileset.name,
            //"Music: " + map.Music
            "Layers: " + map.LayerCount,
            layerString.ToString()
            );
        }
        else
        {
          text.SetMessage("No Map Selected.");
        }
      }
    }
  }
}