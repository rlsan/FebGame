using FebEngine.Tiles;
using FebEngine.GUI;
using FebGame.States;
using Microsoft.Xna.Framework;
using System;

namespace FebGame.Editor
{
  public class GUIGroupView : GUIContainer
  {
    private GroupEditor groupEditor;
    private GroupInfo groupInfo;
    private ThumbnailInfo mapInfo;

    public bool selectingDocument;
    private GUIContainer document;

    public GUIGroupView(GroupEditor groupEditor)
    {
      this.groupEditor = groupEditor;
    }

    public override void Init()
    {
      Action a;
      division = Division.Horizontal;

      // Menu Panel
      var menu = AddPanel(120, 1f, ScalingType.absolute);
      menu.AddBar("Menu");
      menu.AddElement(new GroupEditorInfo(groupEditor));
      menu.AddButton("New", groupEditor.NewMapGroup);
      menu.AddButton("Save...", groupEditor.SaveGroup);
      a = () => { Canvas.OpenLoadPrompt(groupEditor.LoadGroup, "amg"); };
      menu.AddButton("Load...", a);
      menu.AddButton("Rename");
      menu.AddButton("Compile", groupEditor.Compile);
      menu.AddButton("Remove Map", groupEditor.RemoveMap);

      // Document Panel
      document = AddPanel(1500, 1f, ScalingType.absolute);
      document.AddBar("Document");

      // Info Panel
      var info = AddPanel();
      info.AddBar("Group Info");
      groupInfo = (GroupInfo)info.AddElement(new GroupInfo(groupEditor.editor.mapGroup));
      info.AddBar("Map Info");
      mapInfo = (ThumbnailInfo)info.AddElement(new ThumbnailInfo());
      info.AddBar("Key Commands");
      var commands = info.AddText();
      commands.alignment = TextAlignment.TopLeft;
      commands.SetMessage(
        "Tools:",
        "A - Select",
        "M - Move",
        "Q - Transform",
        "D - Draw"
        );
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);

      mapInfo.thumbnail = groupEditor.selectedThumbnail;
      selectingDocument = document.isHolding;
    }

    private class GroupEditorInfo : GUIContainer
    {
      private GroupEditor groupEditor;
      private GUIText text;

      public GroupEditorInfo(GroupEditor groupEditor)
      {
        this.groupEditor = groupEditor;
      }

      public override void Init()
      {
        text = AddText();
        text.alignment = TextAlignment.TopLeft;
      }

      public override void Update(GameTime gameTime)
      {
        base.Update(gameTime);

        text.SetMessage(
          "Tool: " + groupEditor.selectedTool.ToString()
          );
      }
    }

    private class GroupInfo : GUIContainer
    {
      private MapGroup mapGroup;
      private GUIText text;

      public GroupInfo(MapGroup mapGroup)
      {
        this.mapGroup = mapGroup;
      }

      public override void Init()
      {
        text = AddText();
        text.alignment = TextAlignment.TopLeft;
      }

      public override void Update(GameTime gameTime)
      {
        base.Update(gameTime);

        text.SetMessage(
          "Name: " + mapGroup.Name,
          "Maps: " + mapGroup.Tilemaps.Count
          );
      }
    }

    private class ThumbnailInfo : GUIContainer
    {
      public MapThumbnail thumbnail;
      private GUIText text;

      public override void Init()
      {
        text = AddText();
        text.alignment = TextAlignment.TopLeft;
      }

      public override void Update(GameTime gameTime)
      {
        base.Update(gameTime);

        if (thumbnail != null)
        {
          text.SetMessage(
            "Name: " + thumbnail.tilemap.Name
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