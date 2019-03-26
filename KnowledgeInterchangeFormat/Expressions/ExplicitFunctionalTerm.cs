// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// A function invoked via the <c>value</c> operator.
    /// </summary>
    public class ExplicitFunctionalTerm : FunctionalTerm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExplicitFunctionalTerm"/> class.
        /// </summary>
        /// <param name="function">The term identifying the function.</param>
        /// <param name="arguments">The arguments to the function.</param>
        /// <param name="sequenceVariable">An optional <see cref="SequenceVariable"/> containing additional arguments.</param>
        public ExplicitFunctionalTerm(Term function, IEnumerable<Term> arguments, SequenceVariable sequenceVariable)
            : base(function, arguments, sequenceVariable)
        {
        }

        /// <inheritdoc/>
        public override void ToString(StringBuilder sb)
        {
            sb.Append("(value ");

            this.Function.ToString(sb);

            foreach (var arg in this.Arguments)
            {
                sb.Append(' ');
                arg.ToString(sb);
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
