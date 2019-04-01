// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;
    using System.Text;
    using Pegasus.Common;

    /// <summary>
    /// The base class for all types in the <see cref="KnowledgeInterchangeFormat.Expressions"/> namespace.
    /// </summary>
    public abstract class Expression : ILexical, IEquatable<Expression>
    {
        /// <inheritdoc />
        public Cursor EndCursor { get; set; }

        /// <inheritdoc />
        public Cursor StartCursor { get; set; }

        /// <summary>
        /// Determines whether two specified expressions are identical.
        /// </summary>
        /// <param name="left">The first <see cref="Expression"/> to compare, or null.</param>
        /// <param name="right">The second <see cref="Expression"/> to compare, or null.</param>
        /// <returns>true if the value of <paramref name="left"/> is the same as the value of <paramref name="right"/>; otherwise, false.</returns>
        public static bool operator ==(Expression left, Expression right) => object.Equals(left, right);

        /// <summary>
        /// Determines whether two specified expressions are different.
        /// </summary>
        /// <param name="left">The first <see cref="Expression"/> to compare, or null.</param>
        /// <param name="right">The second <see cref="Expression"/> to compare, or null.</param>
        /// <returns>true if the value of <paramref name="left"/> is the different from the value of <paramref name="right"/>; otherwise, false.</returns>
        public static bool operator !=(Expression left, Expression right) => !object.Equals(left, right);

        /// <inheritdoc />
        public abstract bool Equals(Expression other);

        /// <inheritdoc />
        public override bool Equals(object obj) => obj is Expression other && this.Equals(other);

        /// <inheritdoc />
        public abstract override int GetHashCode();

        /// <inheritdoc />
        public sealed override string ToString()
        {
            var sb = new StringBuilder();
            this.ToString(sb);
            return sb.ToString();
        }

        /// <summary>
        /// Serialze into the specified string builder.
        /// </summary>
        /// <param name="sb">The destination string builder.</param>
        public abstract void ToString(StringBuilder sb);
    }
}
