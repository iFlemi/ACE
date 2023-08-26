using LanguageExt;
using System;
 
namespace Ace.Models.Stats;

public abstract record Stat
{
  public Guid id { get; set; } = Guid.NewGuid();
  public float BaseValue { get; set; }
  protected float Current;
  public float GetCurrent() => BaseValue + Current;
  public abstract StatType StatType { get; }

  public Stat UpdateAndFetchCurrent(Seq<StatModifier> modifiers)
  {
    Current = StatHelpers.GetModified(this, modifiers);
    return this;
  }
  public float SetCurrent(float value) => Current = value;

  public override int GetHashCode() => id.GetHashCode();
}

public abstract record PrimaryStat : Stat
{
}

public record Strength : PrimaryStat { public override StatType StatType =>  StatType.Strength; }
public record Agility : PrimaryStat{ public override StatType StatType => StatType.Agility; }
public record Intelligence : PrimaryStat{ public override StatType StatType => StatType.Intelligence; }
public record Power : PrimaryStat{ public override StatType StatType => StatType.Power; }
public record Willpower : PrimaryStat{ public override StatType StatType => StatType.Willpower; }
public record Endurance : PrimaryStat{ public override StatType StatType => StatType.Endurance; }