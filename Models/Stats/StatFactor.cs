using LanguageExt.TypeClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Models.Stats;

public abstract record StatFactor
{
    public Type SourceStat { get; init; }
    public Type TargetStat { get; init; }
    public float Value { get; init; }
    public abstract int OrderWeight { get; }
}

public record AdditiveFactor : StatFactor
{
    public override int OrderWeight => 10;
}

public record MultiplicativeFactor : StatFactor
{
    public override int OrderWeight => 1;
}

public struct StatFactorOrd : Ord<StatFactor>
{
    public int Compare(StatFactor x, StatFactor y) =>
        default(Ord<int>).Compare(x.OrderWeight, y.OrderWeight);

    public Task<int> CompareAsync(StatFactor x, StatFactor y) =>
        Task.FromResult(Compare(x, y));

    public bool Equals(StatFactor x, StatFactor y) =>
        x == y;

    public Task<bool> EqualsAsync(StatFactor x, StatFactor y) =>
        Task.FromResult(Equals(x, y));


    public int GetHashCode(StatFactor x) =>
        x.GetHashCode();

    public Task<int> GetHashCodeAsync(StatFactor x) =>
        Task.FromResult(GetHashCode(x));
}
