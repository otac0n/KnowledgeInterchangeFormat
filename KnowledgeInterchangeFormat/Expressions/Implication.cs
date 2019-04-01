// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Text;

    /// <summary>
    /// A <see cref="Sentence"/> involving the <c>=&gt;</c> operator.
    /// </summary>
    public class Implication : LogicalSentence, IEquatable<Implication>
    {
        private const int HashCodeSeed = 0x1cb4b938;

        /// <summary>
        /// Initializes a new instance of the <see cref="Implication"/> class.
        /// </summary>
        /// <param name="antecedents">The antecedents of the implication.</param>
        /// <param name="consequent">The consequent of the implication.</param>
        public Implication(IEnumerable<Sentence> antecedents, Sentence consequent)
        {
            this.Antecedents = (antecedents ?? throw new ArgumentNullException(nameof(antecedents))).ToImmutableList();
            this.Consequent = consequent ?? throw new ArgumentNullException(nameof(consequent));
        }

        /// <summary>
        /// Gets the antecedents of the implication.
        /// </summary>
        public ImmutableList<Sentence> Antecedents { get; }

        /// <summary>
        /// Gets the consequent of the implication.
        /// </summary>
        public Sentence Consequent { get; }

        /// <inheritdoc/>
        public override bool Equals(Expression other) => other is Implication implication && this.Equals(implication);

        /// <inheritdoc/>
        public virtual bool Equals(Implication other) => !(other is null) &&
            !(other is ReverseImplication) &&
            this.Antecedents.Count == other.Antecedents.Count &&
            this.Consequent == other.Consequent &&
            EquatableUtilities.ListsEqual(this.Antecedents, other.Antecedents);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hash = HashCodeSeed;
            EquatableUtilities.Combine(ref hash, this.Consequent);
            EquatableUtilities.Combine(ref hash, EquatableUtilities.HashList(this.Antecedents));
            return hash;
        }

        /// <inheritdoc />
        public override void ToString(StringBuilder sb)
        {
            sb.Append("(=> ");

            foreach (var antecedent in this.Antecedents)
            {
                antecedent.ToString(sb);
                sb.Append(' ');
            }

            this.Consequent.ToString(sb);

            sb.Append(')');
        }
    }
}
