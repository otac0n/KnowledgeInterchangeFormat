// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using KnowledgeInterchangeFormat.Expressions;
    using Xunit;

    public class CompleteObjectDefinitionTests
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

        public static IEnumerable<object[]> ValidArguments =>
            from s1 in ValidConstants
            from s2 in ValidStrings
            from s3 in ValidTerms
            select s1.Concat(s2).Concat(s3).ToArray();

        public static IEnumerable<object[]> ValidArgumentsNoConstant =>
            from s1 in ValidStrings
            from s2 in ValidTerms
            select s1.Concat(s2).ToArray();

        public static IEnumerable<object[]> ValidArgumentsNoSentence =>
            from s1 in ValidConstants
            from s2 in ValidStrings
            select s1.Concat(s2).ToArray();

        [Theory]
        [MemberData(nameof(ValidArgumentsNoConstant))]
        public void Constructor_WhenGivenANullConstant_ThrowsArgumentNullException(CharacterString description, Term term)
        {
            var exception = (ArgumentNullException)Record.Exception(() => new CompleteObjectDefinition(null, description, term));
            Assert.Equal("constant", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(ValidArgumentsNoSentence))]
        public void Constructor_WhenGivenANullSentence_ThrowsArgumentNullException(Constant constant, CharacterString description)
        {
            var exception = (ArgumentNullException)Record.Exception(() => new CompleteObjectDefinition(constant, description, null));
            Assert.Equal("term", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(ValidArguments))]
        public void Constructor_WhenGivenValidArguments_CreatesAnObjectWithTheSpecifiedProperties(Constant constant, CharacterString description, Term term)
        {
            var subject = new CompleteObjectDefinition(constant, description, term);

            Assert.Equal(constant, subject.Constant);
            Assert.Equal(description, subject.Description);
            Assert.Equal(term, subject.Term);
        }

        [Theory]
        [MemberData(nameof(ValidArguments))]
        public void ToString_Always_ReturnsExpectedOutput(Constant constant, CharacterString description, Term term)
        {
            var expected = $"(defobject {constant}{(description == null ? "" : $" {description}")} := {term})";
            var subject = new CompleteObjectDefinition(constant, description, term);

            Assert.Equal(expected, subject.ToString());
        }
    }
}
