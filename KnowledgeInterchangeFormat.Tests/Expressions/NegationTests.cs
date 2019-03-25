// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using KnowledgeInterchangeFormat.Expressions;
    using Xunit;

    public class NegationTests
    {
        public static IEnumerable<object[]> ValidSentences => new List<object[]>
        {
            new object[] { new ConstantSentence(new Constant("A")) },
            new object[] { new ConstantSentence(new Constant("Bb")) },
            new object[] { new ConstantSentence(new Constant(new string('C', 1024))) },
        };

        [Fact]
        public void Constructor_WhenGivenANullSentence_ThrowsArgumentNullException()
        {
            var exception = (ArgumentNullException)Record.Exception(() => new Negation(null));
            Assert.Equal("negated", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(ValidSentences))]
        public void Constructor_WhenGivenAValidSentence_CreatesASentenceWithTheSpecifiedSentence(Sentence contents)
        {
            var subject = new Negation(contents);

            Assert.Equal(contents, subject.Negated);
        }

        [Theory]
        [MemberData(nameof(ValidSentences))]
        public void ToString_Always_ReturnsExpectedOutput(Sentence contents)
        {
            var expected = $"(not {contents})";
            var subject = new Negation(contents);

            Assert.Equal(expected, subject.ToString());
        }
    }
}
