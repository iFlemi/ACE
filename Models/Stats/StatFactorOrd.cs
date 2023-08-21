using LanguageExt.TypeClasses;
using System.Threading.Tasks;

namespace Ace.Models.Stats;

public struct StatFactorOrd : Ord<StatFactor>
{
    public int Compare(StatFactor x, StatFactor y) =>
        default(Ord<int>).Compare((int)x.Layer, (int)y.Layer);

    public Task<int> CompareAsync(StatFactor x, StatFactor y) => 
        Task.FromResult(Compare(x,y));

    public bool Equals(StatFactor x, StatFactor y) =>
        x == y;

    public Task<bool> EqualsAsync(StatFactor x, StatFactor y) =>    
        Task.FromResult(Equals(x, y));
    

    public int GetHashCode(StatFactor x) =>
        x.GetHashCode();

    public Task<int> GetHashCodeAsync(StatFactor x) =>
        Task.FromResult(GetHashCode(x));
}

