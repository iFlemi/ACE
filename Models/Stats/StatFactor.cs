using LanguageExt;
using System;

namespace Ace.Models.Stats;

public abstract record StatModifier
{
  public Type TargetStat { get; init; }
  public EnhancementLayer Layer { get; init; }

}

public abstract record StatFactor : StatModifier
{
  public float Factor { get; init; }
}

public abstract record StatValue : StatModifier
{
  public float Value { get; init; }
}

public record AdditiveFactor : StatFactor { }

public record MultiplicativeFactor : StatFactor { }

public record AdditiveValue : StatValue { }
public record MultiplicativeValue : StatValue { }