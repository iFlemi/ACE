using System.Linq;
using Ace.Util;
using Godot;

namespace Ace.Models.Stats;

public abstract record SecondaryStat : Stat
{
  public SecondaryStat DeriveFromPrimaryStats(Seq<SecondaryStatFactor> modifiers, PrimaryStats stats) =>
    this with {
      Current = pipe(
      modifiers,
      GetModifiersAffectingThisStat,
      GroupFactorsByStatAndLayer,
      grouped => MergeFactorsWithCurrentStats(grouped, stats),
      SumMergedFactorsByStat,
      total => Mathf.Max(total, 1f))
    };

  protected Seq<SecondaryStatFactor> GetModifiersAffectingThisStat(Seq<SecondaryStatFactor> modifiers) =>
    modifiers.Filter(m => m.TargetStat == StatType);

  protected static Seq<(SourceStatLayer, float)> GroupFactorsByStatAndLayer(Seq<SecondaryStatFactor> modifiers) =>
    modifiers.GroupBy(mod => mod.SourceStatLayer)
      .Map(g => (g.Key, g.Map(x => x.Factor).Sum()))
      .ToSeq();

  protected static Seq<(StatType, float)> MergeFactorsWithCurrentStats(Seq<(SourceStatLayer, float)> grouped, PrimaryStats stats) =>
    grouped
      .Map(grouping => grouping.Item1.SourceStat switch
      {
        StatType.Strength => (StatType.Strength, stats.Strength.GetCurrent() * grouping.Item2),
        StatType.Agility => (StatType.Agility, stats.Agility.GetCurrent() * grouping.Item2),
        StatType.Intelligence => (StatType.Intelligence, stats.Intelligence.GetCurrent() * grouping.Item2),
        StatType.Power => (StatType.Power, stats.Power.GetCurrent() * grouping.Item2),
        StatType.Willpower => (StatType.Willpower, stats.Willpower.GetCurrent() * grouping.Item2),
        StatType.Endurance => (StatType.Endurance, stats.Endurance.GetCurrent() * grouping.Item2),
        _ => (StatType.Unknown, grouping.Item2)
      }).ToSeq();

  protected static float SumMergedFactorsByStat(Seq<(StatType, float)> statValues) =>
    statValues.GroupBy(x => x.Item1)
      .Map(grouping => grouping.Map(item => item.Item2).Sum())
      .Sum();
  
  protected static Seq<(StatType, float)> MergeFactorsWithBaseStats(Seq<(SourceStatLayer, float)> grouped, PrimaryStats stats) =>
    grouped
      .Map(grouping => grouping.Item1.SourceStat switch
      {
        StatType.Strength => (StatType.Strength, stats.Strength.BaseValue * grouping.Item2),
        StatType.Agility => (StatType.Agility, stats.Agility.BaseValue * grouping.Item2),
        StatType.Intelligence => (StatType.Intelligence, stats.Intelligence.BaseValue * grouping.Item2),
        StatType.Power => (StatType.Power, stats.Power.BaseValue * grouping.Item2),
        StatType.Willpower => (StatType.Willpower, stats.Willpower.BaseValue * grouping.Item2),
        StatType.Endurance => (StatType.Endurance, stats.Endurance.BaseValue * grouping.Item2),
        _ => (StatType.Unknown, grouping.Item2)
      }).ToSeq();
}

public record Speed : SecondaryStat { public override StatType StatType => StatType.Speed; }
public record Critical : SecondaryStat { public override StatType StatType => StatType.Critical; }
public record Evasion : SecondaryStat { public override StatType StatType => StatType.Evasion; }
public record DamageMulti : SecondaryStat { public override StatType StatType => StatType.DamageMulti; }
public record SpellDamageMulti : SecondaryStat { public override StatType StatType => StatType.SpellDamageMulti; }
public record DamageResistance : SecondaryStat { public override StatType StatType => StatType.DamageResistance; }


public abstract record VitalStat : SecondaryStat
{
  public new float GetCurrent() => Current;
  public new VitalStat DeriveFromPrimaryStats(Seq<SecondaryStatFactor> modifiers, PrimaryStats stats) =>
    this with
    {
      BaseValue = pipe(
        modifiers,
        GetModifiersAffectingThisStat,
        GroupFactorsByStatAndLayer,
        grouped => MergeFactorsWithBaseStats(grouped, stats),
        SumMergedFactorsByStat,
        total => Mathf.Max(total, 1f))
    };
}

public record Health : VitalStat { public override StatType StatType => StatType.Health; }
public record Shield : VitalStat { public override StatType StatType => StatType.Shield; }
public record Stamina : VitalStat { public override StatType StatType => StatType.Stamina; }

