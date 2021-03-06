// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;
    using System.Text;

    /// <summary>
    /// A <see cref="Sentence"/> involving the <c>&lt;=&gt;</c> operator.
    /// </summary>
    public class Equivalence : LogicalSentence, IEquatable<Equivalence>
    {
        private const int HashCodeSeed = unchecked((int)0x93e5a1c3);

        /// <summary>
        /// Initializes a new instance of the <see cref="Equivalence"/> class.
        /// </summary>
        /// <param name="left">The left side of the equivalence.</param>
        /// <param name="right">The right side of the equivalence.</param>
        public Equivalence(Sentence left, Sentence right)
        {
            this.Left = left ?? throw new ArgumentNullException(nameof(left));
            this.Right = right ?? throw new ArgumentNullException(nameof(right));
        }

        /// <summary>
        /// Gets the left side of the equivalence.
        /// </summary>
        public Sentence Left { get; }

        /// <summary>
        /// Gets the right side of the equivalence.
        /// </summary>
        public Sentence Right { get; }

        /// <inheritdoc/>
        public override bool Equals(Expression other) => other is Equivalence equivalence && this.Equals(equivalence);

        /// <inheritdoc/>
        public bool Equals(Equivalence other) => !(other is null) &&
            this.Left == other.Left &&
            this.Right == other.Right;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hash = HashCodeSeed;
            EquatableUtilities.Combine(ref hash, this.Left);
            EquatableUtilities.Combine(ref hash, this.Right);
            return hash;
        }

        /// <inheritdoc />
        public override void ToString(StringBuilder sb)
        {
            sb.Append("(<=> ");
            this.Left.ToString(sb);
            sb.Append(' ');
            this.Right.ToString(sb);
            sb.Append(')');
        }
    }
}
