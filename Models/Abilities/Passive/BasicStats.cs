using Ace.Models.Stats;
using Ace.Util;
using Godot;

namespace Ace.Models.Abilities.Passive;

public record BasicStats(string Name = "Base Stats") : PassiveAbility(Name)
{
  public override string Description => "Basic Stats";

  public override Seq<StatModifier> GetStatModifiers() =>
  Seq<StatModifier>(
    new AdditiveValue(StatType.Strength, EnhancementLayer.Material, Constants.DefaultStatValue)
    , new AdditiveValue(StatType.Agility, EnhancementLayer.Material, GD.Randf() * 10)
    , new AdditiveValue(StatType.Intelligence, EnhancementLayer.Material, Constants.DefaultStatValue)
    , new AdditiveValue(StatType.Power, EnhancementLayer.Material, Constants.DefaultStatValue)
    , new AdditiveValue(StatType.Willpower, EnhancementLayer.Material, Constants.DefaultStatValue)
    , new AdditiveValue(StatType.Endurance, EnhancementLayer.Material, Constants.DefaultStatValue)
    , new SecondaryStatFactor(StatType.Speed, EnhancementLayer.Material, Constants.Factor5, StatType.Agility)
    , new SecondaryStatFactor(StatType.Critical, EnhancementLayer.Material, Constants.Intensifier2, StatType.Intelligence)
    , new SecondaryStatFactor(StatType.Evasion, EnhancementLayer.Material, Constants.Intensifier2, StatType.Agility)
    , new SecondaryStatFactor(StatType.Evasion, EnhancementLayer.Material, Constants.Intensifier1, StatType.Intelligence)
    , new SecondaryStatFactor(StatType.Health, EnhancementLayer.Material, Constants.Factor5, StatType.Endurance)
    , new SecondaryStatFactor(StatType.Stamina, EnhancementLayer.Material, Constants.Factor3, StatType.Willpower)
    , new SecondaryStatFactor(StatType.Stamina, EnhancementLayer.Material, Constants.Factor3, StatType.Endurance)
    , new SecondaryStatFactor(StatType.Stamina, EnhancementLayer.Material, Constants.Intensifier2, StatType.Intelligence)
  );
}
