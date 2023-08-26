﻿using Godot;
using LanguageExt;
using static LanguageExt.Prelude;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework.Internal;
using Newtonsoft.Json;

namespace Ace.Models.Stats;

public abstract record SecondaryStat : Stat
{
  public SecondaryStat DeriveFromPrimaryStats(Seq<SecondaryStatFactor> modifiers, PrimaryStats stats) =>
    this with
    {
      Current = pipe(
      modifiers,
      GetModifiersAffectingThisStat,
      GroupFactorsByStatAndLayer,
      grouped => MergeFactorsWithCurrentStats(grouped, stats),
      SumMergedFactorsByStat,
      withStats => withStats.Fold(0f, (c, n) => c + n),
      total => Mathf.Max(total, 0f)
      )
    };

  private Seq<SecondaryStatFactor> GetModifiersAffectingThisStat(Seq<SecondaryStatFactor> modifiers) =>
    modifiers.Filter(m => m.TargetStat == StatType);

  private static Seq<(SourceStatLayer, float)> GroupFactorsByStatAndLayer(Seq<SecondaryStatFactor> modifiers) =>
    modifiers.GroupBy(mod => mod.SourceStatLayer)
      .Map(g => (g.Key, g.Map(x => x.Factor).Sum()))
      .ToSeq();

  private static Seq<(StatType, float)> MergeFactorsWithCurrentStats(Seq<(SourceStatLayer, float)> grouped, PrimaryStats stats) =>
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

  private static Map<StatType, float> SumMergedFactorsByStat(Seq<(StatType, float)> statValues) =>
    statValues.GroupBy(x => x.Item1)
      .Map(grouping => (grouping.Key, grouping.Map(item => item.Item2).Sum()))
      .ToMap();
}

public record Speed : SecondaryStat { public override StatType StatType => StatType.Speed; }
public record Critical : SecondaryStat { public override StatType StatType => StatType.Critical; }
public record Evasion : SecondaryStat { public override StatType StatType => StatType.Evasion; }
public record Health : SecondaryStat { public override StatType StatType => StatType.Health; }
public record Shield : SecondaryStat { public override StatType StatType => StatType.Shield; }
public record Stamina : SecondaryStat { public override StatType StatType => StatType.Stamina; }

