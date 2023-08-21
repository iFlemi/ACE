using Ace.Models;
using Ace.Models.Abilities.Passive;
using Ace.Models.Stats;

namespace Ace.Interfaces;

public interface IStatCalculator : ISpeedCalculator
{

}

public interface ISpeedCalculator
{
  float GetSpeed(Battler battler);

}

public record StandardStatCalculator : IStatCalculator
{
  public float GetSpeed(Battler battler)
  {
    var speedPassives = battler.PassiveAbilities
      .Filter(x => x.AffectedStats.Exists(y => y == typeof(Speed)))
      .Bind(ap => ap.GetStatModifiers());
    return battler.Speed.GetModified(speedPassives);
  }
}
