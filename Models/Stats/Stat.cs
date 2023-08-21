using Godot;
using LanguageExt;
using System;

namespace Ace.Models.Stats;

public abstract record Stat
{
  public Guid id { get; set; }

  public float Value;

  public bool IsCurrent = false;

  public float Current;

  [Signal]
  public delegate void UpdateRequiredEventHandler(Seq<StatModifier> modifiers);

  public float GetCurrent(Seq<StatModifier> modifiers) => 
    IsCurrent
    ? Current 
    : UpdateAndFetchCurrent(modifiers);

  private float UpdateAndFetchCurrent(Seq<StatModifier> modifiers)
  {
    Current = GetModified(modifiers);
    IsCurrent = true;
    return Current;
  }

  public float GetModified(Seq<StatModifier> modifiers) =>
    modifiers.Filter(f => f.TargetStat == GetType())
      .GroupBy(f => f.Layer)
      .Fold((m: 1.0f, v: Value), (acc, factorsForLayer) => GetModifierForLayer(factorsForLayer.ToSeq())
      .Apply(layer => (acc.m * layer.layerMultiplier, acc.v + layer.layerValue)))
    .Apply(totalsForAllLayers => totalsForAllLayers.m * totalsForAllLayers.v);


  public static (float layerMultiplier, float layerValue) GetModifierForLayer(Seq<StatModifier> factors) =>
    factors.Fold(
      (a: 1.0f, m: 1.0f, v: 0f, vm: 1.0f),
      (acc, next) => next switch
      {
        AdditiveFactor af => (acc.a + af.Factor, acc.m, acc.v, acc.vm),
        MultiplicativeFactor mf => (acc.a, acc.m + mf.Factor, acc.v, acc.vm),
        AdditiveValue av => (acc.a, acc.m, acc.v + av.Value, acc.vm),
        MultiplicativeValue mv => (acc.a, acc.m, acc.v, acc.vm + mv.Value),
        _ => acc
      })
    .Apply(acc => (acc.a * acc.m, acc.v * acc.vm));
}

public record Strength : Stat{}
public record Agility : Stat{}
public record Intelligence : Stat{}
public record Power : Stat{}
public record Willpower : Stat{}
public record Endurance : Stat{}

public record Speed : Stat { }