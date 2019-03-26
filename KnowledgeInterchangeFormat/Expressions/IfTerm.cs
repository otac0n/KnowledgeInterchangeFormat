// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Text;

    /// <summary>
    /// A conditional term using the <c>if</c> operator.
    /// </summary>
    public class IfTerm : LogicalTerm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IfTerm"/> class.
        /// </summary>
        /// <param name="pairs">The logical pairs used in this conditional term.</param>
        /// <param name="default">An optional default value used when none of the conditions are met.</param>
        public IfTerm(IEnumerable<LogicalPair> pairs, Term @default)
        {
            this.Pairs = (pairs ?? throw new ArgumentNullException(nameof(pairs))).ToImmutableList();
            this.Default = @default;
        }

        /// <summary>
        /// Gets an optional default value used when none of the conditions are met.
        /// </summary>
        public Term Default { get; }

        /// <summary>
        /// Gets the logical pairs used in this conditional term.
        /// </summary>
        public ImmutableList<LogicalPair> Pairs { get; }

        /// <inheritdoc/>
        public override void ToString(StringBuilder sb)
        {
            sb.Append("(if");

            foreach (var pair in this.Pairs)
            {
                sb.Append(' ');
                pair.ToString(sb);
            }

            if (this.Default != null)
            {
                sb.Append(' ');
                this.Default.ToString(sb);
            }

            sb.Append(')');
        }
    }
}
