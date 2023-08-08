using Ace;
using Godot;
using System;

public partial class PartyMember : Battler
{
    public override bool InParty => true;

    public override float GetSpeed() => (0.75f * Agility) + (0.5f * Intelligence);

}
