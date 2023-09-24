namespace Ace.Models.Abilities.Active;

public record Attack(string Name = "Attack") : ActiveAbility(Name)
{
  public override string Description => "Attack an enemy with your equipped weapon.";

  public override Character.Battler Action(Character.Battler user, params Character.Battler[] targets)
    {
        return user;
    }
}
