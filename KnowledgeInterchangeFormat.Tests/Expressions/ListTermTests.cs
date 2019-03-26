// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using KnowledgeInterchangeFormat.Expressions;
    using Xunit;

    public class ListTermTests
    {
        public static IEnumerable<object[]> ValidItems => new List<object[]>
        {
            new object[] { new Term[] { new CharacterReference('a') } },
            new object[] { new Term[] { new CharacterReference('a'), new CharacterReference('b'), new CharacterReference('b') } },
            new object[] { new Term[] { new CharacterString("abc") } },
            new object[] { new Term[] { new CharacterString("a"), new ListTerm(new Term[] { new CharacterReference('b') }, null), new CharacterReference('c') } },
        };

        [Fact]
        public void Constructor_WhenGivenANullList_ThrowsArgumentNullException()
        {
            var exception = (ArgumentNullException)Record.Exception(() => new ListTerm(null, null));
            Assert.Equal("items", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(ValidItems))]
        public void Constructor_WhenGivenValidArguments_CreatesAnObjectWithTheSpecifiedProperties(Term[] items)
        {
            var subject = new ListTerm(items, null);

            Assert.Equal(items, subject.Items);
        }

        [Fact]
        public void ToString_WhenNoSequenceVariableIsPresent_ReturnsExpectedOutput()
        {
            var subject = new ListTerm(new Term[] { new CharacterString("a"), new ListTerm(new Term[] { new CharacterReference('b') }, null), new CharacterReference('c') }, null);
            Assert.Equal("(listof \"a\" (listof #\\b) #\\c)", subject.ToString());
        }

        [Fact]
        public void ToString_WhenSequenceVariableIsPresent_ReturnsExpectedOutput()
        {
            var subject = new ListTerm(new Term[] { new CharacterString("abc") }, new SequenceVariable("@OK"));
            Assert.Equal("(listof \"abc\" @OK)", subject.ToString());
        }

        [Fact]
        public void ToString_WhenSequenceVariableIsPresentWithEmptyList_ReturnsExpectedOutput()
        {
            var subject = new ListTerm(Array.Empty<Term>(), new SequenceVariable("@OK"));
            Assert.Equal("(listof @OK)", subject.ToString());
        }
    }
}
