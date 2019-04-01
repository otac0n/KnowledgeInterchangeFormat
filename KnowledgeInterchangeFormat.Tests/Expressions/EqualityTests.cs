// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using KnowledgeInterchangeFormat.Expressions;
    using Xunit;

    public class EqualityTests
    {
        private static readonly Type[] ConcreteExpressionTypes = typeof(Expression).Assembly.GetTypes().Where(t => !t.IsAbstract && typeof(Expression).IsAssignableFrom(t)).ToArray();

        public static IEnumerable<object[]> TypeArgumentIndices =>
            from t in ConcreteExpressionTypes
            where !typeof(WordTerm).IsAssignableFrom(t)
            from i in Enumerable.Range(0, t.GetConstructors().Max(c => c.GetParameters().Length))
            select new object[] { t, i };

        public static IEnumerable<object[]> Types =>
            from t in ConcreteExpressionTypes
            select new object[] { t };

        [Theory]
        [MemberData(nameof(TypeArgumentIndices))]
        public void Equals_WhenArgumentsAreDifferent_ReturnsFalse(Type type, int constructorIndex)
        {
            var arguments = MakeArguments(type, 0, out var constructor);
            var a = (Expression)constructor.Invoke(arguments);
            arguments[constructorIndex] = MakeBasicInstance(constructor.GetParameters()[constructorIndex].ParameterType, 1);
            var b = (Expression)constructor.Invoke(arguments);

            Assert.NotEqual(a, b);
            Assert.False(a.Equals(b));
            Assert.False(b.Equals(a));
            Assert.False(a == b);
            Assert.False(b == a);
            Assert.True(a != b);
            Assert.True(b != a);
        }

        [Theory]
        [MemberData(nameof(TypeArgumentIndices))]
        public void GetHashCode_WhenArgumentsAreDifferent_ReturnsDifferentValues(Type type, int constructorIndex)
        {
            var arguments = MakeArguments(type, 0, out var constructor);
            var a = constructor.Invoke(arguments).GetHashCode();
            arguments[constructorIndex] = MakeBasicInstance(constructor.GetParameters()[constructorIndex].ParameterType, 1);
            var b = constructor.Invoke(arguments).GetHashCode();

            Assert.NotEqual(a, b);
        }

        [Theory]
        [MemberData(nameof(Types))]
        public void Equals_WhenArgumentsAreIdentical_ReturnsTrue(Type type)
        {
            var arguments = MakeArguments(type, 0, out var constructor);
            var a = (Expression)constructor.Invoke(arguments);
            var b = (Expression)constructor.Invoke(arguments);

            Assert.Equal(a, b);
            Assert.True(a.Equals(b));
            Assert.True(b.Equals(a));
            Assert.True(a == b);
            Assert.True(b == a);
            Assert.False(a != b);
            Assert.False(b != a);
        }

        [Theory]
        [MemberData(nameof(Types))]
        public void Equals_WhenArgumentsAreCopies_ReturnsTrue(Type type)
        {
            var arguments = MakeArguments(type, 0, out var constructor);
            var a = (Expression)constructor.Invoke(arguments);
            arguments = MakeArguments(type, 0, out constructor);
            var b = (Expression)constructor.Invoke(arguments);

            Assert.Equal(a, b);
            Assert.True(a.Equals(b));
            Assert.True(b.Equals(a));
            Assert.True(a == b);
            Assert.True(b == a);
            Assert.False(a != b);
            Assert.False(b != a);
        }

        private static object MakeBasicInstance(Type type, int ix)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                var elementType = type.GetGenericArguments()[0];
                var array = Array.CreateInstance(elementType, 1);
                array.SetValue(MakeBasicInstance(elementType, ix), 0);
                return array;
            }
            else if (type == typeof(string))
            {
                return ix == 0 ? "a" : "b";
            }
            else if (type == typeof(char))
            {
                return ix == 0 ? 'a' : 'b';
            }
            else if (type.IsAbstract)
            {
                if (type == typeof(Sentence) || type == typeof(Form))
                {
                    type = typeof(ConstantSentence);
                }
                else if (type == typeof(Term) || type == typeof(Expression))
                {
                    type = typeof(CharacterString);
                }
                else if (type == typeof(Variable))
                {
                    type = typeof(IndividualVariable);
                }
                else
                {
                    throw new InvalidOperationException($"Could not construct {type.Name}");
                }
            }

            return Activator.CreateInstance(type, MakeArguments(type, ix, out var constructor));
        }

        private static object[] MakeArguments(Type type, int ix, out ConstructorInfo constructor)
        {
            constructor = type.GetConstructors().OrderByDescending(c => c.GetParameters().Length).FirstOrDefault();
            if (constructor == null)
            {
                throw new InvalidOperationException($"Could not construct {type.Name}");
            }

            return Array.ConvertAll(constructor.GetParameters(), p => MakeBasicInstance(p.ParameterType, ix));
        }
    }
}
