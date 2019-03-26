// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using KnowledgeInterchangeFormat.Expressions;
    using Xunit;

    public class CharacterBlockTests
    {
        public static IEnumerable<object[]> ValidStrings { get; } = new List<object[]>
        {
            new object[] { "" },
            new object[] { "a" },
            new object[] { new string('a', 1024) },
            new object[] { new string(Enumerable.Range(0, 127).Select(i => (char)i).ToArray()) },
        };

        [Fact]
        public void Constructor_WhenGivenANullString_ThrowsArgumentNullException()
        {
            var exception = (ArgumentNullException)Record.Exception(() => new CharacterBlock(null));
            Assert.Equal("characters", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(ValidStrings))]
        public void Constructor_WhenGivenAValidString_CreatesAListWithTheCharactersAsElements(string value)
        {
            var subject = (ListTerm)new CharacterBlock(value);
            Assert.Equal(value, subject.Items.Select(t => ((CharacterReference)t).Character));
        }

        [Theory]
        [MemberData(nameof(ValidStrings))]
        public void Constructor_WhenGivenAValidString_CreatesAValidInstance(string value)
        {
            var subject = new CharacterBlock(value);
            Assert.Equal(value, subject.Characters);
        }

        [Theory]
        [MemberData(nameof(ValidStrings))]
        public void ToString_Always_ReturnsExpectedOutput(string value)
        {
            var expected = $"#{value.Length}q{value}";
            var subject = new CharacterBlock(value);

            Assert.Equal(expected, subject.ToString());
        }
    }
}
