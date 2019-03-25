// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using KnowledgeInterchangeFormat.Expressions;
    using Xunit;

    public class ConstantSentenceTests
    {
        public static IEnumerable<object[]> ValidConstants => new List<object[]>
        {
            new object[] { new Constant("A") },
            new object[] { new Constant("Bb") },
            new object[] { new Constant(new string('C', 1024)) },
        };

        [Fact]
        public void Constructor_WhenGivenANullConstant_ThrowsArgumentNullException()
        {
            var exception = (ArgumentNullException)Record.Exception(() => new ConstantSentence(null));
            Assert.Equal("constant", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(ValidConstants))]
        public void Constructor_WhenGivenAValidConstant_CreatesASentenceWithTheSpecifiedConstant(Constant contents)
        {
            var subject = new ConstantSentence(contents);

            Assert.Equal(contents, subject.Constant);
        }

        [Theory]
        [MemberData(nameof(ValidConstants))]
        public void ToString_Always_ReturnsTheSameValueAsTheConstant(Constant contents)
        {
            var subject = new ConstantSentence(contents);

            Assert.Equal(contents.ToString(), subject.ToString());
        }
    }
}
