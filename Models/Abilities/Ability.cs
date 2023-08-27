using System;
using Ace.Models.Stats;
using Godot;

namespace Ace.Models.Abilities;

public abstract record Ability (string Name)
{
  public Guid Id { get; } = Guid.NewGuid();
  public abstract string Description { get; }
  public override int GetHashCode() => Id.GetHashCode();
}

public abstract record BattleAbility (string Name) : Ability (Name)
{
  public abstract Battler Action(Battler user, params Battler[] targets);
}

public abstract record ActiveAbility (string Name) : BattleAbility(Name)
{
  [Export]
  public Texture2D MenuIcon { get; set; }
}

public abstract record TriggeredAbility (string Name) : BattleAbility(Name)
{
  public abstract StringName TriggeringSignal { get; set; }
}

public abstract record PassiveAbility (string Name) : Ability(Name)
{
  public abstract Seq<StatModifier> GetStatModifiers();
}
