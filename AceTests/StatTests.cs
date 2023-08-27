 namespace AceTests.Stats;

[TestFixture]
public class StatTests
{
  [TestCaseSource(typeof(StatFactorTestCases), nameof(StatFactorTestCases.GetCurrentStatTestCases))]
  public void GivenStatModifiers_WhenGetCurrent_ThenAddAndMultiply(StatModifier[] statFactors, float expectedResult)
  {
    var baseValue = 1f;
    var totalFactor = new Strength { BaseValue = baseValue }.UpdateAndFetchCurrent(statFactors.ToSeq());
    totalFactor.GetCurrent().Should().Be(expectedResult + baseValue);
  }

  [TestCaseSource(typeof(SecondaryStatFactorTestCases), nameof(SecondaryStatFactorTestCases.GetSecondaryStatFactorTestCases))]
  public void GivenSecondaryStatModifiers_WhenGetCurrent_ThenAddAndMultiply(SecondaryStatFactor[] statFactors, float expectedResult)
  {
    var baseValue = 1f;
    var primaryStats = new PrimaryStats { Strength = new Strength { BaseValue = 100f }, Agility = new Agility { BaseValue = 100f } };
    var speed = new Speed { BaseValue = baseValue };
    var newSpeed = speed.DeriveFromPrimaryStats(statFactors.ToSeq(), primaryStats);
    newSpeed.GetCurrent().Should().Be(expectedResult + baseValue);
  }
}


public static class SecondaryStatFactorTestCases
{
  public static IEnumerable<TestCaseData> GetSecondaryStatFactorTestCases()
  {
    yield return new TestCaseData(new SecondaryStatFactor[]
    {
       new SecondaryStatFactor(StatType.Speed, EnhancementLayer.Material, 0f, StatType.Strength),
    }, 0f);
    yield return new TestCaseData(new SecondaryStatFactor[]
    {
       new SecondaryStatFactor(StatType.Speed, EnhancementLayer.Material, 1f, StatType.Strength),
    }, 100f).SetName($"{nameof(StatTests.GivenSecondaryStatModifiers_WhenGetCurrent_ThenAddAndMultiply)} One stat one layer");
    yield return new TestCaseData(new SecondaryStatFactor[]
    {
       new SecondaryStatFactor(StatType.Speed, EnhancementLayer.Material, 1f, StatType.Strength),
       new SecondaryStatFactor(StatType.Speed, EnhancementLayer.Material, 1f, StatType.Agility),
    }, 200f).SetName($"{nameof(StatTests.GivenSecondaryStatModifiers_WhenGetCurrent_ThenAddAndMultiply)} Two stats one layer");
    yield return new TestCaseData(new SecondaryStatFactor[]
    {
       new SecondaryStatFactor(StatType.Speed, EnhancementLayer.Material, 1f, StatType.Strength),
       new SecondaryStatFactor(StatType.Speed, EnhancementLayer.Expertise, 1f, StatType.Agility),
    }, 200f).SetName($"{nameof(StatTests.GivenSecondaryStatModifiers_WhenGetCurrent_ThenAddAndMultiply)} Two stats two layers");
    yield return new TestCaseData(new SecondaryStatFactor[]
    {
       new SecondaryStatFactor(StatType.Speed, EnhancementLayer.Material, -1f, StatType.Strength),
       new SecondaryStatFactor(StatType.Speed, EnhancementLayer.Material, 1f, StatType.Agility),
    }, 0f).SetName($"{nameof(StatTests.GivenSecondaryStatModifiers_WhenGetCurrent_ThenAddAndMultiply)} Negative stat on different stat same layer");
    yield return new TestCaseData(new SecondaryStatFactor[]
    {
       new SecondaryStatFactor(StatType.Speed, EnhancementLayer.Expertise, -0.25f, StatType.Strength),
       new SecondaryStatFactor(StatType.Speed, EnhancementLayer.Material, 1f, StatType.Agility),
    }, 75f).SetName($"{nameof(StatTests.GivenSecondaryStatModifiers_WhenGetCurrent_ThenAddAndMultiply)} Negative stat on different stat other layer");
    yield return new TestCaseData(new SecondaryStatFactor[]
    {
       new SecondaryStatFactor(StatType.Speed, EnhancementLayer.Expertise, -0.25f, StatType.Strength),
       new SecondaryStatFactor(StatType.Speed, EnhancementLayer.Material, -0.25f, StatType.Agility),
    }, 0f).SetName($"{nameof(StatTests.GivenSecondaryStatModifiers_WhenGetCurrent_ThenAddAndMultiply)} two negative stats on different stats on two layers");
    yield return new TestCaseData(new SecondaryStatFactor[]
    {
       new SecondaryStatFactor(StatType.Speed, EnhancementLayer.Material, 1f, StatType.Strength),
       new SecondaryStatFactor(StatType.Speed, EnhancementLayer.Material, -0.25f, StatType.Strength),
    },75f).SetName($"{nameof(StatTests.GivenSecondaryStatModifiers_WhenGetCurrent_ThenAddAndMultiply)} positive and negative on same stat on same layer");
    yield return new TestCaseData(new SecondaryStatFactor[]
   {
       new SecondaryStatFactor(StatType.Speed, EnhancementLayer.Material, 1f, StatType.Strength),
       new SecondaryStatFactor(StatType.Speed, EnhancementLayer.Expertise, -0.25f, StatType.Strength),
   }, 75f).SetName($"{nameof(StatTests.GivenSecondaryStatModifiers_WhenGetCurrent_ThenAddAndMultiply)} positive and negative on same stat on differnt layers");
  }
}


