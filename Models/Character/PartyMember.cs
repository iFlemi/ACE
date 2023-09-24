using Ace.Models.Abilities.Passive;
using Ace.Util;

namespace Ace.Models;

public partial class PartyMember : Character.Battler
{
  public override bool InParty => true;

  public PartyMember()
  {
    PassiveAbilities = PassiveAbilities.Add(new Preparation(Constants.Factor3, Constants.Intensifier4));
    RecalculateAllStats();
  }

}
