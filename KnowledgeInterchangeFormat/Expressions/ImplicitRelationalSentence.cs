// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// A relation applied directly by name.
    /// </summary>
    public class ImplicitRelationalSentence : RelationalSentence
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImplicitRelationalSentence"/> class.
        /// </summary>
        /// <param name="relation">The term identifying the relation.</param>
        /// <param name="arguments">The arguments to the relation.</param>
        /// <param name="sequenceVariable">An optional <see cref="SequenceVariable"/> containing additional arguments.</param>
        public ImplicitRelationalSentence(Constant relation, IEnumerable<Term> arguments, SequenceVariable sequenceVariable)
            : base(relation, arguments, sequenceVariable)
        {
            this.Relation = relation;
        }

        /// <summary>
        /// Gets the relation to apply.
        /// </summary>
        public new Constant Relation { get; }

        /// <inheritdoc/>
        public override void ToString(StringBuilder sb)
        {
            sb.Append('(');

            this.Relation.ToString(sb);

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
