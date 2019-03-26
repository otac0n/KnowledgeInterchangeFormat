// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using KnowledgeInterchangeFormat.Expressions;
    using Xunit;

    public class ExplicitRelationalSentenceTests
    {
        public static IEnumerable<object[]> ValidRelations => new List<object[]>
        {
            new object[] { new Constant("A") },
            new object[] { new IndividualVariable("?Bb") },
        };

        public static IEnumerable<object[]> ValidSequenceVariables => new List<object[]>
        {
            new object[] { null },
            new object[] { new SequenceVariable("@OK") },
        };

        public static IEnumerable<object[]> ValidArgs => new List<object[]>
        {
            new object[] { Array.Empty<IndividualVariable>() },
            new object[] { new[] { new IndividualVariable("?OK") } },
            new object[] { new[] { new IndividualVariable("?A"), new IndividualVariable("?Bb") } },
        };

        public static IEnumerable<object[]> ValidArguments =>
            from s1 in ValidRelations
            from s2 in ValidArgs
            from s3 in ValidSequenceVariables
            select s1.Concat(s2).Concat(s3).ToArray();

        public static IEnumerable<object[]> ValidArgumentsNoRelation =>
            from s1 in ValidArgs
            from s2 in ValidSequenceVariables
            select s1.Concat(s2).ToArray();

        public static IEnumerable<object[]> ValidArgumentsNoArgs =>
            from s1 in ValidRelations
            from s2 in ValidSequenceVariables
            select s1.Concat(s2).ToArray();

        [Theory]
        [MemberData(nameof(ValidArgumentsNoRelation))]
        public void Constructor_WhenGivenANullRelation_ThrowsArgumentNullException(IndividualVariable[] arguments, SequenceVariable sequenceVariable)
        {
            var exception = (ArgumentNullException)Record.Exception(() => new ExplicitRelationalSentence(null, arguments, sequenceVariable));
            Assert.Equal("relation", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(ValidArgumentsNoArgs))]
        public void Constructor_WhenGivenNullArguments_ThrowsArgumentNullException(Term relation, SequenceVariable sequenceVariable)
        {
            var exception = (ArgumentNullException)Record.Exception(() => new ExplicitRelationalSentence(relation, null, sequenceVariable));
            Assert.Equal("arguments", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(ValidArguments))]
        public void Constructor_WhenGivenValidSentences_CreatesASentenceWithTheSpecifiedSentencesAndConsequents(Term relation, IndividualVariable[] arguments, SequenceVariable sequenceVariable)
        {
            var subject = new ExplicitRelationalSentence(relation, arguments, sequenceVariable);

            Assert.Equal(relation, subject.Relation);
            Assert.Equal(arguments, subject.Arguments);
            Assert.Equal(sequenceVariable, subject.SequenceVariable);
        }

        [Theory]
        [MemberData(nameof(ValidArguments))]
        public void ToString_Always_ReturnsExpectedOutput(Term relation, IndividualVariable[] arguments, SequenceVariable sequenceVariable)
        {
            var expected = $"(holds {relation}{string.Concat(arguments.Cast<Variable>().Concat(new[] { sequenceVariable }.Where(v => v != null)).Select(arg => $" {arg}"))})";
            var subject = new ExplicitRelationalSentence(relation, arguments, sequenceVariable);

            Assert.Equal(expected, subject.ToString());
        }
    }
}
