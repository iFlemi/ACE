using LanguageExt;
using System;

namespace Ace.Models.Stats;

public abstract record StatModifier (StatType TargetStat, EnhancementLayer Layer);
public abstract record StatFactor (StatType TargetStat, EnhancementLayer Layer, float Factor) : StatModifier (TargetStat, Layer);
public abstract record StatValue (StatType TargetStat, EnhancementLayer Layer, float Value) : StatModifier (TargetStat, Layer);

public record AdditiveFactor (StatType TargetStat, EnhancementLayer Layer, float Factor) : StatFactor(TargetStat, Layer, Factor);
public record MultiplicativeFactor(StatType TargetStat, EnhancementLayer Layer, float Factor) : StatFactor(TargetStat, Layer, Factor);
public record AdditiveValue(StatType TargetStat, EnhancementLayer Layer, float Value) : StatValue(TargetStat, Layer, Value);
public record MultiplicativeValue(StatType TargetStat, EnhancementLayer Layer, float Value) : StatValue(TargetStat, Layer, Value);

public record SecondaryStatFactor(StatType TargetStat, EnhancementLayer Layer, float Factor, StatType SourceStat) : AdditiveFactor (TargetStat, Layer, Factor)
{
  public SourceStatLayer SourceStatLayer => new (Layer, SourceStat);
}

public record SourceStatLayer(EnhancementLayer Layer, StatType SourceStat) : IComparable<SourceStatLayer>
{
  public int CompareTo(SourceStatLayer other) =>
    SourceStat.CompareTo(other.SourceStat) != 0
      ? SourceStat.CompareTo(other.SourceStat)
      : Layer.CompareTo(other.Layer);

  public override int GetHashCode() =>
    ((int)Layer + (int)SourceStat).GetHashCode();
}