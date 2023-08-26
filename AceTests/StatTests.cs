using Ace.Models.Stats;
using FluentAssertions;
using LanguageExt;

namespace AceTests.Stats;

[TestFixture]
public class StatTests
{
  [TestCaseSource(typeof(StatFactorTestCases), nameof(StatFactorTestCases.GetCurrentStatTestCases))]
  public void GivenStatModifiers_WhenGetCurrent_ThenAddAndMultiply(StatModifier[] statFactors, float expectedResult)
  {
    var totalFactor = new Strength { BaseValue = 1f }.UpdateAndFetchCurrent(statFactors.ToSeq());
    totalFactor.Should().Be(expectedResult);
  }
}

public static class StatFactorTestCases
{
  public static IEnumerable<TestCaseData> GetCurrentStatTestCases()
  {
    yield return new TestCaseData(new StatModifier[] {
      new AdditiveFactor { TargetStat = typeof(Strength), Factor = 0f },
    }, 1f);
    yield return new TestCaseData(new StatModifier[] {
      new AdditiveFactor { TargetStat = typeof(Strength), Factor = 1f },
      new AdditiveFactor { TargetStat = typeof(Strength), Factor = 1f }
    }, 3f).SetName($"{nameof(StatTests.GivenStatModifiers_WhenGetCurrent_ThenAddAndMultiply)} Additive in layer");
    yield return new TestCaseData(new StatModifier[] {
      new AdditiveFactor { TargetStat = typeof(Strength), Factor = 1f },
      new AdditiveFactor { TargetStat = typeof(Willpower), Factor = 1f }
    }, 2f).SetName($"{nameof(StatTests.GivenStatModifiers_WhenGetCurrent_ThenAddAndMultiply)} Ignore wrong target stat");
    yield return new TestCaseData(new StatModifier[] {
      new AdditiveFactor { TargetStat = typeof(Strength), Factor = 1f },
      new MultiplicativeFactor { TargetStat = typeof(Strength), Factor = 1f }
    }, 4f).SetName($"{nameof(StatTests.GivenStatModifiers_WhenGetCurrent_ThenAddAndMultiply)} Additive and multiplicative in layer");
    yield return new TestCaseData(new StatModifier[] {
      new MultiplicativeFactor { TargetStat = typeof(Strength), Factor = 1f },
      new MultiplicativeFactor { TargetStat = typeof(Strength), Factor = 1f }
    }, 3f).SetName($"{nameof(StatTests.GivenStatModifiers_WhenGetCurrent_ThenAddAndMultiply)} Multiplicative in layer"); ;
    yield return new TestCaseData(new StatModifier[] {
      new AdditiveFactor { TargetStat = typeof(Strength), Factor = 1f },
      new AdditiveFactor { TargetStat = typeof(Strength), Factor = 1f },
      new MultiplicativeFactor { TargetStat = typeof(Strength), Factor = 1f },
      new MultiplicativeFactor { TargetStat = typeof(Strength), Factor = 1f }
    }, 9f).SetName($"{nameof(StatTests.GivenStatModifiers_WhenGetCurrent_ThenAddAndMultiply)} Multiple add/multiply in layer"); ;
    yield return new TestCaseData(new StatModifier[] {
      new AdditiveFactor { TargetStat = typeof(Strength), Factor = 1f, Layer = EnhancementLayer.Material },
      new AdditiveFactor { TargetStat = typeof(Strength), Factor = 1f, Layer = EnhancementLayer.Expertise }
    }, 4f).SetName($"{nameof(StatTests.GivenStatModifiers_WhenGetCurrent_ThenAddAndMultiply)} Add between layers");
    yield return new TestCaseData(new StatModifier[] {
      new AdditiveFactor { TargetStat = typeof(Strength), Factor = 1f, Layer = EnhancementLayer.Material },
      new MultiplicativeFactor { TargetStat = typeof(Strength), Factor = 1f, Layer = EnhancementLayer.Expertise }
    }, 4f).SetName($"{nameof(StatTests.GivenStatModifiers_WhenGetCurrent_ThenAddAndMultiply)} Add and multiply between layers");
    yield return new TestCaseData(new StatModifier[] {
      new MultiplicativeFactor { TargetStat = typeof(Strength), Factor = 1f, Layer = EnhancementLayer.Material },
      new MultiplicativeFactor { TargetStat = typeof(Strength), Factor = 1f, Layer = EnhancementLayer.Expertise }
    }, 4f).SetName($"{nameof(StatTests.GivenStatModifiers_WhenGetCurrent_ThenAddAndMultiply)} Multiply between layers");
    yield return new TestCaseData(new StatModifier[] {
      new AdditiveFactor { TargetStat = typeof(Strength), Factor = 1f, Layer = EnhancementLayer.Material },
      new AdditiveFactor { TargetStat = typeof(Strength), Factor = 1f, Layer = EnhancementLayer.Expertise },
      new MultiplicativeFactor { TargetStat = typeof(Strength), Factor = 1f, Layer = EnhancementLayer.Material },
      new MultiplicativeFactor { TargetStat = typeof(Strength), Factor = 1f, Layer = EnhancementLayer.Expertise }
    }, 16f).SetName($"{nameof(StatTests.GivenStatModifiers_WhenGetCurrent_ThenAddAndMultiply)} Add and multiply within and between layers");
    yield return new TestCaseData(new StatModifier[] {
      new AdditiveFactor { TargetStat = typeof(Strength), Factor = 1f, Layer = EnhancementLayer.Material },
      new AdditiveFactor { TargetStat = typeof(Strength), Factor = 1f, Layer = EnhancementLayer.Expertise },
      new AdditiveFactor { TargetStat = typeof(Strength), Factor = 1f, Layer = EnhancementLayer.Enchantment },
      new AdditiveFactor { TargetStat = typeof(Strength), Factor = 1f, Layer = EnhancementLayer.Divine },
      new AdditiveFactor { TargetStat = typeof(Strength), Factor = 1f, Layer = EnhancementLayer.Enigmatic },
      new MultiplicativeFactor { TargetStat = typeof(Strength), Factor = 1f, Layer = EnhancementLayer.Material },
      new MultiplicativeFactor { TargetStat = typeof(Strength), Factor = 1f, Layer = EnhancementLayer.Expertise },
      new MultiplicativeFactor { TargetStat = typeof(Strength), Factor = 1f, Layer = EnhancementLayer.Enchantment },
      new MultiplicativeFactor { TargetStat = typeof(Strength), Factor = 1f, Layer = EnhancementLayer.Divine },
      new MultiplicativeFactor { TargetStat = typeof(Strength), Factor = 1f, Layer = EnhancementLayer.Enigmatic },
    }, 1024f).SetName($"{nameof(StatTests.GivenStatModifiers_WhenGetCurrent_ThenAddAndMultiply)} Add and multiply between all layers - for fun");
  }
}

