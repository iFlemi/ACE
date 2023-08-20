using Ace.Models.Stats;
using FluentAssertions;

namespace AceTests.Stats;

[TestFixture]
public class StatTests
{
  [TestCaseSource(typeof(StatFactorTestCases), nameof(StatFactorTestCases.GetCurrentStatTestCases))]
  public void GivenStatFactors_WhenGetFactor_ThenAddAndMultiply(StatFactor[] statFactors, float expectedResult)
  {
    var totalFactor = new Strength().GetFactor(statFactors);
    totalFactor.Should().Be(expectedResult);
  }
}

public static class StatFactorTestCases
{
  public static IEnumerable<TestCaseData> GetCurrentStatTestCases()
  {
    yield return new TestCaseData(new StatFactor[] {
      new AdditiveFactor { SourceStat = typeof(Strength), TargetStat = typeof(Strength), Value = 1f },
      new AdditiveFactor { SourceStat = typeof(Strength), TargetStat = typeof(Strength), Value = 1f }
    }, 3f);
    yield return new TestCaseData(
      new StatFactor[] {
      new AdditiveFactor { SourceStat = typeof(Strength), TargetStat = typeof(Strength), Value = 1f },
      new MultiplicativeFactor { SourceStat = typeof(Strength), TargetStat = typeof(Strength), Value = 1f }
    }, 4f);
    yield return new TestCaseData(
      new StatFactor[] {
      new MultiplicativeFactor { SourceStat = typeof(Strength), TargetStat = typeof(Strength), Value = 1f },
      new MultiplicativeFactor { SourceStat = typeof(Strength), TargetStat = typeof(Strength), Value = 1f }
    }, 3f);
    yield return new TestCaseData(
      new StatFactor[] {
      new AdditiveFactor { SourceStat = typeof(Strength), TargetStat = typeof(Strength), Value = .5f },
      new AdditiveFactor { SourceStat = typeof(Strength), TargetStat = typeof(Strength), Value = .5f },
      new MultiplicativeFactor { SourceStat = typeof(Strength), TargetStat = typeof(Strength), Value = .5f },
      new MultiplicativeFactor { SourceStat = typeof(Strength), TargetStat = typeof(Strength), Value = .5f }
    }, 4f);
    yield return new TestCaseData(new StatFactor[] {
      new AdditiveFactor { SourceStat = typeof(Strength), TargetStat = typeof(Strength), Value = 1f, Layer = EnhancementLayer.Material },
      new AdditiveFactor { SourceStat = typeof(Strength), TargetStat = typeof(Strength), Value = 1f, Layer = EnhancementLayer.Expertise }
    }, 4f);
    yield return new TestCaseData(new StatFactor[] {
      new AdditiveFactor { SourceStat = typeof(Strength), TargetStat = typeof(Strength), Value = 1f, Layer = EnhancementLayer.Material },
      new MultiplicativeFactor { SourceStat = typeof(Strength), TargetStat = typeof(Strength), Value = 1f, Layer = EnhancementLayer.Expertise }
    }, 4f);
    yield return new TestCaseData(new StatFactor[] {
      new MultiplicativeFactor { SourceStat = typeof(Strength), TargetStat = typeof(Strength), Value = 1f, Layer = EnhancementLayer.Material },
      new MultiplicativeFactor { SourceStat = typeof(Strength), TargetStat = typeof(Strength), Value = 1f, Layer = EnhancementLayer.Expertise }
    }, 4f);
    yield return new TestCaseData(
    new StatFactor[] {
      new AdditiveFactor { SourceStat = typeof(Strength), TargetStat = typeof(Strength), Value = .5f, Layer = EnhancementLayer.Material },
      new AdditiveFactor { SourceStat = typeof(Strength), TargetStat = typeof(Strength), Value = .5f, Layer = EnhancementLayer.Expertise },
      new MultiplicativeFactor { SourceStat = typeof(Strength), TargetStat = typeof(Strength), Value = .5f, Layer = EnhancementLayer.Material },
      new MultiplicativeFactor { SourceStat = typeof(Strength), TargetStat = typeof(Strength), Value = .5f, Layer = EnhancementLayer.Expertise }
  }, 5.0625f);
  }
}

