// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using KnowledgeInterchangeFormat.Expressions;
    using Xunit;

    public class ConditionalTermTests
    {
        public static IEnumerable<object[]> ValidPairs => new List<object[]>
        {
            new object[] { Array.Empty<LogicalPair>() },
            new object[] { new LogicalPair[] { new LogicalPair(new ConstantSentence(new Constant("A")), new Constant("1")) } },
            new object[] { new LogicalPair[] { new LogicalPair(new ConstantSentence(new Constant("AbC")), new CharacterString("abc")) } },
        };

        [Fact]
        public void Constructor_WhenGivenNullPairs_ThrowsArgumentNullException()
        {
            var exception = (ArgumentNullException)Record.Exception(() => new ConditionalTerm(null));
            Assert.Equal("pairs", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(ValidPairs))]
        public void Constructor_WhenGivenValidSentences_CreatesASentenceWithTheSpecifiedConditionAndValues(LogicalPair[] pairs)
        {
            var subject = new ConditionalTerm(pairs);

            Assert.Equal(pairs, subject.Pairs);
        }

        [Theory]
        [MemberData(nameof(ValidPairs))]
        public void ToString_Always_ReturnsExpectedOutput(LogicalPair[] pairs)
        {
            var expected = $"(cond{string.Join(' ', pairs.Select(p => $" ({p})"))})";
            var subject = new ConditionalTerm(pairs);

            Assert.Equal(expected, subject.ToString());
        }
    }
}
