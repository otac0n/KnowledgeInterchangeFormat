// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Tests.Expressions
{
    using System.Collections.Generic;
    using System.Linq;
    using KnowledgeInterchangeFormat.Expressions;
    using Xunit;

    public class CharacterReferenceTests
    {
        public static IEnumerable<object[]> ValidCharacters { get; } = Enumerable.Range(0, 127).Select(i => new object[] { (char)i });

        [Theory]
        [MemberData(nameof(ValidCharacters))]
        public void Constructor_WhenGivenAValidCharacter_CreatesAValidInstance(char value)
        {
            var subject = new CharacterReference(value);
            Assert.Equal(value, subject.Character);
        }

        [Theory]
        [MemberData(nameof(ValidCharacters))]
        public void ToString_Always_ReturnsExpectedOutput(char value)
        {
            var expected = $"#\\{value}";
            var subject = new CharacterReference(value);

            Assert.Equal(expected, subject.ToString());
        }
    }
}
