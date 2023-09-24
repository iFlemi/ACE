using Ace.Util;
using Godot;
using Battler = Ace.Models.Character.Battler;

namespace Ace.Scenes.UserInterface.UIBattlerHUD;

public partial class BattlerHUDList : VBoxContainer
{
  private PackedScene _battlerHud;
  private Seq<BattlerHUD> _huds;

  public override void _Ready()
  {
    _battlerHud = ResourceLoader.Load<PackedScene>(Constants.BattlerHUDPath);
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