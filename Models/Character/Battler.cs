using System.Linq;
using Ace.Interfaces;
using Ace.Models.Abilities;
using Ace.Models.Abilities.Passive;
using Ace.Models.Stats;
using Ace.Util;
using Godot;

namespace Ace.Models;

public abstract partial class Battler : Sprite2D
{
  public ulong Id { get; private set; }

  public AllStats Stats = new();

  [Export]
  public Texture2D BattleIcon { get; set; }
  [Export]
  public Texture2D BattleIconBorder { get; set; }

  public float CurrentAp { get; set; }
  public ApState CurrentApState { get; set; }

  public Scenes.Battle.TurnBar.TurnBarIcon TurnBarIcon { get; set; }

  public abstract bool InParty { get; }

  public Seq<TriggeredAbility> TriggeredAbilities { get; set; }
  public Seq<ActiveAbility> ActiveAbilities { get; set; }
  public Seq<PassiveAbility> PassiveAbilities { get; set; } = Seq1<PassiveAbility>(new BasicStats());

  public virtual IDamageAllocator DamageAllocator { get; set; } = new StandardDamageAllocator();

  public override void _Ready()
  {
    Id = GetInstanceId();
  }

  public CurrentVitals CurrentVitals => Stats.VitalStats.ToCurrentVitals();
   
    
  
  public virtual float GetHealthFactor()
  {
    var healthPercentage = Stats.VitalStats.Health.GetCurrent() / Stats.VitalStats.Health.BaseValue;

    const float factorAt0 = 0.1f;
    const float factorAt50 = 0.67f;
    const float factorAt100 = 1.0f;

    var factor = Mathf.Lerp(factorAt0, factorAt100, healthPercentage * 2);
    factor = Mathf.Lerp(factor, factorAt50, Mathf.Abs(healthPercentage - 0.5f) * 4);

    return Mathf.Clamp(factor, factorAt0, 1.0f);
  }

  [Signal]
  public delegate void DamageTakenEventHandler(Battler damagedCharacter, Battler damagingCharacter);
  [Signal]
  public delegate void CharacterDiedEventHandler(Battler deadCharacter, Battler killer);
  [Signal]
  public delegate void ReadyForInputEventHandler(Battler battler);
  [Signal]
  public delegate void ReadyToActEventHandler(Battler battler);
  [Signal]
  public delegate void NeedToRecalculateStatsEventHandler(Battler battler);

  public Battler UpdateAp(double delta)
  {
    (CurrentApState, CurrentAp) = GetNewApAndApState((float)delta);
    EmitApRelatedSignals();
    return this;
  }

  private void EmitApRelatedSignals()
  {
    switch (CurrentApState)
    {
      case ApState.ReadyForInput:
        EmitSignal(SignalName.ReadyForInput, this);
        break;
      case ApState.ReadyToActivate:
        EmitSignal(SignalName.ReadyToAct, this);
        break;
      case ApState.Unknown:
        GD.PrintErr($"Battler {Id} - hit {nameof(ApState)}: {ApState.Unknown}\r\nnewAP: {CurrentAp}");
        break;
    }
  }

  protected virtual float CalculateNewAp(float delta) =>  
     CurrentAp + Stats.SecondaryStats.Speed.GetCurrent() * delta / 100;
  
  private (ApState newState, float newAP) GetNewApAndApState(float delta)
  {
    var oldAp = CurrentAp;
    var adjustedAp = CalculateNewAp(delta);

    return adjustedAp switch
    {
      <= 0 => (ApState.Waiting, 0),
      var ap and < ApBarActionPoint => (ApState.Waiting, ap),
      >= ApBarActionPoint when oldAp < ApBarActionPoint => (ApState.ReadyForInput, ApBarActionPoint),
      >= ApBarActionPoint when CurrentApState is ApState.ReadyForInput or ApState.WaitingForInput => (ApState.WaitingForInput, ApBarActionPoint),
      var ap and >= ApBarActionPoint when oldAp < 1.00f => (ApState.Activating, ap),
      >= 1.00f when oldAp < 1.00f => (ApState.ReadyToActivate, 1.00f),
      >= 1.00f => (ApState.ReadyToActivate, 1.00f),
      _ => (ApState.Unknown, CurrentAp)
    };
  }

  public Battler TakeDamage(int damageAmount, Battler damageSource)
  {
    var currentShield = Mathf.RoundToInt(Stats.VitalStats.Shield.GetCurrent());
    var currentStamina = Mathf.RoundToInt(Stats.VitalStats.Stamina.GetCurrent());
    var currentHealth = Mathf.RoundToInt(Stats.VitalStats.Health.GetCurrent());

    var (shield, stamina, health) = DamageAllocator.AllocateDamage(damageAmount, currentShield, currentStamina, currentHealth, Stats.SecondaryStats.DamageResistance.GetCurrent());

    Stats.VitalStats.Shield.SetCurrent(shield);
    Stats.VitalStats.Stamina.SetCurrent(stamina);
    Stats.VitalStats.Health.SetCurrent(health);

    var signalName = Stats.VitalStats.Health.GetCurrent() > 0
        ? SignalName.DamageTaken
        : SignalName.CharacterDied;
    EmitSignal(signalName, this, damageSource);

    return this;
  }

  protected AllStats RecalculateAllStats() =>
    Stats = Stats.RecalculateAllStats(PassiveAbilityStatModifiers);

  protected PrimaryStats RecalculatePrimaryStats() =>
    Stats.PrimaryStats = Stats.PrimaryStats.RecalculatePrimaryStats(PassiveAbilityStatModifiers);
  protected SecondaryStats RecalculateSecondaryStats() =>
    Stats.SecondaryStats = Stats.SecondaryStats.RecalculateSecondaryStats(PassiveAbilityStatModifiers);

  protected SecondaryStats ReDeriveSecondaryStats() =>
    Stats.SecondaryStats = Stats.SecondaryStats
      .DeriveAllSecondaryStats(SecondaryStatFactors, Stats.PrimaryStats);

  protected Seq<StatModifier> PassiveAbilityStatModifiers =>
    PassiveAbilities.Bind(pa => pa.GetStatModifiers());

  protected Seq<SecondaryStatFactor> SecondaryStatFactors =>
    PassiveAbilityStatModifiers
    .Filter(x => x is SecondaryStatFactor)
    .Cast<SecondaryStatFactor>();

  public override int GetHashCode() => Id.GetHashCode();

  public override bool Equals(object obj) => obj is Battler other && other.Id == Id;

}
