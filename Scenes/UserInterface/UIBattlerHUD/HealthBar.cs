using Godot;
using System;
using Ace.Scenes.UserInterface.UIBattlerActionSelectionMenu;

public partial class HealthBar : StatBar 
{
  public override void OnTweenCompleted()
  {
    if (Value <= 0.2 * MaxValue)
      AnimationPlayer.Play("danger");
    else
      AnimationPlayer.Stop();
  }
}
