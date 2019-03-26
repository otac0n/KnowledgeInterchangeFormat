// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using KnowledgeInterchangeFormat.Expressions;
    using Xunit;

    public class CompleteFunctionDefinitionTests
    {
        public static IEnumerable<object[]> ValidTerms => new List<object[]>
        {
            new object[] { new CharacterReference('a') },
            new object[] { new CharacterString("Bb") },
            new object[] { new CharacterBlock(new string('C', 1024)) },
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
            from s5 in ValidTerms
            select s1.Concat(s2).Concat(s3).Concat(s4).Concat(s5).ToArray();

        public static IEnumerable<object[]> ValidArgumentsNoConstant =>
            from s1 in ValidStrings
            from s2 in ValidParameters
            from s3 in ValidSequenceVariables
            from s4 in ValidTerms
            select s1.Concat(s2).Concat(s3).Concat(s4).ToArray();

        public static IEnumerable<object[]> ValidArgumentsNoParameters =>
            from s1 in ValidConstants
            from s2 in ValidStrings
            from s3 in ValidSequenceVariables
            from s4 in ValidTerms
            select s1.Concat(s2).Concat(s3).Concat(s4).ToArray();

        public static IEnumerable<object[]> ValidArgumentsNoTerm =>
            from s1 in ValidConstants
            from s2 in ValidStrings
            from s3 in ValidParameters
            from s4 in ValidSequenceVariables
            select s1.Concat(s2).Concat(s3).Concat(s4).ToArray();

        [Theory]
        [MemberData(nameof(ValidArgumentsNoConstant))]
        public void Constructor_WhenGivenANullConstant_ThrowsArgumentNullException(CharacterString description, IndividualVariable[] parameters, SequenceVariable sequenceVariable, Term term)
        {
            var exception = (ArgumentNullException)Record.Exception(() => new CompleteFunctionDefinition(null, description, parameters, sequenceVariable, term));
            Assert.Equal("constant", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(ValidArgumentsNoTerm))]
        public void Constructor_WhenGivenANullTerm_ThrowsArgumentNullException(Constant constant, CharacterString description, IndividualVariable[] parameters, SequenceVariable sequenceVariable)
        {
            var exception = (ArgumentNullException)Record.Exception(() => new CompleteFunctionDefinition(constant, description, parameters, sequenceVariable, null));
            Assert.Equal("term", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(ValidArgumentsNoParameters))]
        public void Constructor_WhenGivenNullParameters_ThrowsArgumentNullException(Constant constant, CharacterString description, SequenceVariable sequenceVariable, Term term)
        {
            var exception = (ArgumentNullException)Record.Exception(() => new CompleteFunctionDefinition(constant, description, null, sequenceVariable, term));
            Assert.Equal("parameters", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(ValidArguments))]
        public void Constructor_WhenGivenValidSentences_CreatesASentenceWithTheSpecifiedSentencesAndConsequents(Constant constant, CharacterString description, IndividualVariable[] parameters, SequenceVariable sequenceVariable, Term term)
        {
            var subject = new CompleteFunctionDefinition(constant, description, parameters, sequenceVariable, term);

            Assert.Equal(constant, subject.Constant);
            Assert.Equal(description, subject.Description);
            Assert.Equal(parameters, subject.Parameters);
            Assert.Equal(sequenceVariable, subject.SequenceVariable);
            Assert.Equal(term, subject.Term);
        }

        [Theory]
        [MemberData(nameof(ValidArguments))]
        public void ToString_Always_ReturnsExpectedOutput(Constant constant, CharacterString description, IndividualVariable[] parameters, SequenceVariable sequenceVariable, Term term)
        {
            var expected = $"(deffunction {constant} ({string.Join(' ', parameters.Cast<Variable>().Concat(new[] { sequenceVariable }.Where(v => v != null)))}){(description == null ? "" : $" {description}")} := {term})";
            var subject = new CompleteFunctionDefinition(constant, description, parameters, sequenceVariable, term);

            Assert.Equal(expected, subject.ToString());
        }
    }
}
