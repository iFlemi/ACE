using Ace.Models.Abilities;
using Ace.Models.Abilities.Passive;
using Godot;
using LanguageExt;
using LanguageExt.Pipes;
using System;
using static LanguageExt.Prelude;

namespace Ace.Models;

public partial class PartyMember : Battler
{
  public override bool InParty => true;

  public PartyMember() : base()
  {
    //PassiveAbilities = PassiveAbilities.Add(new Preparation(Constants.FACTOR_3, Constants.INTENSIFIER_4));
    RecalculateAllStats();
  }

}
