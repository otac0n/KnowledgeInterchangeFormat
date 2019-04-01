// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// A relation applied directly by name.
    /// </summary>
    public class ImplicitRelationalSentence : RelationalSentence, IEquatable<ImplicitRelationalSentence>
    {
        private const int HashCodeSeed = unchecked((int)0xace0988a);

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
        public override bool Equals(Expression other) => other is ImplicitRelationalSentence implicitRelationalSentence && this.Equals(implicitRelationalSentence);

        /// <inheritdoc/>
        public bool Equals(ImplicitRelationalSentence other) => !(other is null) &&
            this.Arguments.Count == other.Arguments.Count &&
            this.Relation == other.Relation &&
            this.SequenceVariable == other.SequenceVariable &&
            EquatableUtilities.ListsEqual(this.Arguments, other.Arguments);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hash = HashCodeSeed;
            EquatableUtilities.Combine(ref hash, this.Relation);
            EquatableUtilities.Combine(ref hash, EquatableUtilities.HashList(this.Arguments));
            EquatableUtilities.Combine(ref hash, this.SequenceVariable);
            return hash;
        }

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
