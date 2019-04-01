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
    public class ConditionalTerm : LogicalTerm, IEquatable<ConditionalTerm>
    {
        private const int HashCodeSeed = 0x30c7c4d9;

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
        public override bool Equals(Expression other) => other is ConditionalTerm conditionalTerm && this.Equals(conditionalTerm);

        /// <inheritdoc/>
        public bool Equals(ConditionalTerm other) => !(other is null) &&
            this.Pairs.Count == other.Pairs.Count &&
            EquatableUtilities.ListsEqual(this.Pairs, other.Pairs);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hash = HashCodeSeed;
            EquatableUtilities.Combine(ref hash, EquatableUtilities.HashList(this.Pairs));
            return hash;
        }

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
