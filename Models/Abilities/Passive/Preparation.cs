using Ace.Models.Stats;
using LanguageExt;
using System.Security.Cryptography.X509Certificates;

namespace Ace.Models.Abilities.Passive;

public record Preparation : PassiveAbility
{
  private float IntToAgiFactor;
  private float AgiPenalty;
  
  public Preparation(float intToAgiFactor = 0.5f, float agiPenalty = 0.25f)
  {
    IntToAgiFactor = intToAgiFactor;
    AgiPenalty = agiPenalty;
    AffectedStats = Seq.create(typeof(Speed), typeof(Agility));
  }

  public override Seq<StatModifier> GetStatModifiers() =>
  Seq.create<StatModifier>(
    new AdditiveFactor { TargetStat = typeof(Speed), Layer = EnhancementLayer.Expertise, Factor = IntToAgiFactor },
    new AdditiveFactor { TargetStat = typeof(Speed), Layer = EnhancementLayer.Material, Factor = -AgiPenalty }
  );

}
