// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using KnowledgeInterchangeFormat.Expressions;
    using Xunit;

    public class OperatorTests
    {
        public static IEnumerable<object[]> ValidNames { get; } = Array.ConvertAll(
            new object[]
            {
                "VALUE", "LISTOF", "QUOTE", "IF", "COND",
                "HOLDS", "=", "/=", "NOT", "AND", "OR", "=>", "<=", "<=>", "FORALL", "EXISTS",
                "DEFOBJECT", "DEFFUNCTION", "DEFRELATION", "DEFLOGICAL", ":=", ":->", ":<=", ":=>",
            },
            n => new object[] { n });

        [Fact]
        public void Constructor_WhenGivenANullString_ThrowsArgumentNullException()
        {
            var exception = (ArgumentNullException)Record.Exception(() => new Operator(null));
            Assert.Equal("name", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(ValidNames))]
        public void Constructor_WhenGivenAValidString_CreatesOperatorWithTheSpecifiedName(string value)
        {
            var subject = new Operator(value);
            Assert.Equal(value, subject.Name);
        }

        [Theory]
        [MemberData(nameof(ValidNames))]
        public void ToString_Always_ReturnsExpectedOutput(string value)
        {
            var subject = new Operator(value);

            Assert.Equal(value, subject.ToString());
        }
    }
}
