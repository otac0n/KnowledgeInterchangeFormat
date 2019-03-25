// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Text;

    /// <summary>
    /// A <see cref="Sentence"/> involving the <c>or</c> operator.
    /// </summary>
    public class Disjunction : LogicalSentence
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Disjunction"/> class.
        /// </summary>
        /// <param name="disjuncts">The sentences in the disjunction.</param>
        public Disjunction(IEnumerable<Sentence> disjuncts)
        {
            this.Disjuncts = (disjuncts ?? throw new ArgumentNullException(nameof(disjuncts))).ToImmutableList();
        }

        /// <summary>
        /// Gets the sentences in the disjunction.
        /// </summary>
        public ImmutableList<Sentence> Disjuncts { get; }

        /// <inheritdoc />
        public override void ToString(StringBuilder sb)
        {
            sb.Append("(or");

            foreach (var disjunct in this.Disjuncts)
            {
                sb.Append(' ');
                disjunct.ToString(sb);
            }

            sb.Append(')');
        }
    }
}
