// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;
    using System.Text;

    /// <summary>
    /// An arbitrary quoted expression.
    /// </summary>
    public class Quotation : Term, IEquatable<Quotation>
    {
        private const int HashCodeSeed = unchecked((int)0xecbb4d87);

        /// <summary>
        /// Initializes a new instance of the <see cref="Quotation"/> class.
        /// </summary>
        /// <param name="quoted">The quoted expression.</param>
        public Quotation(Expression quoted)
        {
            this.Quoted = quoted ?? throw new ArgumentNullException(nameof(quoted));
        }

        /// <summary>
        /// Gets the quoted expression.
        /// </summary>
        public Expression Quoted { get; }

        /// <inheritdoc/>
        public override bool Equals(Expression other) => other is Quotation quotation && this.Equals(quotation);

        /// <inheritdoc/>
        public bool Equals(Quotation other) => !(other is null) &&
            this.Quoted == other.Quoted;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hash = HashCodeSeed;
            EquatableUtilities.Combine(ref hash, this.Quoted);
            return hash;
        }

        /// <inheritdoc/>
        public override void ToString(StringBuilder sb)
        {
            sb.Append('\'');
            this.Quoted.ToString(sb);
        }
    }
}
