using System;
using System.Collections.Generic;
using Xtof.RandomVariables;
using Xunit;

namespace Dice.Tests
{
    public class UnitTest1
    {
        [Theory, MemberData("ReductionTestsData")]
        public void ReductionTests(Rational actual, Rational expected)
        {
            Assert.Equal(expected.Numerator, actual.Numerator);
            Assert.Equal(expected.Denominator, actual.Denominator);
        }

        public static readonly IEnumerable<object[]> ReductionTestsData = new[] {
            new object[] {new Rational(1, 2), new Rational(1, 2) },
            new object[] {new Rational(-1, -2), new Rational(1, 2) },
            new object[] {new Rational(2, 4), new Rational(1, 2) },

            new object[] {new Rational(-1, 2), new Rational(-1, 2) },
            new object[] {new Rational(1, -2), new Rational(-1, 2) },

            new object[] {new Rational(1, 2) + new Rational(1, 3), new Rational(5, 6) }
        };

        [Theory, MemberData("ConversionTestsData")]
        public void ConversionTests(Rational data, Decimal expected)
        {
            var actual = Convert.ToDecimal(data);
            Assert.Equal(expected, actual);
        }

        public static readonly IEnumerable<object[]> ConversionTestsData = new[] {
            new object[] {new Rational(1, 3), 1/3m },
            new object[] {new Rational(1, -3), -1/3m }
        };
    }
}
