using Godot;
using System;

namespace Ace.Models.Abilities;

public abstract record Ability
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name;
    public string Description { get; set; }
    public override int GetHashCode() => Id.GetHashCode();
}

public abstract record BattleAbility
{
    public abstract Battler Action(Battler user, params Battler[] targets);
}

public abstract record ActiveAbility : BattleAbility
{
    [Export]
    public Texture2D MenuIcon { get; set; }
}

public abstract record TriggeredAbility : BattleAbility
{
    public abstract StringName TriggeringSingal { get; set; }
}

public abstract record PassiveAbility : Ability
{

}
