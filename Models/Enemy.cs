using Ace.Models.Abilities.Passive;

namespace Ace.Models;

public partial class Enemy : Battler
{
  public override bool InParty => false;

  public Enemy() : base()
  {
    RecalculateAllStats();
  }
}