// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Text;

    /// <summary>
    /// A <see cref="Term"/> that contains a list of other terms.
    /// </summary>
    public class ListTerm : Term, IEquatable<ListTerm>
    {
        private const int HashCodeSeed = unchecked((int)0xa53e870d);

        /// <summary>
        /// Initializes a new instance of the <see cref="ListTerm"/> class.
        /// </summary>
        /// <param name="items">The other items contained in this list.</param>
        /// <param name="sequenceVariable">An optional <see cref="SequenceVariable"/> containing additional items contained in this list.</param>
        public ListTerm(IEnumerable<Term> items, SequenceVariable sequenceVariable)
        {
            this.Items = (items ?? throw new ArgumentNullException(nameof(items))).ToImmutableList();
            this.SequenceVariable = sequenceVariable;
        }

        /// <summary>
        /// Gets an optional <see cref="SequenceVariable"/> containing additional items contained in this list.
        /// </summary>
        public SequenceVariable SequenceVariable { get; }

        /// <summary>
        /// Gets the other items contained in this list.
        /// </summary>
        public ImmutableList<Term> Items { get; }

        /// <inheritdoc/>
        public override bool Equals(Expression other) => other is ListTerm listTerm && this.Equals(listTerm);

        /// <inheritdoc/>
        public bool Equals(ListTerm other) => !(other is null) &&
            this.Items.Count == other.Items.Count &&
            !(other is CharacterBlock || other is CharacterString) &&
            this.SequenceVariable == other.SequenceVariable &&
            EquatableUtilities.ListsEqual(this.Items, other.Items);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hash = HashCodeSeed;
            EquatableUtilities.Combine(ref hash, EquatableUtilities.HashList(this.Items));
            EquatableUtilities.Combine(ref hash, this.SequenceVariable);
            return hash;
        }

        /// <inheritdoc/>
        public override void ToString(StringBuilder sb)
        {
            sb.Append("(listof");

            foreach (var item in this.Items)
            {
                sb.Append(' ');
                item.ToString(sb);
            }

            if (this.SequenceVariable != null)
            {
                sb.Append(' ');
                this.SequenceVariable.ToString(sb);
            }

            sb.Append(')');
        }
    }
}
