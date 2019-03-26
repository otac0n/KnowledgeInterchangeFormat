// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// A function invoked directly by name.
    /// </summary>
    public class ImplicitFunctionalTerm : FunctionalTerm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImplicitFunctionalTerm"/> class.
        /// </summary>
        /// <param name="function">The function to invoke.</param>
        /// <param name="arguments">The arguments to the function.</param>
        /// <param name="sequenceVariable">An optional <see cref="SequenceVariable"/> containing additional arguments.</param>
        public ImplicitFunctionalTerm(Constant function, IEnumerable<Term> arguments, SequenceVariable sequenceVariable)
            : base(function, arguments, sequenceVariable)
        {
            this.Function = function;
        }

        /// <summary>
        /// Gets the function to invoke.
        /// </summary>
        public new Constant Function { get; }

        /// <inheritdoc/>
        public override void ToString(StringBuilder sb)
        {
            sb.Append('(');

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
