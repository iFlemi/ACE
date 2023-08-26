using Godot;
using LanguageExt;
using static LanguageExt.Prelude;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Ace.Models.Stats;

public abstract record SecondaryStat : Stat
{
  public new int BaseValue { get; set; } = 1;
  public new int Current { get; private set; }
  public new int GetCurrent() => BaseValue + Current;
  
  public SecondaryStat DeriveFromPrimaryStats(Seq<SecondaryStatFactor> modifiers, PrimaryStats stats) => 
    this with {
      Current = pipe(
      modifiers,
      GetModifiersAffectingThisStat,
      GroupFactorsByStatAndLayer,
      grouped => MergeFactorsWithCurrentStats(grouped, stats),
      withStats => withStats.Fold(0f, (c, n) => c + n),
      Mathf.RoundToInt)
    };

  private Seq<SecondaryStatFactor> GetModifiersAffectingThisStat(Seq<SecondaryStatFactor> modifiers) => 
    modifiers.Filter(m => m.TargetStat == StatType);

  private static Map<SourceStatLayer, float> GroupFactorsByStatAndLayer(Seq<SecondaryStatFactor> modifiers) =>
    modifiers.GroupBy(mod => mod.SourceStatLayer)
      .Map(g => (g.Key, g.Sum(sf => sf.Factor)))
      .ToMap();

  private static Map<StatType, float> MergeFactorsWithCurrentStats(Map<SourceStatLayer, float> grouped, PrimaryStats stats) =>
    grouped
      .Map(grouping => grouping.Key.SourceStat switch
      {
        StatType.Strength => (StatType.Strength, stats.Strength.GetCurrent() * grouping.Value),
        StatType.Agility => (StatType.Agility, stats.Agility.GetCurrent() * grouping.Value),
        StatType.Intelligence => (StatType.Intelligence, stats.Intelligence.GetCurrent() * grouping.Value),
        StatType.Power => (StatType.Power, stats.Power.GetCurrent() * grouping.Value),
        StatType.Willpower => (StatType.Willpower, stats.Willpower.GetCurrent() * grouping.Value),
        StatType.Endurance => (StatType.Endurance, stats.Endurance.GetCurrent() * grouping.Value),
        _ => (StatType.Unknown, grouping.Value)
      }).ToMap();
}

public record Speed : SecondaryStat { public override StatType StatType => StatType.Speed; }
public record Critical : SecondaryStat { public override StatType StatType => StatType.Critical; }
public record Evasion : SecondaryStat { public override StatType StatType => StatType.Evasion; }
public record Health : SecondaryStat { public override StatType StatType => StatType.Health; }
public record Shield : SecondaryStat { public override StatType StatType => StatType.Shield; }
public record Stamina : SecondaryStat { public override StatType StatType => StatType.Stamina; }

