using Ace.Interfaces;
using Ace.Models.Abilities;
using Ace.Models.Abilities.Passive;
using Ace.Models.Stats;
using Godot;
using LanguageExt;
using static LanguageExt.Prelude;
using System;
using static Constants;

namespace Ace.Models;

public abstract partial class Battler : Sprite2D
{
  public Guid Id { get; } = Guid.NewGuid();

  public AllStats Stats = new();

  [Export]
  public Texture2D BattleIcon { get; set; }
  [Export]
  public Texture2D BattleIconBorder { get; set; }

  public float CurrentAP { get; set; } = 0;
  public APState CurrentAPState { get; set; }

  public TurnBarIcon TurnBarIcon { get; set; }

  public abstract bool InParty { get; }

  public Seq<TriggeredAbility> TriggeredAbilities { get; set; }
  public Seq<ActiveAbility> ActiveAbilities { get; set; }
  public Seq<PassiveAbility> PassiveAbilities { get; set; } = Seq1<PassiveAbility>(new BasicStats());

  public virtual IDamageAllocator DamageAllocator { get; set; } = new StandardDamageAllocator();

  public Battler()
  {
  }

  public virtual float GetHealthFactor()
  {
    float healthPercentage = (float)Stats.SecondaryStats.Health.GetCurrent() / Stats.SecondaryStats.Health.BaseValue;

    float factorAt0 = 0.1f;
    float factorAt50 = 0.67f;
    float factorAt100 = 1.0f;

    float factor = Mathf.Lerp(factorAt0, factorAt100, healthPercentage * 2);
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

  public Battler UpdateAP(double delta)
  {
    (CurrentAPState, CurrentAP) = GetNewAPAndAPState((float)delta);
    EmitAPRelatedSignals();
    return this;
  }

  private void EmitAPRelatedSignals()
  {
    switch (CurrentAPState)
    {
      case APState.ReadyForInput:
        EmitSignal(SignalName.ReadyForInput, this);
        break;
      case APState.ReadyToActivate:
        EmitSignal(SignalName.ReadyToAct, this);
        break;
      case APState.Unknown:
        GD.PrintErr($"Battler {Id} - hit {nameof(APState)}: {APState.Unknown}\r\nnewAP: {CurrentAP}");
        break;
      default:
        break;
    }
  }

  protected virtual float CalculateNewAP(float delta) =>  
     (float)(CurrentAP + Stats.SecondaryStats.Speed.GetCurrent() * delta / 100);
  
  private (APState newState, float newAP) GetNewAPAndAPState(float delta)
  {
    var oldAP = CurrentAP;
    var adjustedAP = CalculateNewAP(delta);

    return adjustedAP switch
    {
      var ap when ap <= 0 => (APState.Waiting, 0),
      var ap when ap < AP_BAR_ACTION_POINT => (APState.Waiting, ap),
      var ap when ap >= AP_BAR_ACTION_POINT && oldAP < AP_BAR_ACTION_POINT => (APState.ReadyForInput, AP_BAR_ACTION_POINT),
      var ap when ap >= AP_BAR_ACTION_POINT && (CurrentAPState == APState.ReadyForInput || CurrentAPState == APState.WaitingForInput) => (APState.WaitingForInput, AP_BAR_ACTION_POINT),
      var ap when ap >= AP_BAR_ACTION_POINT && oldAP < 1.00f => (APState.Activating, ap),
      var ap when ap >= 1.00f && oldAP < 1.00f => (APState.ReadyToActivate, 1.00f),
      var ap when ap >= 1.00f => (APState.ReadyToActivate, 1.00f),
      _ => (APState.Unknown, CurrentAP)
    };
  }

  public Battler TakeDamage(int damageAmount, Battler damageSource)
  {
    var currentShield = Mathf.RoundToInt(Stats.SecondaryStats.Shield.GetCurrent());
    var currentStamina = Mathf.RoundToInt(Stats.SecondaryStats.Stamina.GetCurrent());
    var currentHealth = Mathf.RoundToInt(Stats.SecondaryStats.Health.GetCurrent());

    var (shield, stamina, health) = DamageAllocator.AllocateDamage(damageAmount, currentShield, currentStamina, currentHealth);

    Stats.SecondaryStats.Shield.SetCurrent(shield);
    Stats.SecondaryStats.Stamina.SetCurrent(stamina);
    Stats.SecondaryStats.Health.SetCurrent(health);

    var signalName = Stats.SecondaryStats.Health.GetCurrent() > 0
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
