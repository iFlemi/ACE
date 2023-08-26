using LanguageExt.TypeClasses;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Ace.Models.Stats;

public struct StatOrd : Ord<Stat>
{
  public int Compare(Stat x, Stat y) =>
    x.id.CompareTo(y.id);

  public Task<int> CompareAsync(Stat x, Stat y) =>
    Task.FromResult(Compare(x, y));

  public bool Equals(Stat x, Stat y) =>
    x.id == y.id;

  public Task<bool> EqualsAsync(Stat x, Stat y) =>
    Task.FromResult(Equals(x, y));
 
  public int GetHashCode(Stat x) =>
    x.id.GetHashCode();

  public Task<int> GetHashCodeAsync(Stat x) => 
    Task.FromResult(GetHashCode(x));
}

