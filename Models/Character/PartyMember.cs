using System;
using Ace.Models.Abilities.Passive;
using Ace.Util;
using Godot;

namespace Ace.Models;

public partial class PartyMember : Character.Battler
{
  public override bool InParty => true;

  public PartyMember()
  {
    PassiveAbilities = PassiveAbilities.Add(new Preparation(Constants.Factor3, Constants.Intensifier4));
    if (GD.Randi() % 3 == 0)
      PassiveAbilities = PassiveAbilities.Add(new Shield(GD.Randf(), GD.Randf()));
    RecalculateAllStats();
  }

}
