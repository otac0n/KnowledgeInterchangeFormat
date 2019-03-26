// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using KnowledgeInterchangeFormat.Expressions;
    using Xunit;

    public class PartialRelationDefinitionTests
    {
        public static IEnumerable<object[]> ValidSentences => new List<object[]>
        {
            new object[] { new ConstantSentence(new Constant("A")) },
            new object[] { new ConstantSentence(new Constant("Bb")) },
            new object[] { new ConstantSentence(new Constant(new string('C', 1024))) },
        };

        public static IEnumerable<object[]> ValidConstants => new List<object[]>
        {
            new object[] { new Constant("A") },
            new object[] { new Constant("Bb") },
            new object[] { new Constant(new string('C', 1024)) },
        };

        public static IEnumerable<object[]> ValidStrings => new List<object[]>
        {
            new object[] { null },
            new object[] { new CharacterString("OK Description") },
        };

        public static IEnumerable<object[]> ValidSequenceVariables => new List<object[]>
        {
            new object[] { null },
            new object[] { new SequenceVariable("@OK") },
        };

        public static IEnumerable<object[]> ValidParameters => new List<object[]>
        {
            new object[] { Array.Empty<IndividualVariable>() },
            new object[] { new[] { new IndividualVariable("?OK") } },
            new object[] { new[] { new IndividualVariable("?A"), new IndividualVariable("?Bb") } },
        };

        public static IEnumerable<object[]> ValidArguments =>
            from s1 in ValidConstants
            from s2 in ValidStrings
            from s3 in ValidParameters
            from s4 in ValidSequenceVariables
            from s5 in ValidSentences
            select s1.Concat(s2).Concat(s3).Concat(s4).Concat(s5).ToArray();

        public static IEnumerable<object[]> ValidArgumentsNoConstant =>
            from s1 in ValidStrings
            from s2 in ValidParameters
            from s3 in ValidSequenceVariables
            from s4 in ValidSentences
            select s1.Concat(s2).Concat(s3).Concat(s4).ToArray();

        public static IEnumerable<object[]> ValidArgumentsNoParameters =>
            from s1 in ValidConstants
            from s2 in ValidStrings
            from s3 in ValidSequenceVariables
            from s4 in ValidSentences
            select s1.Concat(s2).Concat(s3).Concat(s4).ToArray();

        public static IEnumerable<object[]> ValidArgumentsNoSentence =>
            from s1 in ValidConstants
            from s2 in ValidStrings
            from s3 in ValidParameters
            from s4 in ValidSequenceVariables
            select s1.Concat(s2).Concat(s3).Concat(s4).ToArray();

        [Theory]
        [MemberData(nameof(ValidArgumentsNoConstant))]
        public void Constructor_WhenGivenANullConstant_ThrowsArgumentNullException(CharacterString description, IndividualVariable[] parameters, SequenceVariable sequenceVariable, Sentence sentence)
        {
            var exception = (ArgumentNullException)Record.Exception(() => new PartialRelationDefinition(null, description, parameters, sequenceVariable, sentence));
            Assert.Equal("constant", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(ValidArgumentsNoSentence))]
        public void Constructor_WhenGivenANullSentence_ThrowsArgumentNullException(Constant constant, CharacterString description, IndividualVariable[] parameters, SequenceVariable sequenceVariable)
        {
            var exception = (ArgumentNullException)Record.Exception(() => new PartialRelationDefinition(constant, description, parameters, sequenceVariable, null));
            Assert.Equal("sentence", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(ValidArgumentsNoParameters))]
        public void Constructor_WhenGivenNullParameters_ThrowsArgumentNullException(Constant constant, CharacterString description, SequenceVariable sequenceVariable, Sentence sentence)
        {
            var exception = (ArgumentNullException)Record.Exception(() => new PartialRelationDefinition(constant, description, null, sequenceVariable, sentence));
            Assert.Equal("parameters", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(ValidArguments))]
        public void Constructor_WhenGivenValidSentences_CreatesASentenceWithTheSpecifiedSentencesAndConsequents(Constant constant, CharacterString description, IndividualVariable[] parameters, SequenceVariable sequenceVariable, Sentence sentence)
        {
            var subject = new PartialRelationDefinition(constant, description, parameters, sequenceVariable, sentence);

            Assert.Equal(constant, subject.Constant);
            Assert.Equal(description, subject.Description);
            Assert.Equal(parameters, subject.Parameters);
            Assert.Equal(sequenceVariable, subject.SequenceVariable);
            Assert.Equal(sentence, subject.Sentence);
        }

        [Theory]
        [MemberData(nameof(ValidArguments))]
        public void ToString_Always_ReturnsExpectedOutput(Constant constant, CharacterString description, IndividualVariable[] parameters, SequenceVariable sequenceVariable, Sentence sentence)
        {
            var expected = $"(defrelation {constant} ({string.Join(' ', parameters.Cast<Variable>().Concat(new[] { sequenceVariable }.Where(v => v != null)))}){(description == null ? "" : $" {description}")} :=> {sentence})";
            var subject = new PartialRelationDefinition(constant, description, parameters, sequenceVariable, sentence);

            Assert.Equal(expected, subject.ToString());
        }
    }
}
