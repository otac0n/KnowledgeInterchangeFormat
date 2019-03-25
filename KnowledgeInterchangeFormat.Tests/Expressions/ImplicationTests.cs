// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using KnowledgeInterchangeFormat.Expressions;
    using Xunit;

    public class ImplicationTests
    {
        public static IEnumerable<object[]> ValidAntecedents => new List<object[]>
        {
            new object[] { Array.Empty<Sentence>() },
            new object[] { new Sentence[] { new ConstantSentence(new Constant("A")) } },
            new object[] { new Sentence[] { new ConstantSentence(new Constant("A")), new ConstantSentence(new Constant("Bb")) } },
            new object[] { new Sentence[] { new ConstantSentence(new Constant("A")), new ConstantSentence(new Constant("Bb")), new ConstantSentence(new Constant(new string('C', 1024))) } },
        };

        public static IEnumerable<object[]> ValidConsequents => new List<object[]>
        {
            new object[] { new ConstantSentence(new Constant("A")) },
            new object[] { new ConstantSentence(new Constant("Bb")) },
            new object[] { new ConstantSentence(new Constant(new string('C', 1024))) },
        };

        public static IEnumerable<object[]> ValidSentencePairs =>
            from s1 in ValidAntecedents
            from s2 in ValidConsequents
            select s1.Concat(s2).ToArray();

        [Theory]
        [MemberData(nameof(ValidAntecedents))]
        public void Constructor_WhenGivenANullConsequent_ThrowsArgumentNullException(Sentence[] antecedents)
        {
            var exception = (ArgumentNullException)Record.Exception(() => new Implication(antecedents, null));
            Assert.Equal("consequent", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(ValidConsequents))]
        public void Constructor_WhenGivenNullAntecedents_ThrowsArgumentNullException(Sentence consequent)
        {
            var exception = (ArgumentNullException)Record.Exception(() => new Implication(null, consequent));
            Assert.Equal("antecedents", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(ValidSentencePairs))]
        public void Constructor_WhenGivenValidSentences_CreatesASentenceWithTheSpecifiedAntecedentsAndConsequents(Sentence[] antecedents, Sentence consequent)
        {
            var subject = new Implication(antecedents, consequent);

            Assert.Equal(antecedents, subject.Antecedents);
            Assert.Equal(consequent, subject.Consequent);
        }

        [Theory]
        [MemberData(nameof(ValidSentencePairs))]
        public void ToString_Always_ReturnsExpectedOutput(Sentence[] antecedents, Sentence consequent)
        {
            var expected = $"(=>{string.Concat(antecedents.Select(s => string.Concat($" {s}")))} {consequent})";
            var subject = new Implication(antecedents, consequent);

            Assert.Equal(expected, subject.ToString());
        }
    }
}
