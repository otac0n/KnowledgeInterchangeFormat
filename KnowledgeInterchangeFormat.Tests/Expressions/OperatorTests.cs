// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using KnowledgeInterchangeFormat.Expressions;
    using Xunit;

    public class OperatorTests
    {
        public static IEnumerable<object[]> ValidIds { get; } = Array.ConvertAll(
            new object[]
            {
                "VALUE", "LISTOF", "QUOTE", "IF", "COND",
                "HOLDS", "=", "/=", "NOT", "AND", "OR", "=>", "<=", "<=>", "FORALL", "EXISTS",
                "DEFOBJECT", "DEFFUNCTION", "DEFRELATION", "DEFLOGICAL", ":=", ":->", ":<=", ":=>",
            },
            n => new object[] { n });

        public static IEnumerable<object[]> ValidIdNamePairs =>
            from ids in ValidIds
            let id = (string)ids.Single()
            from name in new[] { null, id, id.ToLowerInvariant() }.Distinct()
            select new[] { id, name };

        [Fact]
        public void Constructor_WhenGivenANullId_ThrowsArgumentNullException()
        {
            var exception = (ArgumentNullException)Record.Exception(() => new Operator(null));
            Assert.Equal("id", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(ValidIdNamePairs))]
        public void Constructor_WhenGivenAValidId_CreatesOperatorWithTheSpecifiedId(string id, string name)
        {
            var subject = new Operator(id, name);
            Assert.Equal(id, subject.Id);
            Assert.Equal(name ?? id, subject.Name);
        }

        [Theory]
        [MemberData(nameof(ValidIds))]
        public void ToString_Always_ReturnsExpectedOutput(string id)
        {
            var subject = new Operator(id);

            Assert.Equal(id, subject.ToString());
        }
    }
}
