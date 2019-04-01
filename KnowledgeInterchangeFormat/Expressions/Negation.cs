// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;
    using System.Text;

    /// <summary>
    /// A <see cref="Sentence"/> involving the <c>not</c> operator.
    /// </summary>
    public class Negation : LogicalSentence, IEquatable<Negation>
    {
        private const int HashCodeMask = unchecked((int)0x9156948f);

        /// <summary>
        /// Initializes a new instance of the <see cref="Negation"/> class.
        /// </summary>
        /// <param name="negated">The <see cref="Sentence"/> being negated.</param>
        public Negation(Sentence negated)
        {
            this.Negated = negated ?? throw new ArgumentNullException(nameof(negated));
        }

        /// <summary>
        /// Gets the <see cref="Negated"/> being negated.
        /// </summary>
        public Sentence Negated { get; }

        /// <inheritdoc/>
        public override bool Equals(Expression other) => other is Negation negation && this.Equals(negation);

        /// <inheritdoc/>
        public bool Equals(Negation other) => !(other is null) && this.Negated == other.Negated;

        /// <inheritdoc />
        public override int GetHashCode() => this.Negated.GetHashCode() ^ HashCodeMask;

        /// <inheritdoc />
        public override void ToString(StringBuilder sb)
        {
            sb.Append("(not ");
            this.Negated.ToString(sb);
            sb.Append(')');
        }
    }
}
