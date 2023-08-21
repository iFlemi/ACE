using Ace.Interfaces;

namespace AceTests.Interfaces;


[TestFixture]
public class DamageAllocatorTests
{

    [SetUp]
    public void Setup()
    {
    }

    private static IEnumerable<TestCaseData> TestCases()
    {
        yield return new TestCaseData(30, 50, 30, 20).Returns((20, 30, 20));
        yield return new TestCaseData(50, 50, 30, 20).Returns((0, 30, 20));
        yield return new TestCaseData(60, 50, 30, 20).Returns((0, 20, 20));
        yield return new TestCaseData(80, 50, 30, 20).Returns((0, 0, 20));
        yield return new TestCaseData(100, 50, 30, 20).Returns((0, 0, 0));
        yield return new TestCaseData(150, 50, 30, 20).Returns((0, 0, 0));
    }

    [TestCaseSource(nameof(TestCases))]
    public (int, int, int) TestStandardDamageAllocation(int damage, int shield, int stamina, int health)
    {
        var sut = new StandardDamageAllocator();

        var result = sut.AllocateDamage(damage, shield, stamina, health);
        return result;
    }

    private static IEnumerable<TestCaseData> SoftenedShieldTestCases()
    {
        yield return new TestCaseData(30, 50, 30, 20, 0.5f).Returns((35, 15, 20));
        yield return new TestCaseData(40, 50, 30, 20, 0.5f).Returns((30, 10, 20));
        yield return new TestCaseData(60, 50, 30, 20, 0.5f).Returns((20, 0, 20));
        yield return new TestCaseData(80, 50, 30, 20, 0.5f).Returns((10, 0, 10));
        yield return new TestCaseData(90, 50, 30, 20, 0.5f).Returns((5, 0, 5));
        yield return new TestCaseData(100, 50, 30, 20, 0.5f).Returns((0, 0, 0));
        yield return new TestCaseData(150, 50, 30, 20, 0.5f).Returns((0, 0, 0));
    }

    [TestCaseSource(nameof(SoftenedShieldTestCases))]
    public (int, int, int) TestSoftenedShieldDamageAllocation(int damage, int shield, int stamina, int health, float shieldHardness)
    {
        var sut = new SoftenedShieldDamageAllocator(shieldHardness);

        var result = sut.AllocateDamage(damage, shield, stamina, health);
        return result;
    }
}


