using System.Collections.Generic;
using Ace.Models;
using Ace.Scenes.UserInterface.UIBattlerActionSelectionMenu;
using Ace.Util;
using Godot;
using NUnit.Framework.Internal;

namespace Ace.Scenes.UserInterface.UIBattlerHUD;

public partial class BattlerHUD : TextureRect
{
  private Seq<LabelledBar> _bars;

  private Label _nameLabel;

  [Signal]
  public delegate void CombatActionHoveredEventHandler(ActionButton button);

  [Signal]
  public delegate void PlayerTargetSelectionCompleteEventHandler(Battler player, Battler[] targets);

  public override void _Ready()
  {
  }

  public BattlerHUD Setup(Battler battler)
  {
    _bars = new Seq<LabelledBar>
    {
      new(this.GetFirstChildOfType<HealthBar>(), FindChild("HealthLabel") as Label, StatType.Health),
      new(this.GetFirstChildOfType<StaminaBar>(), FindChild("StaminaLabel") as Label, StatType.Stamina),
      new(this.GetFirstChildOfType<ShieldBar>(), FindChild("ShieldLabel") as Label, StatType.Shield)
    };
    _nameLabel = this.GetFirstChildOfType<Label>();
    _nameLabel.Text = battler.Name;
    
    battler.Connect(Battler.SignalName.DamageTaken, new Callable(this, nameof(OnDamageTaken)));
    return this;
  }

  public void OnDamageTaken(Battler damagedCharacter, Battler _) =>
    _bars = damagedCharacter.Stats.VitalStats
      .Map(v => _bars
        .First(b => b.StatType == v.StatType)
        .SetTargetValue(v.GetCurrent(), v.BaseValue))
      .ToSeq();
}

public record LabelledBar(StatBar Bar, Label Label, StatType StatType)
{
  public LabelledBar SetTargetValue(float newValue, float maxValue)
  {
    Bar.SetTargetValue(newValue);
    Label.Text = $"{newValue} / {maxValue}";
    return this;
  }
}