using Ace.Util;

namespace Ace.Models.Stats;
public static class StatHelpers
{
  public static float GetModified(Stat stat, Seq<StatModifier> modifiers) =>
  modifiers.Filter(f => f.TargetStat == stat.StatType)
    .GroupBy(f => f.Layer)
    .Fold((m: 1.0f, v: stat.BaseValue), (acc, factorsForLayer) => GetModifierForFactors(factorsForLayer.ToSeq())
    .Apply(layer => (acc.m * layer.layerMultiplier, acc.v + layer.layerValue)))
  .Apply(totalsForAllLayers => totalsForAllLayers.m * totalsForAllLayers.v);

  private static (float layerMultiplier, float layerValue) GetModifierForFactors(Seq<StatModifier> factors) =>
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


  public static AllStats RecalculateAllStats(this AllStats currentStats, Seq<StatModifier> modifiers)
  {
    var newPrimary = currentStats.PrimaryStats.RecalculatePrimaryStats(modifiers
      .Filter(m => m is not SecondaryStatFactor and not VitalStatFactor));
    
    var newSecondary = currentStats.SecondaryStats
      .DeriveAllSecondaryStats(modifiers
        .Filter(m => m is SecondaryStatFactor)
        .Cast<SecondaryStatFactor>(), newPrimary);
    
    var newVital = currentStats.VitalStats
      .DeriveAllVitalStats(modifiers
        .Filter(m => m is VitalStatFactor)
        .Cast<VitalStatFactor>(), newPrimary);
    
    return new AllStats { PrimaryStats = newPrimary, SecondaryStats = newSecondary, VitalStats = newVital };
  }

  public static PrimaryStats RecalculatePrimaryStats(this PrimaryStats stats, Seq<StatModifier> modifiers) =>
    stats with
    {
      Strength = stats.Strength.Recalculate(modifiers),
      Agility = stats.Agility.Recalculate(modifiers),
      Intelligence = stats.Intelligence.Recalculate(modifiers),
      Power = stats.Power.Recalculate(modifiers),
      Willpower = stats.Willpower.Recalculate(modifiers),
      Endurance = stats.Endurance.Recalculate(modifiers),
    };

  public static SecondaryStats RecalculateSecondaryStats(this SecondaryStats stats, Seq<StatModifier> modifiers) =>
    stats with
    {
      Speed = stats.Speed.Recalculate(modifiers),
      Critical = stats.Critical.Recalculate(modifiers),
      Evasion = stats.Evasion.Recalculate(modifiers),
      DamageMulti = stats.DamageMulti.Recalculate(modifiers),
      SpellDamageMulti = stats.SpellDamageMulti.Recalculate(modifiers),
      DamageResistance = stats.DamageResistance.Recalculate(modifiers)
    };
  
  public static VitalStats DeriveAllVitalStats(this VitalStats stats, Seq<VitalStatFactor> modifiers, PrimaryStats primaryStats) =>
    stats with
    {
      Health = stats.Health.Rederive(modifiers, primaryStats),
      Stamina = stats.Stamina.Rederive(modifiers, primaryStats),
      Shield = stats.Shield.Rederive(modifiers, primaryStats)
    };
  
  public static T Recalculate<T>(this T stat, Seq<StatModifier> statModifiers) where T: Stat =>
    (T)stat.UpdateAndFetchCurrent(statModifiers);

  public static SecondaryStats DeriveAllSecondaryStats(this SecondaryStats stats, Seq<SecondaryStatFactor> modifiers, PrimaryStats primaryStats) =>
    stats with
    {
      Speed = stats.Speed.Rederive(modifiers, primaryStats),
      Critical = stats.Critical.Rederive(modifiers, primaryStats),
      Evasion = stats.Evasion.Rederive(modifiers, primaryStats),
      DamageMulti = stats.DamageMulti.Rederive(modifiers, primaryStats),
      SpellDamageMulti = stats.SpellDamageMulti.Rederive(modifiers, primaryStats),
      DamageResistance = stats.DamageResistance.Rederive(modifiers, primaryStats)
    };
  
  public static T Rederive<T>(this T stat, Seq<SecondaryStatFactor> modifiers, PrimaryStats primaryStats) where T : SecondaryStat =>
    (T)stat.DeriveFromPrimaryStats(modifiers, primaryStats);
  
  public static T Rederive<T>(this T stat, Seq<VitalStatFactor> modifiers, PrimaryStats primaryStats) where T : VitalStat  =>
    (T)stat.DeriveFromPrimaryStats(modifiers, primaryStats);
  
  public static CurrentVitals ToCurrentVitals(this VitalStats vitalStats) =>
    new(
      vitalStats.Health.GetCurrent(),
      vitalStats.Stamina.GetCurrent(),
      vitalStats.Shield.GetCurrent()
    );
}
