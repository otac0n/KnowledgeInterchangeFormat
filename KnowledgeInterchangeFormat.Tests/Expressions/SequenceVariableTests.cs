// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using KnowledgeInterchangeFormat.Expressions;
    using Xunit;

    public class SequenceVariableTests
    {
        public static IEnumerable<object[]> ValidNames { get; } = new List<object[]>
        {
            new object[] { "@OK" },
            new object[] { "@VAR" },
        };

        [Fact]
        public void Constructor_WhenGivenANullString_ThrowsArgumentNullException()
        {
            var exception = (ArgumentNullException)Record.Exception(() => new SequenceVariable(null));
            Assert.Equal("name", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(ValidNames))]
        public void Constructor_WhenGivenAValidString_CreatesVariableWithTheSpecifiedName(string value)
        {
            var subject = new SequenceVariable(value);
            Assert.Equal(value, subject.Name);
        }

        [Theory]
        [MemberData(nameof(ValidNames))]
        public void ToString_WhenEscapingIsNotRequired_ReturnsTheOriginalName(string value)
        {
            var subject = new SequenceVariable(value);

            Assert.Equal(value, subject.ToString());
        }

        [Fact]
        public void ToString_WhenEscapingIsRequired_ReturnsExpectedOutput()
        {
            var subject = new SequenceVariable(@"@A\b0-!$%&*+./<=>?@_~");
            Assert.Equal(@"@A\\\b0-!$%&*+./<=>?@_~", subject.ToString());
        }
    }
}
