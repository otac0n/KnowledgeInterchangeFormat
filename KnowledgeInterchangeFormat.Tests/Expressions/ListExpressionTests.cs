// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using KnowledgeInterchangeFormat.Expressions;
    using Xunit;

    public class ListExpressionTests
    {
        public static IEnumerable<object[]> ValidItems => new List<object[]>
        {
            new object[] { new Expression[] { new CharacterReference('a') } },
            new object[] { new Expression[] { new CharacterReference('a'), new CharacterReference('b'), new CharacterReference('b') } },
            new object[] { new Expression[] { new CharacterString("abc") } },
            new object[] { new Expression[] { new CharacterString("a"), new ListExpression(new Expression[] { new CharacterReference('b') }), new CharacterReference('c') } },
            new object[] { new Expression[] { new Constant("A"), new CharacterReference('b'), new SequenceVariable("@C") } },
        };

        [Fact]
        public void Constructor_WhenGivenANullList_ThrowsArgumentNullException()
        {
            var exception = (ArgumentNullException)Record.Exception(() => new ListExpression(null));
            Assert.Equal("items", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(ValidItems))]
        public void Constructor_WhenGivenValidArguments_CreatesAnObjectWithTheSpecifiedProperties(Expression[] items)
        {
            var subject = new ListExpression(items);

            Assert.Equal(items, subject.Items);
        }

        [Theory]
        [MemberData(nameof(ValidItems))]
        public void ToString_Always_ReturnsExpectedOutput(Expression[] items)
        {
            var expected = $"({string.Join(' ', items.AsEnumerable())})";
            var subject = new ListExpression(items);

            Assert.Equal(expected, subject.ToString());
        }
    }
}
