// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;
    using System.Text;

    /// <summary>
    /// A partial logical definition.
    /// </summary>
    public class ReversePartialLogicalDefinition : PartialDefinition, IEquatable<ReversePartialLogicalDefinition>
    {
        private const int HashCodeSeed = 0x3e1bec57;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReversePartialLogicalDefinition"/> class.
        /// </summary>
        /// <param name="constant">The constant being defined.</param>
        /// <param name="description">An optional description of the definition.</param>
        /// <param name="sentence">The defining sentence.</param>
        public ReversePartialLogicalDefinition(Constant constant, CharacterString description, Sentence sentence)
            : base(constant, description)
        {
            this.Sentence = sentence ?? throw new ArgumentNullException(nameof(sentence));
        }

        /// <summary>
        /// Gets the defining sentence.
        /// </summary>
        public Sentence Sentence { get; }

        /// <inheritdoc/>
        public override bool Equals(Expression other) => other is ReversePartialLogicalDefinition reversePartialLogicalDefinition && this.Equals(reversePartialLogicalDefinition);

        /// <inheritdoc/>
        public bool Equals(ReversePartialLogicalDefinition other) => !(other is null) &&
            this.Constant == other.Constant &&
            this.Description == other.Description &&
            this.Sentence == other.Sentence;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hash = HashCodeSeed;
            EquatableUtilities.Combine(ref hash, this.Constant);
            EquatableUtilities.Combine(ref hash, this.Description);
            EquatableUtilities.Combine(ref hash, this.Sentence);
            return hash;
        }

        /// <inheritdoc/>
        public override void ToString(StringBuilder sb)
        {
            sb.Append("(deflogical ");

            this.Constant.ToString(sb);

            if (this.Description != null)
            {
                sb.Append(' ');
                this.Description.ToString(sb);
            }

            sb.Append(" :<= ");

            this.Sentence.ToString(sb);

            sb.Append(')');
        }
    }
}
