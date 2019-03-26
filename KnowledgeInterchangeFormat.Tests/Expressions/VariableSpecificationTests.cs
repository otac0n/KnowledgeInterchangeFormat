// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using KnowledgeInterchangeFormat.Expressions;
    using Xunit;

    public class VariableSpecificationTests
    {
        public static IEnumerable<object[]> ValidVariables => new List<object[]>
        {
            new object[] { new IndividualVariable("?OK") },
            new object[] { new SequenceVariable("@A") },
            new object[] { new IndividualVariable("?Bb") },
        };

        public static IEnumerable<object[]> ValidConstants => new List<object[]>
        {
            new object[] { null },
            new object[] { new Constant("A") },
            new object[] { new Constant("Bb") },
            new object[] { new Constant(new string('C', 1024)) },
        };

        public static IEnumerable<object[]> ValidArguments =>
            from s1 in ValidVariables
            from s2 in ValidConstants
            select s1.Concat(s2).ToArray();

        [Theory]
        [MemberData(nameof(ValidConstants))]
        public void Constructor_WhenGivenANullVariable_ThrowsArgumentNullException(Constant constant)
        {
            var exception = (ArgumentNullException)Record.Exception(() => new VariableSpecification(null, constant));
            Assert.Equal("variable", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(ValidArguments))]
        public void Constructor_WhenGivenValidArguments_CreatesAnObjectWithTheSpecifiedProperties(Variable variable, Constant constant)
        {
            var subject = new VariableSpecification(variable, constant);

            Assert.Equal(variable, subject.Variable);
            Assert.Equal(constant, subject.Constant);
        }

        [Theory]
        [MemberData(nameof(ValidVariables))]
        public void ToString_WhenNoConstantIsPresent_DoesNotWrapWithParentheses(Variable variable)
        {
            var subject = new VariableSpecification(variable, null);
            Assert.Equal(variable.ToString(), subject.ToString());
        }

        [Theory]
        [MemberData(nameof(ValidVariables))]
        public void ToString_WhenConstantIsPresent_WrapsWithParentheses(Variable variable)
        {
            var expected = $"({variable} OK)";
            var subject = new VariableSpecification(variable, new Constant("OK"));

            Assert.Equal(expected, subject.ToString());
        }
    }
}
