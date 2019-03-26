// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using KnowledgeInterchangeFormat.Expressions;
    using Xunit;

    public class LogicalPairTests
    {
        public static IEnumerable<object[]> ValidConditions => new List<object[]>
        {
            new object[] { new ConstantSentence(new Constant("A")) },
            new object[] { new ConstantSentence(new Constant("Bb")) },
            new object[] { new ConstantSentence(new Constant(new string('C', 1024))) },
        };

        public static IEnumerable<object[]> ValidValues => new List<object[]>
        {
            new object[] { new SequenceVariable("@A") },
            new object[] { new IndividualVariable("?Bb") },
            new object[] { new Constant(new string('C', 1024)) },
        };

        public static IEnumerable<object[]> ValidArguments =>
            from s1 in ValidConditions
            from s2 in ValidValues
            select s1.Concat(s2).ToArray();

        [Theory]
        [MemberData(nameof(ValidConditions))]
        public void Constructor_WhenGivenANullValue_ThrowsArgumentNullException(Sentence condition)
        {
            var exception = (ArgumentNullException)Record.Exception(() => new LogicalPair(condition, null));
            Assert.Equal("value", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(ValidValues))]
        public void Constructor_WhenGivenANullCondition_ThrowsArgumentNullException(Term value)
        {
            var exception = (ArgumentNullException)Record.Exception(() => new LogicalPair(null, value));
            Assert.Equal("condition", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(ValidArguments))]
        public void Constructor_WhenGivenValidArguments_CreatesAnObjectWithTheSpecifiedProperties(Sentence condition, Term value)
        {
            var subject = new LogicalPair(condition, value);

            Assert.Equal(condition, subject.Condition);
            Assert.Equal(value, subject.Value);
        }

        [Theory]
        [MemberData(nameof(ValidArguments))]
        public void ToString_Always_ReturnsExpectedOutput(Sentence condition, Term value)
        {
            var expected = $"{condition} {value}";
            var subject = new LogicalPair(condition, value);

            Assert.Equal(expected, subject.ToString());
        }
    }
}
