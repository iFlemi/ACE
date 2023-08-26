namespace Ace.Models.Abilities.Active;

public record Attack(string Name = "Attack") : ActiveAbility(Name)
{
  public override string Description { get => "Attack an enemy with your equipped weapon."; }
  public override Battler Action(Battler user, params Battler[] targets)
    {
        return user;
    }
}
