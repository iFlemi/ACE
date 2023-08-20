using Godot;
using System;

namespace Ace.Models.Abilities;

public abstract class Ability
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name;
    public string Description { get; set; }
    public override int GetHashCode() => Id.GetHashCode();
    public override bool Equals(object obj) => obj is Ability other && other.Id == Id;
}

public abstract class BattleAbility
{
    public abstract Battler Action(Battler user, params Battler[] targets);
}

public abstract class ActiveAbility : BattleAbility
{
    [Export]
    public Texture2D MenuIcon { get; set; }
}

public abstract partial class TriggeredAbility : BattleAbility
{
    public abstract StringName TriggeringSingal { get; set; }
}

public abstract partial class PassiveAbility : Ability
{

}
