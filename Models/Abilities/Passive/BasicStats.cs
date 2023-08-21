using Ace.Models.Stats;
using LanguageExt;

namespace Ace.Models.Abilities.Passive;

public record BasicStats : PassiveAbility
{
  public BasicStats()
  {

  }

  public override Seq<StatModifier> GetStatModifiers() =>
  Seq.create<StatModifier>(
    new AdditiveValue { TargetStat = typeof(Strength), Layer = EnhancementLayer.Material, Value = Constants.DEFAULT_STAT_VALUE },
    new AdditiveValue { TargetStat = typeof(Agility), Layer = EnhancementLayer.Material, Value = Constants.DEFAULT_STAT_VALUE },
    new AdditiveValue { TargetStat = typeof(Intelligence), Layer = EnhancementLayer.Material, Value = Constants.DEFAULT_STAT_VALUE },
    new AdditiveValue { TargetStat = typeof(Power), Layer = EnhancementLayer.Material, Value = Constants.DEFAULT_STAT_VALUE },
    new AdditiveValue { TargetStat = typeof(Willpower), Layer = EnhancementLayer.Material, Value = Constants.DEFAULT_STAT_VALUE },
    new AdditiveValue { TargetStat = typeof(Endurance), Layer = EnhancementLayer.Material, Value = Constants.DEFAULT_STAT_VALUE }
  );
}
