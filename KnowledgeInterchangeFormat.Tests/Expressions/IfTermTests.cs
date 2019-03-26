// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using KnowledgeInterchangeFormat.Expressions;
    using Xunit;

    public class IfTermTests
    {
        public static IEnumerable<object[]> ValidPairs => new List<object[]>
        {
            new object[] { new LogicalPair[] { new LogicalPair(new ConstantSentence(new Constant("A")), new Constant("1")) } },
            new object[] { new LogicalPair[] { new LogicalPair(new ConstantSentence(new Constant("AbC")), new CharacterString("abc")) } },
        };

        public static IEnumerable<object[]> ValidDefaults => new List<object[]>
        {
            new object[] { null },
            new object[] { new IndividualVariable("?Ab") },
        };

        public static IEnumerable<object[]> ValidArguments =>
            from s1 in ValidPairs
            from s2 in ValidDefaults
            select s1.Concat(s2).ToArray();

        [Theory]
        [MemberData(nameof(ValidDefaults))]
        public void Constructor_WhenGivenNullPairs_ThrowsArgumentNullException(Term @default)
        {
            var exception = (ArgumentNullException)Record.Exception(() => new IfTerm(null, @default));
            Assert.Equal("pairs", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(ValidArguments))]
        public void Constructor_WhenGivenValidArguments_CreatesAnObjectWithTheSpecifiedProperties(LogicalPair[] pairs, Term @default)
        {
            var subject = new IfTerm(pairs, @default);

            Assert.Equal(pairs, subject.Pairs);
            Assert.Equal(@default, subject.Default);
        }

        [Theory]
        [MemberData(nameof(ValidArguments))]
        public void ToString_Always_ReturnsExpectedOutput(LogicalPair[] pairs, Term @default)
        {
            var expected = $"(if {string.Join(' ', pairs.AsEnumerable())}{(@default == null ? "" : $" {@default}")})";
            var subject = new IfTerm(pairs, @default);

            Assert.Equal(expected, subject.ToString());
        }
    }
}
