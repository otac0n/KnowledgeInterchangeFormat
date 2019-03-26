// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;
    using System.Text;

    /// <summary>
    /// An arbitrary quoted expression.
    /// </summary>
    public class Quotation : Term
    {
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
        public override void ToString(StringBuilder sb)
        {
            sb.Append('\'');
            this.Quoted.ToString(sb);
        }
    }
}
