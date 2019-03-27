// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using KnowledgeInterchangeFormat.Expressions;
    using Xunit;

    public class IndividualVariableTests
    {
        public static IEnumerable<object[]> ValidIds { get; } = new List<object[]>
        {
            new object[] { "?OK" },
            new object[] { "?VAR" },
        };

        public static IEnumerable<object[]> ValidIdNamePairs =>
            from ids in ValidIds
            let id = (string)ids.Single()
            from name in new[] { null, id, id.ToLowerInvariant() }.Distinct()
            select new[] { id, name };

        [Fact]
        public void Constructor_WhenGivenANullId_ThrowsArgumentNullException()
        {
            var exception = (ArgumentNullException)Record.Exception(() => new IndividualVariable(null));
            Assert.Equal("id", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(ValidIdNamePairs))]
        public void Constructor_WhenGivenAValidId_CreatesVariableWithTheSpecifiedId(string id, string name)
        {
            var subject = new IndividualVariable(id, name);
            Assert.Equal(id, subject.Id);
            Assert.Equal(name ?? id, subject.Name);
        }

        [Theory]
        [MemberData(nameof(ValidIds))]
        public void ToString_WhenEscapingIsNotRequired_ReturnsTheOriginalId(string value)
        {
            var subject = new IndividualVariable(value);

            Assert.Equal(value, subject.ToString());
        }

        [Fact]
        public void ToString_WhenEscapingIsRequired_ReturnsExpectedOutput()
        {
            var subject = new IndividualVariable(@"?A\b0-!$%&*+./<=>?@_~");
            Assert.Equal(@"?A\\\b0-!$%&*+./<=>?@_~", subject.ToString());
        }
    }
}
