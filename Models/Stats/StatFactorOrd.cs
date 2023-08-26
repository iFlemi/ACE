using LanguageExt.TypeClasses;
using System.Threading.Tasks;

namespace Ace.Models.Stats;

public struct StatFactorOrd : Ord<StatModifier>
{
  public int Compare(StatModifier x, StatModifier y) =>
      default(Ord<int>).Compare((int)x.Layer, (int)y.Layer);

  public Task<int> CompareAsync(StatModifier x, StatModifier y) =>
      Task.FromResult(Compare(x, y));

  public bool Equals(StatModifier x, StatModifier y) =>
      x == y;

  public Task<bool> EqualsAsync(StatModifier x, StatModifier y) =>
      Task.FromResult(Equals(x, y));


  public int GetHashCode(StatModifier x) =>
      x.GetHashCode();

  public Task<int> GetHashCodeAsync(StatModifier x) =>
      Task.FromResult(GetHashCode(x));
}

