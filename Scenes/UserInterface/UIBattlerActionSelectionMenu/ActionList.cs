using System;
using Ace.Models;
using Ace.Models.Abilities;
using Godot;
using LanguageExt.Pipes;
using Microsoft.VisualBasic.CompilerServices;
using Utils = Ace.Util.Utils;

namespace Ace.Scenes.UserInterface.UIBattlerActionSelectionMenu;

public partial class ActionList : VBoxContainer
{
  private PackedScene _actionButton;
  private Seq<ActionButton> _buttons;
  private MenuSelectArrow _arrow;

  [Signal]
  public delegate void ActionSelectedEventHandler(ActionButton button); 
  
  public override void _Ready()
  {
    _actionButton = ResourceLoader.Load<PackedScene>(ActionButtonPath);
    _arrow = Utils.GetFirstChildOfType<MenuSelectArrow>(this);
  }
  
  public ActionList Setup(Battler battler)
  {
    _buttons = battler.ActiveAbilities.Map(ability => pipe(
      _actionButton.Instantiate() as ActionButton,
      b => b.Setup(ability))
    ).Map(ConnectSignalsToButton);
    _arrow.Position = _buttons.Head().GetMenuArrowPosition();
    return this;
  }

  private ActionButton ConnectSignalsToButton(ActionButton button)
  {
    button.Connect(BaseButton.SignalName.Pressed, new Callable(button, nameof(OnPressed)));
    button.Connect(Control.SignalName.FocusEntered, new Callable(button, nameof(OnFocusEntered)));
    return button;
  }

  public void Focus() =>
    _buttons.Head().GrabFocus();
  
  public void SetButtonState(bool disableButtons) =>
    _buttons.Iter(button => button.Disabled = disableButtons);
  
  private void OnPressed(ActionButton button)
  {
    SetButtonState(disableButtons: true);
    EmitSignal(SignalName.ActionSelected, button);
  }
  
  private void OnFocusEntered(ActionButton button) =>
    _arrow.MoveTo(button.GetMenuArrowPosition());
    //EmitSignal(SignalName.ActionHovered, button);
}