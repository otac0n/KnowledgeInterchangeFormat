// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Text;

    /// <summary>
    /// A <see cref="Sentence"/> involving the <c>and</c> operator.
    /// </summary>
    public class Conjunction : LogicalSentence
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Conjunction"/> class.
        /// </summary>
        /// <param name="conjuncts">The sentences in the conjunction.</param>
        public Conjunction(IEnumerable<Sentence> conjuncts)
        {
            this.Conjuncts = (conjuncts ?? throw new ArgumentNullException(nameof(conjuncts))).ToImmutableList();
        }

        /// <summary>
        /// Gets the sentences in the conjunction.
        /// </summary>
        public ImmutableList<Sentence> Conjuncts { get; }

        /// <inheritdoc/>
        public override void ToString(StringBuilder sb)
        {
            sb.Append("(and");

            foreach (var conjunct in this.Conjuncts)
            {
                sb.Append(' ');
                conjunct.ToString(sb);
            }

            sb.Append(')');
        }
    }
}
