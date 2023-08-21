namespace Ace.Models.Abilities.Active;

public record Attack : ActiveAbility
{
    public override Battler Action(Battler user, params Battler[] targets)
    {
        return user;
    }
}
