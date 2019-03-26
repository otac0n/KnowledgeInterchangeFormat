// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// A <see cref="QuantifiedSentence"/> using the <c>forall</c> operator.
    /// </summary>
    public class UniversallyQuantifiedSentence : QuantifiedSentence
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UniversallyQuantifiedSentence"/> class.
        /// </summary>
        /// <param name="variables">The variables used in the quantified sentence.</param>
        /// <param name="quantified">The quantified sentence.</param>
        public UniversallyQuantifiedSentence(IEnumerable<VariableSpecification> variables, Sentence quantified)
            : base(variables, quantified)
        {
        }

        /// <inheritdoc/>
        public override void ToString(StringBuilder sb)
        {
            sb.Append("(forall (");

            var first = true;
            foreach (var variable in this.Variables)
            {
                if (!first)
                {
                    sb.Append(' ');
                }

                variable.ToString(sb);
                first = false;
            }

            sb.Append(") ");

            this.Quantified.ToString(sb);

            sb.Append(')');
        }
    }
}
