// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using KnowledgeInterchangeFormat.Expressions;
    using Xunit;

    public class UnrestrictedLogicalDefinitionTests
    {
        public static IEnumerable<object[]> ValidSentences => new List<object[]>
        {
            new object[] { Array.Empty<Sentence>() },
            new object[] { new Sentence[] { new ConstantSentence(new Constant("A")) } },
            new object[] { new Sentence[] { new ConstantSentence(new Constant("A")), new ConstantSentence(new Constant("Bb")) } },
            new object[] { new Sentence[] { new ConstantSentence(new Constant("A")), new ConstantSentence(new Constant("Bb")), new ConstantSentence(new Constant(new string('C', 1024))) } },
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

        public static IEnumerable<object[]> ValidArguments =>
            from s1 in ValidConstants
            from s2 in ValidStrings
            from s3 in ValidSentences
            select s1.Concat(s2).Concat(s3).ToArray();

        [Theory]
        [MemberData(nameof(ValidSentences))]
        public void Constructor_WhenGivenANullConstant_ThrowsArgumentNullException(Sentence[] sentences)
        {
            var exception = (ArgumentNullException)Record.Exception(() => new UnrestrictedLogicalDefinition(null, null, sentences));
            Assert.Equal("constant", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(ValidConstants))]
        public void Constructor_WhenGivenNullSentences_ThrowsArgumentNullException(Constant constant)
        {
            var exception = (ArgumentNullException)Record.Exception(() => new UnrestrictedLogicalDefinition(constant, null, null));
            Assert.Equal("sentences", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(ValidArguments))]
        public void Constructor_WhenGivenValidArguments_CreatesAnObjectWithTheSpecifiedProperties(Constant constant, CharacterString description, Sentence[] sentences)
        {
            var subject = new UnrestrictedLogicalDefinition(constant, description, sentences);

            Assert.Equal(constant, subject.Constant);
            Assert.Equal(description, subject.Description);
            Assert.Equal(sentences, subject.Sentences);
        }

        [Theory]
        [MemberData(nameof(ValidArguments))]
        public void ToString_Always_ReturnsExpectedOutput(Constant constant, CharacterString description, Sentence[] sentences)
        {
            var expected = $"(deflogical {constant}{(description == null ? "" : $" {description}")}{string.Concat(sentences.Select(s => string.Concat($" {s}")))})";
            var subject = new UnrestrictedLogicalDefinition(constant, description, sentences);

            Assert.Equal(expected, subject.ToString());
        }
    }
}
