// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using KnowledgeInterchangeFormat.Expressions;
    using Xunit;

    public class QuotationTests
    {
        public static IEnumerable<object[]> ValidQuotations => new List<object[]>
        {
            new object[] { new IndividualVariable("?a") },
            new object[] { new Constant("Bb") },
            new object[] { new ListExpression(Array.Empty<Expression>()) },
        };

        [Fact]
        public void Constructor_WhenGivenANullSentence_ThrowsArgumentNullException()
        {
            var exception = (ArgumentNullException)Record.Exception(() => new Quotation(null));
            Assert.Equal("quoted", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(ValidQuotations))]
        public void Constructor_WhenGivenAValidSentence_CreatesASentenceWithTheSpecifiedSentence(Expression quoted)
        {
            var subject = new Quotation(quoted);

            Assert.Equal(quoted, subject.Quoted);
        }

        [Theory]
        [MemberData(nameof(ValidQuotations))]
        public void ToString_Always_ReturnsExpectedOutput(Expression quoted)
        {
            var expected = $"'{quoted}";
            var subject = new Quotation(quoted);

            Assert.Equal(expected, subject.ToString());
        }
    }
}