public static class StatFactorTestCases
{
  public static IEnumerable<TestCaseData> GetCurrentStatTestCases()
  {
    yield return new TestCaseData(new StatModifier[] {
    new AdditiveFactor (StatType.Strength, EnhancementLayer.Material, 0f),
    }, 1f);

    yield return new TestCaseData(new StatModifier[] {
    new AdditiveFactor (StatType.Strength, EnhancementLayer.Material, 1f),
    new AdditiveFactor (StatType.Strength, EnhancementLayer.Material, 1f)
    }, 3f).SetName($"{nameof(StatTests.GivenStatModifiers_WhenGetCurrent_ThenAddAndMultiply)} Additive in layer");

    yield return new TestCaseData(new StatModifier[] {
    new AdditiveFactor (StatType.Strength, EnhancementLayer.Material, 1f),
    new AdditiveFactor (StatType.Willpower, EnhancementLayer.Material, 1f)
    }, 2f).SetName($"{nameof(StatTests.GivenStatModifiers_WhenGetCurrent_ThenAddAndMultiply)} Ignore wrong target stat");

    yield return new TestCaseData(new StatModifier[] {
    new AdditiveFactor (StatType.Strength, EnhancementLayer.Material, 1f),
    new MultiplicativeFactor (StatType.Strength, EnhancementLayer.Material, 1f)
    }, 4f).SetName($"{nameof(StatTests.GivenStatModifiers_WhenGetCurrent_ThenAddAndMultiply)} Additive and multiplicative in layer");

    yield return new TestCaseData(new StatModifier[] {
    new MultiplicativeFactor (StatType.Strength, EnhancementLayer.Material, 1f),
    new MultiplicativeFactor (StatType.Strength, EnhancementLayer.Material, 1f)
    }, 3f).SetName($"{nameof(StatTests.GivenStatModifiers_WhenGetCurrent_ThenAddAndMultiply)} Multiplicative in layer");

    yield return new TestCaseData(new StatModifier[] {
    new AdditiveFactor (StatType.Strength, EnhancementLayer.Material, 1f),
    new AdditiveFactor (StatType.Strength, EnhancementLayer.Material, 1f),
    new MultiplicativeFactor (StatType.Strength, EnhancementLayer.Material, 1f),
    new MultiplicativeFactor (StatType.Strength, EnhancementLayer.Material, 1f)
    }, 9f).SetName($"{nameof(StatTests.GivenStatModifiers_WhenGetCurrent_ThenAddAndMultiply)} Multiple add/multiply in layer");

    yield return new TestCaseData(new StatModifier[] {
    new AdditiveFactor (StatType.Strength, EnhancementLayer.Material, 1f),
    new AdditiveFactor (StatType.Strength, EnhancementLayer.Expertise, 1f)
    }, 4f).SetName($"{nameof(StatTests.GivenStatModifiers_WhenGetCurrent_ThenAddAndMultiply)} Add between layers");

    yield return new TestCaseData(new StatModifier[] {
    new AdditiveFactor (StatType.Strength, EnhancementLayer.Material, 1f),
    new MultiplicativeFactor (StatType.Strength, EnhancementLayer.Expertise, 1f)
    }, 4f).SetName($"{nameof(StatTests.GivenStatModifiers_WhenGetCurrent_ThenAddAndMultiply)} Add and multiply between layers");

    yield return new TestCaseData(new StatModifier[] {
    new MultiplicativeFactor (StatType.Strength, EnhancementLayer.Material, 1f),
    new MultiplicativeFactor (StatType.Strength, EnhancementLayer.Expertise, 1f)
    }, 4f).SetName($"{nameof(StatTests.GivenStatModifiers_WhenGetCurrent_ThenAddAndMultiply)} Multiply between layers");

    yield return new TestCaseData(new StatModifier[] {
    new AdditiveFactor (StatType.Strength, EnhancementLayer.Material, 1f),
    new AdditiveFactor (StatType.Strength, EnhancementLayer.Expertise, 1f),
    new MultiplicativeFactor (StatType.Strength, EnhancementLayer.Material, 1f),
    new MultiplicativeFactor (StatType.Strength, EnhancementLayer.Expertise, 1f)
    }, 16f).SetName($"{nameof(StatTests.GivenStatModifiers_WhenGetCurrent_ThenAddAndMultiply)} Add and multiply within and between layers");

    yield return new TestCaseData(new StatModifier[] {
    new AdditiveFactor (StatType.Strength, EnhancementLayer.Material, 1f),
    new AdditiveFactor (StatType.Strength, EnhancementLayer.Expertise, 1f),
    new AdditiveFactor (StatType.Strength, EnhancementLayer.Enchantment, 1f),
    new AdditiveFactor (StatType.Strength, EnhancementLayer.Divine, 1f),
    new AdditiveFactor (StatType.Strength, EnhancementLayer.Enigmatic, 1f),
    new MultiplicativeFactor (StatType.Strength, EnhancementLayer.Material, 1f),
    new MultiplicativeFactor (StatType.Strength, EnhancementLayer.Expertise, 1f),
    new MultiplicativeFactor (StatType.Strength, EnhancementLayer.Enchantment, 1f),
    new MultiplicativeFactor (StatType.Strength, EnhancementLayer.Divine, 1f),
    new MultiplicativeFactor (StatType.Strength, EnhancementLayer.Enigmatic, 1f),
    }, 1024f).SetName($"{nameof(StatTests.GivenStatModifiers_WhenGetCurrent_ThenAddAndMultiply)} Add and multiply between all layers - for fun");

  }
}

