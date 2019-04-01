// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Text;

    /// <summary>
    /// A <see cref="Sentence"/> involving the <c>or</c> operator.
    /// </summary>
    public class Disjunction : LogicalSentence, IEquatable<Disjunction>
    {
        private const int HashCodeSeed = unchecked((int)0xa34e5423);

        /// <summary>
        /// Initializes a new instance of the <see cref="Disjunction"/> class.
        /// </summary>
        /// <param name="disjuncts">The sentences in the disjunction.</param>
        public Disjunction(IEnumerable<Sentence> disjuncts)
        {
            this.Disjuncts = (disjuncts ?? throw new ArgumentNullException(nameof(disjuncts))).ToImmutableList();
        }

        /// <summary>
        /// Gets the sentences in the disjunction.
        /// </summary>
        public ImmutableList<Sentence> Disjuncts { get; }

        /// <inheritdoc/>
        public override bool Equals(Expression other) => other is Disjunction disjunction && this.Equals(disjunction);

        /// <inheritdoc/>
        public bool Equals(Disjunction other) => !(other is null) &&
            this.Disjuncts.Count == other.Disjuncts.Count &&
            EquatableUtilities.ListsEqual(this.Disjuncts, other.Disjuncts);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hash = HashCodeSeed;
            EquatableUtilities.Combine(ref hash, EquatableUtilities.HashList(this.Disjuncts));
            return hash;
        }

        /// <inheritdoc />
        public override void ToString(StringBuilder sb)
        {
            sb.Append("(or");

            foreach (var disjunct in this.Disjuncts)
            {
                sb.Append(' ');
                disjunct.ToString(sb);
            }

            sb.Append(')');
        }
    }
}
