// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using KnowledgeInterchangeFormat.Expressions;
    using Xunit;

    public class ConjunctionTests
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
            var exception = (ArgumentNullException)Record.Exception(() => new Conjunction(null));
            Assert.Equal("conjuncts", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(ValidSentences))]
        public void Constructor_WhenGivenAValidCollection_CreatesASentenceWithTheSpecifiedConjuncts(Sentence[] contents)
        {
            var subject = new Conjunction(contents);

            Assert.Equal(contents, subject.Conjuncts);
        }

        [Theory]
        [MemberData(nameof(ValidSentences))]
        public void ToString_Always_ReturnsExpectedOutput(Sentence[] contents)
        {
            var expected = $"(and{string.Concat(contents.Select(s => string.Concat($" {s}")))})";
            var subject = new Conjunction(contents);

            Assert.Equal(expected, subject.ToString());
        }
    }
}
