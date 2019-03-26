// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using KnowledgeInterchangeFormat.Expressions;
    using Xunit;

    public class ExistentiallyQuantifiedSentenceTests
    {
        public static IEnumerable<object[]> ValidVariables => new List<object[]>
        {
            new object[] { new VariableSpecification[] { new VariableSpecification(new IndividualVariable("?A"), null) } },
            new object[] { new VariableSpecification[] { new VariableSpecification(new IndividualVariable("?B"), new Constant("Bb")) } },
            new object[] { new VariableSpecification[] { new VariableSpecification(new SequenceVariable("@C"), null), new VariableSpecification(new SequenceVariable("@D"), new Constant(new string('D', 1024))) } },
        };

        public static IEnumerable<object[]> ValidSentences => new List<object[]>
        {
            new object[] { new ConstantSentence(new Constant("A")) },
            new object[] { new ConstantSentence(new Constant("Bb")) },
            new object[] { new ConstantSentence(new Constant(new string('C', 1024))) },
        };

        public static IEnumerable<object[]> ValidSentencePairs =>
            from s1 in ValidVariables
            from s2 in ValidSentences
            select s1.Concat(s2).ToArray();

        [Theory]
        [MemberData(nameof(ValidVariables))]
        public void Constructor_WhenGivenANullQuantified_ThrowsArgumentNullException(VariableSpecification[] variables)
        {
            var exception = (ArgumentNullException)Record.Exception(() => new ExistentiallyQuantifiedSentence(variables, null));
            Assert.Equal("quantified", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(ValidSentences))]
        public void Constructor_WhenGivenNullVariables_ThrowsArgumentNullException(Sentence quantified)
        {
            var exception = (ArgumentNullException)Record.Exception(() => new ExistentiallyQuantifiedSentence(null, quantified));
            Assert.Equal("variables", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(ValidSentencePairs))]
        public void Constructor_WhenGivenValidArguments_CreatesAnObjectWithTheSpecifiedProperties(VariableSpecification[] variables, Sentence quantified)
        {
            var subject = new ExistentiallyQuantifiedSentence(variables, quantified);

            Assert.Equal(variables, subject.Variables);
            Assert.Equal(quantified, subject.Quantified);
        }

        [Theory]
        [MemberData(nameof(ValidSentencePairs))]
        public void ToString_Always_ReturnsExpectedOutput(VariableSpecification[] variables, Sentence quantified)
        {
            var expected = $"(exists ({string.Join(' ', variables.AsEnumerable())}) {quantified})";
            var subject = new ExistentiallyQuantifiedSentence(variables, quantified);

            Assert.Equal(expected, subject.ToString());
        }
    }
}
