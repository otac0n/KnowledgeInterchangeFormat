// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// A <see cref="Sentence"/> involving the <c>&lt;=</c> operator.
    /// </summary>
    public class ReverseImplication : Implication
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReverseImplication"/> class.
        /// </summary>
        /// <param name="consequent">The consequent of the implication.</param>
        /// <param name="antecedents">The antecedents of the implication.</param>
        public ReverseImplication(Sentence consequent, IEnumerable<Sentence> antecedents)
            : base(antecedents, consequent)
        {
        }

        /// <inheritdoc />
        public override void ToString(StringBuilder sb)
        {
            sb.Append("(<= ");

            this.Consequent.ToString(sb);

            foreach (var antecedent in this.Antecedents)
            {
                sb.Append(' ');
                antecedent.ToString(sb);
            }

            sb.Append(')');
        }
    }
}
