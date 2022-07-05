using System;
using Xunit;
using ExpressionParser;

namespace ExpressionParserTests
{
    public class UnitTest1
    {
        [Theory()]
        [InlineData("1 + 3", 4, true)]      // plus
        [InlineData("1 - 2", -1, true)]     // minus
        [InlineData("1 * 2", 2, true)]      // times
        [InlineData("1 / 2", 0, true)]      // fraction
        [InlineData("2 / 1", 2, true)]      // divide
        [InlineData("1+2", 3, true)]        // no spaces
        [InlineData("1 + 2 + 3", 6, true)]  // more than two numbers
        [InlineData("(1 + 3) * 2", 8, true)]    // brackets
        [InlineData("4 + (12 /(1 * 2))", 10, true)]     // multiple nested brackets
        [InlineData("(1 + (12 * 2)", 0, false)]         // missing bracket
        [InlineData("2 + 1 - 4 * 5 / 5", -1, true)]     // multiple numbers with different signs
        [InlineData("22 + (12 * 3 + 3)", 61, true)]     // bigger values
        [InlineData("", 0, false)]          // empty string
        [InlineData("1 + 3 +", 0, false)]   // symbol at the end
        [InlineData("5 / 0", 0, false)]     // divide by 0
        [InlineData("1 ++ 3", 0, false)]    // too many symbols
        [InlineData("() + 5", 0, false)]    // empty brackets
        [InlineData("1          +           2", 3, true)]   // lots of white space
        [InlineData("(25 + 25) / (5 * 1)", 10, true)]       // unnested brackets
        [InlineData("(25 + 25) / (5 * 1) + (1*2)", 12, true)]   // multiple unnested brackets
        [InlineData("1", 1, true)]      // single digit
        [InlineData("((25 + 25) / (5 * 1) - 1)", 9, true)]       // multiple groups of unnested and nested brackets
        [InlineData("((((25 + 25) / (5 * 1))))", 10, true)]       // lots of brackets
        [InlineData("5 + h", 0, false)]       // contains letters
        public void Test1(string expression, int expected, bool expectedValid)
        {
            SumParser test = new SumParser();
            int val = 0;
            bool isValid = test.EvaluateExpression(expression, out val);

            Assert.Equal(expectedValid, isValid);
            Assert.Equal(expected, val);
        }
    }
}

