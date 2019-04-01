// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// A <see cref="QuantifiedSentence"/> using the <c>exists</c> operator.
    /// </summary>
    public class ExistentiallyQuantifiedSentence : QuantifiedSentence, IEquatable<ExistentiallyQuantifiedSentence>
    {
        private const int HashCodeSeed = 0x237e001c;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExistentiallyQuantifiedSentence"/> class.
        /// </summary>
        /// <param name="variables">The variables used in the quantified sentence.</param>
        /// <param name="quantified">The quantified sentence.</param>
        public ExistentiallyQuantifiedSentence(IEnumerable<VariableSpecification> variables, Sentence quantified)
            : base(variables, quantified)
        {
        }

        /// <inheritdoc/>
        public override bool Equals(Expression other) => other is ExistentiallyQuantifiedSentence existentiallyQuantifiedSentence && this.Equals(existentiallyQuantifiedSentence);

        /// <inheritdoc/>
        public bool Equals(ExistentiallyQuantifiedSentence other) => !(other is null) &&
            this.Variables.Count == other.Variables.Count &&
            this.Quantified == other.Quantified &&
            EquatableUtilities.ListsEqual(this.Variables, other.Variables);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hash = HashCodeSeed;
            EquatableUtilities.Combine(ref hash, EquatableUtilities.HashList(this.Variables));
            EquatableUtilities.Combine(ref hash, this.Quantified);
            return hash;
        }

        /// <inheritdoc/>
        public override void ToString(StringBuilder sb)
        {
            sb.Append("(exists (");

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
