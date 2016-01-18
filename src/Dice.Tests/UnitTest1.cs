using System;
using System.Collections.Generic;
using Xtof.RandomVariables;
using Xunit;

namespace Dice.Tests
{
    public class UnitTest1
    {
        [Theory, MemberData("ReductionTestData")]
        public void ReductionTests(Rational actual, Rational expected)
        {
            Assert.Equal(expected.Numerator, actual.Numerator);
            Assert.Equal(expected.Denominator, actual.Denominator);
        }

        public static readonly IEnumerable<object[]> ReductionTestData = new[] {
            new object[] {new Rational(1, 2), new Rational(1, 2) },
            new object[] {new Rational(-1, -2), new Rational(1, 2) },
            new object[] {new Rational(2, 4), new Rational(1, 2) },

            new object[] {new Rational(-1, 2), new Rational(-1, 2) },
            new object[] {new Rational(1, -2), new Rational(-1, 2) },

            new object[] {new Rational(1, 2) + new Rational(1, 3), new Rational(5, 6) }
        };

        [Theory, MemberData("ConversionTestData")]
        public void ConversionTests(Rational data, decimal expected)
        {
            // var actual = data.ToDecimal(null);
            // var actual = Convert.ToDecimal((object)data);
            var actual = (decimal)data;
            Assert.Equal(expected, actual);
        }

        public static readonly IEnumerable<object[]> ConversionTestData = new[] {
            new object[] {new Rational(1, 3), 1/3m },
            new object[] {new Rational(1, -3), -1/3m }
        };
    }
}
