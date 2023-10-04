using Ace.Models.Stats;
using Ace.Util;

namespace Ace.Models.Abilities.Passive;

public record Shield(float WillpowerFactor, float PowerFactor, string Name = "Shield") : PassiveAbility(Name)
{
  public override string Description => "Grants user a shield that absorbs damage.";
  public override Seq<StatModifier> GetStatModifiers() => VitalStatModifiers;

  private Seq<StatModifier> VitalStatModifiers => Seq<StatModifier>(
    new VitalStatFactor(StatType.Shield, EnhancementLayer.Material, WillpowerFactor, StatType.Willpower),
    new VitalStatFactor(StatType.Shield, EnhancementLayer.Material, PowerFactor, StatType.Power)
    );
}