using System.Threading.Tasks;
using LanguageExt.TypeClasses;

namespace Ace.Models.Stats;

public struct StatOrd : Ord<Stat>
{
  public int Compare(Stat x, Stat y) =>
    x.Id.CompareTo(y.Id);

  public Task<int> CompareAsync(Stat x, Stat y) =>
    Task.FromResult(Compare(x, y));

  public bool Equals(Stat x, Stat y) =>
    x.Id == y.Id;

  public Task<bool> EqualsAsync(Stat x, Stat y) =>
    Task.FromResult(Equals(x, y));
 
  public int GetHashCode(Stat x) =>
    x.Id.GetHashCode();

  public Task<int> GetHashCodeAsync(Stat x) => 
    Task.FromResult(GetHashCode(x));
}

