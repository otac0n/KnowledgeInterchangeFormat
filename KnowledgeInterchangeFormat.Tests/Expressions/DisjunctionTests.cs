// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using KnowledgeInterchangeFormat.Expressions;
    using Xunit;

    public class DisjunctionTests
    {
        public static IEnumerable<object[]> ValidSentences => new List<object[]>
        {
            new object[] { Array.Empty<Sentence>() },
            new object[] { new Sentence[] { new ConstantSentence(new Constant("A")) } },
            new object[] { new Sentence[] { new ConstantSentence(new Constant("A")), new ConstantSentence(new Constant("Bb")) } },
            new object[] { new Sentence[] { new ConstantSentence(new Constant("A")), new ConstantSentence(new Constant("Bb")), new ConstantSentence(new Constant(new string('C', 1024))) } },
        };

        [Fact]
        public void Constructor_WhenGivenANullCollection_ThrowsArgumentNullException()
        {
            var exception = (ArgumentNullException)Record.Exception(() => new Disjunction(null));
            Assert.Equal("disjuncts", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(ValidSentences))]
        public void Constructor_WhenGivenAValidCollection_CreatesASentenceWithTheSpecifiedDisjuncts(Sentence[] contents)
        {
            var subject = new Disjunction(contents);

            Assert.Equal(contents, subject.Disjuncts);
        }

        [Theory]
        [MemberData(nameof(ValidSentences))]
        public void ToString_Always_ReturnsExpectedOutput(Sentence[] contents)
        {
            var expected = $"(or{string.Concat(contents.Select(s => string.Concat($" {s}")))})";
            var subject = new Disjunction(contents);

            Assert.Equal(expected, subject.ToString());
        }
    }
}
