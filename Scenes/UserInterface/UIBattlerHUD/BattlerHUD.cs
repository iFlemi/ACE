using Ace.Models.Stats;
using Ace.Scenes.UserInterface.UIBattlerActionSelectionMenu;
using Ace.Util;
using Godot;
using Battler = Ace.Models.Character.Battler;

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
    _bars = Seq(
      SetupBar<HealthBar>(StatType.Health, battler.Stats.VitalStats.Health.BarFillPercentage),
      SetupBar<StaminaBar>(StatType.Stamina, battler.Stats.VitalStats.Stamina.BarFillPercentage),
      SetupBar<ShieldBar>(StatType.Shield, battler.Stats.VitalStats.Shield.BarFillPercentage)
    );
    _nameLabel = this.GetFirstChildOfType<Label>();
    _nameLabel.Text = battler.Name;
    _bars = UpdateBarText(battler);
     
    battler.Connect(Battler.SignalName.DamageTaken, new Callable(this, nameof(OnDamageTaken)));
    return this;
  }

  private LabelledBar SetupBar<T>(StatType statType, float initialValue) where T : StatBar
  {
    var barNode = this.GetFirstChildOfType<T>();
    
    var labelledBar = new LabelledBar(barNode, barNode.GetFirstChildOfType<Label>(), statType);
    return labelledBar with { Bar = labelledBar.Bar.Setup(initialValue, Constants.BarMaxValue) };
  }
  

  public void OnDamageTaken(Battler damagedCharacter, Battler _) =>
    _bars = UpdateBarText(damagedCharacter);

  private Seq<LabelledBar> UpdateBarText(Battler battler) =>
    battler.Stats.VitalStats
      .Map(v => _bars
        .Filter(b => b.StatType == v.StatType)
        .HeadOrNone()
        .Map(s => s.SetTargetValue(v)))
      .ToSeq()
      .Somes()
      .Log();
}

public record LabelledBar(StatBar Bar, Label Label, StatType StatType)
{
  public LabelledBar SetTargetValue(VitalStat vs)
  {
    Bar.SetTargetValue(vs.BarFillPercentage);
    Label.Text = $"{Mathf.RoundToInt(vs.GetCurrent())} / {Mathf.RoundToInt(vs.BaseValue)}";
    return this;
  }
}