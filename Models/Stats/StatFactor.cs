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
}

public record AdditiveFactor : StatFactor
{
}

public record MultiplicativeFactor : StatFactor
{
}
