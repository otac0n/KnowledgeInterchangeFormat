// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Text;

    /// <summary>
    /// A partial relation definition.
    /// </summary>
    public class PartialRelationDefinition : PartialDefinition, IEquatable<PartialRelationDefinition>
    {
        private const int HashCodeSeed = unchecked((int)0xb8d24410);

        /// <summary>
        /// Initializes a new instance of the <see cref="PartialRelationDefinition"/> class.
        /// </summary>
        /// <param name="constant">The constant being defined.</param>
        /// <param name="description">An optional description of the definition.</param>
        /// <param name="parameters">The parameters of the relation.</param>
        /// <param name="sequenceVariable">An optional <see cref="SequenceVariable"/> containing additional parameters.</param>
        /// <param name="sentence">The defining sentence.</param>
        public PartialRelationDefinition(Constant constant, CharacterString description, IEnumerable<IndividualVariable> parameters, SequenceVariable sequenceVariable, Sentence sentence)
            : base(constant, description)
        {
            this.Parameters = (parameters ?? throw new ArgumentNullException(nameof(parameters))).ToImmutableList();
            this.SequenceVariable = sequenceVariable;
            this.Sentence = sentence ?? throw new ArgumentNullException(nameof(sentence));
        }

        /// <summary>
        /// Gets the parameters of the relation.
        /// </summary>
        public ImmutableList<IndividualVariable> Parameters { get; }

        /// <summary>
        /// Gets an optional <see cref="SequenceVariable"/> containing additional parameters.
        /// </summary>
        public SequenceVariable SequenceVariable { get; }

        /// <summary>
        /// Gets the defining sentence.
        /// </summary>
        public Sentence Sentence { get; }

        /// <inheritdoc/>
        public override bool Equals(Expression other) => other is PartialRelationDefinition partialRelationDefinition && this.Equals(partialRelationDefinition);

        /// <inheritdoc/>
        public bool Equals(PartialRelationDefinition other) => !(other is null) &&
            this.Parameters.Count == other.Parameters.Count &&
            this.Constant == other.Constant &&
            this.Description == other.Description &&
            this.SequenceVariable == other.SequenceVariable &&
            this.Sentence == other.Sentence &&
            EquatableUtilities.ListsEqual(this.Parameters, other.Parameters);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hash = HashCodeSeed;
            EquatableUtilities.Combine(ref hash, this.Constant);
            EquatableUtilities.Combine(ref hash, this.Description);
            EquatableUtilities.Combine(ref hash, EquatableUtilities.HashList(this.Parameters));
            EquatableUtilities.Combine(ref hash, this.SequenceVariable);
            EquatableUtilities.Combine(ref hash, this.Sentence);
            return hash;
        }

        /// <inheritdoc/>
        public override void ToString(StringBuilder sb)
        {
            sb.Append("(defrelation ");

            this.Constant.ToString(sb);

            sb.Append(" (");

            var first = true;
            foreach (var param in this.Parameters)
            {
                if (!first)
                {
                    sb.Append(' ');
                }

                param.ToString(sb);
                first = false;
            }

            if (this.SequenceVariable != null)
            {
                if (!first)
                {
                    sb.Append(' ');
                }

                this.SequenceVariable.ToString(sb);
            }

            sb.Append(") ");

            if (this.Description != null)
            {
                this.Description.ToString(sb);
                sb.Append(' ');
            }

            sb.Append(":=> ");

            this.Sentence.ToString(sb);

            sb.Append(')');
        }
    }
}
