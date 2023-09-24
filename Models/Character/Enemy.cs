namespace Ace.Models;

public partial class Enemy : Character.Battler
{
  public override bool InParty => false;

  public Enemy()
  {
    RecalculateAllStats();
  }
}