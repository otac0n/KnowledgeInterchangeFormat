// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;
    using System.Text;

    /// <summary>
    /// A complete object definition.
    /// </summary>
    public class CompleteObjectDefinition : CompleteDefinition, IEquatable<CompleteObjectDefinition>
    {
        private const int HashCodeSeed = 0x3332591d;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompleteObjectDefinition"/> class.
        /// </summary>
        /// <param name="constant">The constant being defined.</param>
        /// <param name="description">An optional description of the definition.</param>
        /// <param name="term">The defining term.</param>
        public CompleteObjectDefinition(Constant constant, CharacterString description, Term term)
            : base(constant, description)
        {
            this.Term = term ?? throw new ArgumentNullException(nameof(term));
        }

        /// <summary>
        /// Gets the defining term.
        /// </summary>
        public Term Term { get; }

        /// <inheritdoc/>
        public override bool Equals(Expression other) => other is CompleteObjectDefinition completeObjectDefinition && this.Equals(completeObjectDefinition);

        /// <inheritdoc/>
        public bool Equals(CompleteObjectDefinition other) => !(other is null) &&
            this.Constant == other.Constant &&
            this.Description == other.Description &&
            this.Term == other.Term;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hash = HashCodeSeed;
            EquatableUtilities.Combine(ref hash, this.Constant);
            EquatableUtilities.Combine(ref hash, this.Description);
            EquatableUtilities.Combine(ref hash, this.Term);
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

            sb.Append(" := ");

            this.Term.ToString(sb);

            sb.Append(')');
        }
    }
}
