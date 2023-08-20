using Ace.Models.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Interfaces;

public interface IStatCalculator : ISpeedCalculator
{

}

public interface ISpeedCalculator
{
    float GetSpeed(float agility, float intelligence, params PassiveAbility[] statAbilities);

}

public record StandardStatCalculator : IStatCalculator
{
    public float GetSpeed(float agility, float intelligence, params PassiveAbility[] statAbilities)
    {
        return agility;
    }
}
