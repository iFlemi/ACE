﻿using System;
using Ace.Models.Stats;
using Godot;

namespace Ace.Models.Abilities;

public abstract record Ability (string Name)
{
  public Guid Id { get; } = Guid.NewGuid();
  public abstract string Description { get; }
  public override int GetHashCode() => Id.GetHashCode();
}

public class AbilityWrapper
{
  public ActiveAbility ActiveAbility { get; set; }
}

public abstract record BattleAbility (string Name) : Ability (Name)
{
  public abstract Character.Battler Action(Character.Battler user, params Character.Battler[] targets);
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
