// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Tests.Parser
{
    using System.Collections.Generic;
    using Xunit;

    public class ParserTests
    {
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
    }
}
