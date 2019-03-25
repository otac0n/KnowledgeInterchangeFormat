// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using KnowledgeInterchangeFormat.Expressions;
    using Xunit;

    public class EquivalenceTests
    {
        public static IEnumerable<object[]> ValidSentencePairs =>
            from s1 in ValidSentences
            from s2 in ValidSentences
            select s1.Concat(s2).ToArray();

        public static IEnumerable<object[]> ValidSentences => new List<object[]>
        {
            new object[] { new ConstantSentence(new Constant("A")) },
            new object[] { new ConstantSentence(new Constant("Bb")) },
            new object[] { new ConstantSentence(new Constant(new string('C', 1024))) },
        };

        [Theory]
        [MemberData(nameof(ValidSentences))]
        public void Constructor_WhenGivenANullLeftSentence_ThrowsArgumentNullException(Sentence right)
        {
            var exception = (ArgumentNullException)Record.Exception(() => new Equivalence(null, right));
            Assert.Equal("left", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(ValidSentences))]
        public void Constructor_WhenGivenANullRightSentence_ThrowsArgumentNullException(Sentence left)
        {
            var exception = (ArgumentNullException)Record.Exception(() => new Equivalence(left, null));
            Assert.Equal("right", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(ValidSentencePairs))]
        public void Constructor_WhenGivenValidSentences_CreatesASentenceWithTheSpecifiedSentence(Sentence left, Sentence right)
        {
            var subject = new Equivalence(left, right);

            Assert.Equal(left, subject.Left);
            Assert.Equal(right, subject.Right);
        }

        [Theory]
        [MemberData(nameof(ValidSentencePairs))]
        public void ToString_Always_ReturnsExpectedOutput(Sentence left, Sentence right)
        {
            var expected = $"(<=> {left} {right})";
            var subject = new Equivalence(left, right);

            Assert.Equal(expected, subject.ToString());
        }
    }
}
