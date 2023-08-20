using Ace;
using Ace.Models.Abilities;
using Ace.Models.Abilities.Passive;
using Godot;
using System;
using System.Transactions;

namespace Ace.Models;

public partial class PartyMember : Battler
{
    public override bool InParty => true;
   //public override float GetSpeed() => (0.75f * Agility) + (0.5f * Intelligence);

    public PartyMember() {
        StatAbilities = new[]
        {
            new Preparation()
        };
    }

}
