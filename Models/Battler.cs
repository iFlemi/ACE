﻿using Ace.Interfaces;
using Ace.Models.Abilities;
using Ace.Models.Abilities.Passive;
using Ace.Models.Stats;
using Godot;
using LanguageExt;
using System;
using static Constants;

namespace Ace.Models;

public abstract partial class Battler : Sprite2D
{
  public Guid Id { get; } = Guid.NewGuid();

  public Strength Strength { get; set; }
  public Agility Agility { get; set; }
  public Intelligence Intelligence { get; set; }
  public Power Power { get; set; }
  public Willpower Willpower { get; set; }
  public Endurance Endurance { get; set; }

  public Speed Speed { get; set; }

  [Export]
  public Texture2D BattleIcon { get; set; }
  [Export]
  public Texture2D BattleIconBorder { get; set; }

  [Export]
  public float EnduranceHealthFactor = 0.5f;
  [Export]
  public float EnduranceStaminaFactor = 0.5f;
  [Export]
  public float WillpowerStaminaFactor = 0.5f;
  [Export]
  public float WillpowerShieldFactor = 0.0f;

  public virtual int MaxHealth => (int)(Endurance.Value * EnduranceHealthFactor);
  public virtual int MaxStamina => (int)(Endurance.Value * EnduranceStaminaFactor + Willpower.Value * WillpowerStaminaFactor);
  public virtual int MaxShield => (int)(Willpower.Value * WillpowerShieldFactor);

  public int CurrentHealth { get; set; }
  public int CurrentStamina { get; set; }
  public int CurrentShield { get; set; }

  public float CurrentAP { get; set; } = 0;
  public APState CurrentAPState { get; set; }

  public TurnBarIcon TurnBarIcon { get; set; }

  public abstract bool InParty { get; }

  public Seq<TriggeredAbility> TriggeredAbilities { get; set; }
  public Seq<ActiveAbility> ActiveAbilities { get; set; }
  public Seq<PassiveAbility> PassiveAbilities { get; set; } = Seq.create<PassiveAbility>(new BasicStats());

  public virtual IDamageAllocator DamageAllocator { get; set; } = new StandardDamageAllocator();

  public virtual ISpeedCalculator SpeedCalculator { get; set; }

  public virtual float GetHealthFactor()
  {
    float healthPercentage = (float)CurrentHealth / MaxHealth;

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
  public delegate void RecalculateStatsEventHandler(Battler battler);

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
      (float)(CurrentAP + (SpeedCalculator.GetSpeed(this) * delta) / 100);

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
    var (shield, stamina, health) = DamageAllocator.AllocateDamage(damageAmount, CurrentShield, CurrentStamina, CurrentHealth);

    CurrentShield = shield;
    CurrentStamina = stamina;
    CurrentHealth = health;

    var signalName = CurrentHealth > 0
        ? SignalName.DamageTaken
        : SignalName.CharacterDied;
    EmitSignal(signalName, this, damageSource);

    return this;
  }

  public override int GetHashCode() => Id.GetHashCode();

  public override bool Equals(object obj) => obj is Battler other && other.Id == Id;

}
