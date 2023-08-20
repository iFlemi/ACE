using Ace.Interfaces;
using Ace.Models.Abilities;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Constants;

namespace Ace.Models;

public abstract partial class Battler : Sprite2D
{
    public Guid Id { get; } = Guid.NewGuid();
    [Export]
    public float Strength { get; set; } = 10;
    [Export]
    public float Willpower { get; set; } = 10;
    [Export]
    public float Endurance { get; set; } = 10;
    [Export]
    public float Power { get; set; } = 10;
    [Export]
    public float Agility { get; set; } = 10;
    [Export]
    public float Intelligence { get; set; } = 10;

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

    public virtual int MaxHealth => (int)(Endurance * EnduranceHealthFactor);
    public virtual int MaxStamina => (int)(Endurance * EnduranceStaminaFactor + Willpower * WillpowerStaminaFactor);
    public virtual int MaxShield => (int)(Willpower * WillpowerShieldFactor);

    public int CurrentHealth { get; set; }
    public int CurrentStamina { get; set; }
    public int CurrentShield { get; set; }

    public float CurrentAP { get; set; } = 0;
    public APState CurrentAPState { get; set; }

    public TurnBarIcon TurnBarIcon { get; set; }

    public abstract bool InParty { get; }

    public TriggeredAbility[] PassiveAbilities { get; set; }
    public ActiveAbility[] ActiveAbilities { get; set; }
    public PassiveAbility[] StatAbilities { get; set; }

    public virtual IDamageAllocator DamageAllocator { get; set; } = new StandardDamageAllocator();

    public virtual IStatCalculator StatCalculator { get; set; }

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

    public virtual float GetCurrentStrength() => Strength * GetHealthFactor();
    public virtual float GetCurrentWillpower() => Willpower * GetHealthFactor();
    public virtual float GetCurrentEndurance() => Endurance * GetHealthFactor();
    public virtual float GetCurrentPower() => Power * GetHealthFactor();
    public virtual float GetCurrentAgility() => Agility * GetHealthFactor();
    public virtual float GetCurrentIntelligence() => Intelligence * GetHealthFactor();


    [Signal]
    public delegate void DamageTakenEventHandler(Battler damagedCharacter, Battler damagingCharacter);
    [Signal]
    public delegate void CharacterDiedEventHandler(Battler deadCharacter, Battler killer);
    [Signal]
    public delegate void ReadyForInputEventHandler(Battler battler);
    [Signal]
    public delegate void ReadyToActEventHandler(Battler battler);

    public Battler UpdateAP(double delta)
    {
        (CurrentAPState, CurrentAP) = GetNewAPAndAPState((float)delta);
        EmitSignalsWhereRequired();
        return this;
    }

    private void EmitSignalsWhereRequired()
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
        (float)(CurrentAP + (StatCalculator.GetSpeed(Agility, Intelligence, StatAbilities) * delta) / 100);

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
