using Ace.Models.Abilities;
using Ace.Models.Abilities.Passive;
using LanguageExt;

namespace Ace.Models;

public partial class PartyMember : Battler
{
  public override bool InParty => true;

  public PartyMember()
  {
    PassiveAbilities = Seq.create<PassiveAbility>(new Preparation());
  }

}
