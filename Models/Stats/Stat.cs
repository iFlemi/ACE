using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Models.Stats;

public abstract record Stat
{
  public float Value;

  public float GetFactor(IEnumerable<StatFactor> factors) =>
    factors
      .Filter(f => f.TargetStat == GetType())
      .Fold(
        (a: 1.0f, m: 1.0f),
        (acc, next) => next switch
          {
            AdditiveFactor af => (acc.a + af.Value, acc.m),
            MultiplicativeFactor mf => (acc.a, acc.m + mf.Value),
            _ => acc
          })
      .Apply(acc => acc.a * acc.m);

  public float GetCurrent(IEnumerable<StatFactor> factors) => 
    Value * GetFactor(factors);
}
