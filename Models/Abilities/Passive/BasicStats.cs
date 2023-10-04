using Ace.Models.Stats;
using Ace.Util;
using Godot;

namespace Ace.Models.Abilities.Passive;

public record BasicStats(string Name = "Base Stats") : PassiveAbility(Name)
{
  public override string Description => "Basic Stats";

  public override Seq<StatModifier> GetStatModifiers() =>
    PrimaryStatModifiers
      .Concat(SecondaryStatModifiers)
      .Concat(VitalStatModifiers);


  private static Seq<StatModifier> PrimaryStatModifiers =>
    Seq<StatModifier>(
      new AdditiveValue(StatType.Strength, EnhancementLayer.Material, Constants.DefaultStatValue + GD.Randf() * 6)
      , new AdditiveValue(StatType.Agility, EnhancementLayer.Material, Constants.DefaultStatValue + GD.Randf() * 6)
      , new AdditiveValue(StatType.Intelligence, EnhancementLayer.Material, Constants.DefaultStatValue + GD.Randf() * 6)
      , new AdditiveValue(StatType.Power, EnhancementLayer.Material, Constants.DefaultStatValue + GD.Randf() * 6)
      , new AdditiveValue(StatType.Willpower, EnhancementLayer.Material, Constants.DefaultStatValue + GD.Randf() * 6)
      , new AdditiveValue(StatType.Endurance, EnhancementLayer.Material, Constants.DefaultStatValue + GD.Randf() * 6));

  private static Seq<StatModifier> SecondaryStatModifiers =>
    Seq<StatModifier>(
      new SecondaryStatFactor(StatType.Speed, EnhancementLayer.Material, Constants.Factor5, StatType.Agility)
      , new SecondaryStatFactor(StatType.Critical, EnhancementLayer.Material, Constants.Intensifier2, StatType.Intelligence)
      , new SecondaryStatFactor(StatType.Evasion, EnhancementLayer.Material, Constants.Intensifier2, StatType.Agility)
      , new SecondaryStatFactor(StatType.Evasion, EnhancementLayer.Material, Constants.Intensifier1, StatType.Intelligence));

  private static Seq<StatModifier> VitalStatModifiers => Seq<StatModifier>(
    new VitalStatFactor(StatType.Health, EnhancementLayer.Material, Constants.Factor5, StatType.Endurance)
    , new VitalStatFactor(StatType.Stamina, EnhancementLayer.Material, Constants.Factor3, StatType.Willpower)
    , new VitalStatFactor(StatType.Stamina, EnhancementLayer.Material, Constants.Factor3, StatType.Endurance)
    , new VitalStatFactor(StatType.Stamina, EnhancementLayer.Material, Constants.Intensifier2, StatType.Intelligence));
}