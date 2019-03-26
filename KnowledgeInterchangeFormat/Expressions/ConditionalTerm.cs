// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Text;

    /// <summary>
    /// A conditional term using the <c>cond</c> operator.
    /// </summary>
    public class ConditionalTerm : LogicalTerm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionalTerm"/> class.
        /// </summary>
        /// <param name="pairs">The logical pairs used in this conditional term.</param>
        public ConditionalTerm(IEnumerable<LogicalPair> pairs)
        {
            this.Pairs = (pairs ?? throw new ArgumentNullException(nameof(pairs))).ToImmutableList();
        }

        /// <summary>
        /// Gets the logical pairs used in this conditional term.
        /// </summary>
        public ImmutableList<LogicalPair> Pairs { get; }

        /// <inheritdoc/>
        public override void ToString(StringBuilder sb)
        {
            sb.Append("(cond");

            foreach (var pair in this.Pairs)
            {
                sb.Append(" (");
                pair.ToString(sb);
                sb.Append(')');
            }

            sb.Append(')');
        }
    }
}
