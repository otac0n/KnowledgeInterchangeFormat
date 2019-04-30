// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Tests.Parser
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using KnowledgeInterchangeFormat.Expressions;
    using Xunit;

    public class ParserTests
    {
        public static IEnumerable<object[]> ExplicitOperatorExpressions => new List<object[]>
        {
            new object[] { "(holds x)", new ExplicitRelationalSentence(new Constant("X"), Array.Empty<Term>(), null) },
            new object[] { "(holds ok (value x))", new ExplicitRelationalSentence(new Constant("OK"), new[] { new ExplicitFunctionalTerm(new Constant("X"), Array.Empty<Term>(), null) }, null) },
        };

        public static IEnumerable<object[]> ExplicitOperatorExpressionsAsImplicit => new List<object[]>
        {
            new object[] { "(holds x)", new ImplicitRelationalSentence(new Constant("HOLDS"), new[] { new Constant("X") }, null) },
            new object[] { "(ok (value x))", new ImplicitRelationalSentence(new Constant("OK"), new[] { new ImplicitFunctionalTerm(new Constant("VALUE"), new[] { new Constant("X") }, null) }, null) },
        };

        public static IEnumerable<object[]> CanonicalExpressions => new List<object[]>
        {
            new object[] { "(role o)\r\n(role x)" },
        };

        [Theory]
        [MemberData(nameof(CanonicalExpressions))]
        public void Parse_GivenAnExpressionInCanonicalForm_ReturnsAnExpressionThatRoundtripsTheForm(string value)
        {
            var parser = new KifParser();
            var expression = parser.Parse(value);

            Assert.Equal(value, expression.ToString());
        }

        [Theory]
        [MemberData(nameof(ExplicitOperatorExpressions))]
        public void Parse_GivenAnExplicitOperatorExpression_ReturnsTheExpectedExpression(string value, Sentence sentence)
        {
            var parser = new KifParser();
            var expression = parser.Parse(value);

            Assert.Equal(expression.Forms.Single(), sentence);
        }

        [Theory]
        [MemberData(nameof(ExplicitOperatorExpressionsAsImplicit))]
        public void Parse_GivenAnExplicitOperatorExpressionWihtExplicitOperatorsDisabled_ReturnsAnImplicitExpression(string value, Sentence sentence)
        {
            var parser = new KifParser(new KifParser.Options
            {
                ExplicitOperators = false,
            });
            var expression = parser.Parse(value);

            Assert.Equal(expression.Forms.Single(), sentence);
        }
    }
}
