using Ace.Models.Stats;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Ace.Models.Abilities.Passive;

public record Preparation(float IntToAgiFactor, float AgiPenalty, string Name = "Preparation") : PassiveAbility (Name)
{
  public override string Description { get => 
      $"{IntToAgiFactor.FormatPercentage()} of {StatType.Intelligence} added to {StatType.Speed}. " +
      $"{AgiPenalty.FormatPercentage()} of {StatType.Agility} no longer added to {StatType.Speed}."; }

  public override Seq<StatModifier> GetStatModifiers() => Seq<StatModifier>(
    new SecondaryStatFactor (TargetStat: StatType.Speed, EnhancementLayer.Expertise, IntToAgiFactor, SourceStat: StatType.Intelligence),
    new SecondaryStatFactor (TargetStat: StatType.Speed, EnhancementLayer.Expertise, -AgiPenalty, SourceStat: StatType.Agility));
}
