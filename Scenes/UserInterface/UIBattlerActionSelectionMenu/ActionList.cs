using Godot;

namespace Ace.Scenes.UserInterface.UIBattlerActionSelectionMenu;

public partial class ActionList : VBoxContainer
{
  private PackedScene _actionButton;

  public override void _Ready()
  {
    _actionButton = ResourceLoader.Load<PackedScene>(ActionButtonPath);
  }
}