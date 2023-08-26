using Ace.Models.Stats;
using LanguageExt;

namespace Ace.Models.Abilities.Passive;

public record BasicStats(string Name = "Base Stats") : PassiveAbility(Name)
{
  public override string Description { get => "Basic Stats"; }

  public override Seq<StatModifier> GetStatModifiers() =>
  Seq.create<StatModifier>(
    new AdditiveValue(StatType.Strength, EnhancementLayer.Material, Constants.DEFAULT_STAT_VALUE)
    ,new AdditiveValue(StatType.Agility, EnhancementLayer.Material, Constants.DEFAULT_STAT_VALUE)
    ,new AdditiveValue(StatType.Intelligence, EnhancementLayer.Material, Constants.DEFAULT_STAT_VALUE)
    ,new AdditiveValue(StatType.Power, EnhancementLayer.Material, Constants.DEFAULT_STAT_VALUE)
    ,new AdditiveValue(StatType.Willpower, EnhancementLayer.Material, Constants.DEFAULT_STAT_VALUE)
    ,new AdditiveValue(StatType.Endurance, EnhancementLayer.Material, Constants.DEFAULT_STAT_VALUE)
    ,new SecondaryStatFactor(StatType.Speed, EnhancementLayer.Material, Constants.FACTOR_5, StatType.Agility)
    ,new SecondaryStatFactor(StatType.Critical, EnhancementLayer.Material, Constants.INTENSIFIER_2, StatType.Intelligence)
    ,new SecondaryStatFactor(StatType.Evasion, EnhancementLayer.Material, Constants.INTENSIFIER_2, StatType.Agility)
    ,new SecondaryStatFactor(StatType.Evasion, EnhancementLayer.Material, Constants.INTENSIFIER_1, StatType.Intelligence)
    ,new SecondaryStatFactor(StatType.Health, EnhancementLayer.Material, Constants.FACTOR_5, StatType.Endurance)
    ,new SecondaryStatFactor(StatType.Stamina, EnhancementLayer.Material, Constants.FACTOR_3, StatType.Willpower)
    ,new SecondaryStatFactor(StatType.Stamina, EnhancementLayer.Material, Constants.FACTOR_3, StatType.Endurance)
    ,new SecondaryStatFactor(StatType.Stamina, EnhancementLayer.Material, Constants.INTENSIFIER_2, StatType.Intelligence)
  );
}
