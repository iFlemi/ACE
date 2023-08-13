using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Models.Tests;


[TestFixture]
public class PartyMemberTests
{
    private static IBattler _sut;

    [SetUp]
    public void Setup()
    {
        _sut = Substitute.For<IBattler>();
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
    public (int, int, int) TestDamageAllocation(int damage, int shield, int stamina, int health)
    {
        return _sut.AllocateDamage(damage, shield, stamina, health);
    }

}

