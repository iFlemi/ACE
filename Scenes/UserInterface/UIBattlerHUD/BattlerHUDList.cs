using Godot;
using System;
using Ace.Models;
using Ace.Scenes.UserInterface.UIBattlerHUD;
using Ace.Util;

public partial class BattlerHUDList : HBoxContainer
{
  private PackedScene _battlerHud;
  private Seq<BattlerHUD> _huds;

  public override void _Ready()
  {
    _battlerHud = ResourceLoader.Load<PackedScene>(BattlerHUDPath);
  }

  public BattlerHUDList Setup(Seq<Battler> battlers)
  {
     _huds = battlers.Map(b => pipe(
      _battlerHud.Instantiate() as BattlerHUD,
      hud => hud.Setup(b)
      ));
     _huds.Iter(hud => AddChild(hud));
     return this;
  }
}
