using Ace.Interfaces;
using NSubstitute.Exceptions;

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
        yield return new TestCaseData(30, 50, 30, 20, 0f).Returns(new DamageAllocation(20, 30, 20));
        yield return new TestCaseData(50, 50, 30, 20, 0f).Returns(new DamageAllocation(0, 30, 20));
        yield return new TestCaseData(60, 50, 30, 20, 0f).Returns(new DamageAllocation(0, 20, 20));
        yield return new TestCaseData(80, 50, 30, 20, 0f).Returns(new DamageAllocation(0, 0, 20));
        yield return new TestCaseData(100, 50, 30, 20, 0f).Returns(new DamageAllocation(0, 0, 0));
        yield return new TestCaseData(150, 50, 30, 20, 0f).Returns(new DamageAllocation(0, 0, 0));
    }

    [TestCaseSource(nameof(TestCases))]
    public DamageAllocation TestStandardDamageAllocation(int damage, int shield, int stamina, int health, float damageResistance)
    {
        var sut = new StandardDamageAllocator();
        return sut.AllocateDamage(damage, shield, stamina, health, damageResistance);
    }

    private static IEnumerable<TestCaseData> SoftenedShieldTestCases()
    {
        yield return new TestCaseData(30, 50, 30, 20, 0.5f).Returns(new DamageAllocation(35, 15, 20));
        yield return new TestCaseData(40, 50, 30, 20, 0.5f).Returns(new DamageAllocation(30, 10, 20));
        yield return new TestCaseData(60, 50, 30, 20, 0.5f).Returns(new DamageAllocation(20, 0, 20));
        yield return new TestCaseData(80, 50, 30, 20, 0.5f).Returns(new DamageAllocation(10, 0, 10));
        yield return new TestCaseData(90, 50, 30, 20, 0.5f).Returns(new DamageAllocation(5, 0, 5));
        yield return new TestCaseData(100, 50, 30, 20, 0.5f).Returns(new DamageAllocation(0, 0, 0));
        yield return new TestCaseData(150, 50, 30, 20, 0.5f).Returns(new DamageAllocation(0, 0, 0));
    }

    [TestCaseSource(nameof(SoftenedShieldTestCases))]
    public DamageAllocation TestSoftenedShieldDamageAllocation(int damage, int shield, int stamina, int health, float shieldHardness, float damageResistance)
    {
        var sut = new SoftenedShieldDamageAllocator(shieldHardness);
        return sut.AllocateDamage(damage, shield, stamina, health, damageResistance);
    }
}


