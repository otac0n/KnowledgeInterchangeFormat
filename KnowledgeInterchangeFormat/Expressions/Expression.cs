// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System.Text;

    /// <summary>
    /// The base class for all types in the <see cref="KnowledgeInterchangeFormat.Expressions"/> namespace.
    /// </summary>
    public abstract class Expression
    {
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
