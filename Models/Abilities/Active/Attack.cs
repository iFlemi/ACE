using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Models.Abilities.Active;

public class Attack : ActiveAbility
{
    public override Battler Action(Battler user, params Battler[] targets)
    {
        return user;
    }
}
