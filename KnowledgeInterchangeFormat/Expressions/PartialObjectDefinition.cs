// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;
    using System.Text;

    /// <summary>
    /// A partial object definition.
    /// </summary>
    public class PartialObjectDefinition : PartialDefinition, IEquatable<PartialObjectDefinition>
    {
        private const int HashCodeSeed = unchecked((int)0xaf0865e4);

        /// <summary>
        /// Initializes a new instance of the <see cref="PartialObjectDefinition"/> class.
        /// </summary>
        /// <param name="constant">The constant being defined.</param>
        /// <param name="description">An optional description of the definition.</param>
        /// <param name="variable">The individual variable.</param>
        /// <param name="sentence">The defining sentence.</param>
        public PartialObjectDefinition(Constant constant, CharacterString description, IndividualVariable variable, Sentence sentence)
            : base(constant, description)
        {
            this.Variable = variable ?? throw new ArgumentNullException(nameof(variable));
            this.Sentence = sentence ?? throw new ArgumentNullException(nameof(sentence));
        }

        /// <summary>
        /// Gets the defining sentence.
        /// </summary>
        public Sentence Sentence { get; }

        /// <summary>
        /// Gets the individual variable.
        /// </summary>
        public IndividualVariable Variable { get; }

        /// <inheritdoc/>
        public override bool Equals(Expression other) => other is PartialObjectDefinition partialObjectDefinition && this.Equals(partialObjectDefinition);

        /// <inheritdoc/>
        public bool Equals(PartialObjectDefinition other) => !(other is null) &&
            this.Constant == other.Constant &&
            this.Description == other.Description &&
            this.Variable == other.Variable &&
            this.Sentence == other.Sentence;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hash = HashCodeSeed;
            EquatableUtilities.Combine(ref hash, this.Constant);
            EquatableUtilities.Combine(ref hash, this.Description);
            EquatableUtilities.Combine(ref hash, this.Variable);
            EquatableUtilities.Combine(ref hash, this.Sentence);
            return hash;
        }

        /// <inheritdoc/>
        public override void ToString(StringBuilder sb)
        {
            sb.Append("(defobject ");

            this.Constant.ToString(sb);

            if (this.Description != null)
            {
                sb.Append(' ');
                this.Description.ToString(sb);
            }

            sb.Append(" :-> ");

            this.Variable.ToString(sb);

            sb.Append(" :=> ");

            this.Sentence.ToString(sb);

            sb.Append(')');
        }
    }
}
