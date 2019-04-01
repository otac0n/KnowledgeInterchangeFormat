// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// An unrestricted object definition.
    /// </summary>
    public class UnrestrictedObjectDefinition : UnrestrictedDefinition, IEquatable<UnrestrictedObjectDefinition>
    {
        private const int HashCodeSeed = 0x610137f7;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnrestrictedObjectDefinition"/> class.
        /// </summary>
        /// <param name="constant">The constant being defined.</param>
        /// <param name="description">An optional description of the definition.</param>
        /// <param name="sentences">The sentences in the definition.</param>
        public UnrestrictedObjectDefinition(Constant constant, CharacterString description, IEnumerable<Sentence> sentences)
            : base(constant, description, sentences)
        {
        }

        /// <inheritdoc/>
        public override bool Equals(Expression other) => other is UnrestrictedObjectDefinition unrestrictedObjectDefinition && this.Equals(unrestrictedObjectDefinition);

        /// <inheritdoc/>
        public bool Equals(UnrestrictedObjectDefinition other) => !(other is null) &&
            this.Sentences.Count == other.Sentences.Count &&
            this.Constant == other.Constant &&
            this.Description == other.Description &&
            EquatableUtilities.ListsEqual(this.Sentences, other.Sentences);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hash = HashCodeSeed;
            EquatableUtilities.Combine(ref hash, this.Constant);
            EquatableUtilities.Combine(ref hash, this.Description);
            EquatableUtilities.Combine(ref hash, EquatableUtilities.HashList(this.Sentences));
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

            foreach (var sentence in this.Sentences)
            {
                sb.Append(' ');
                sentence.ToString(sb);
            }

            sb.Append(')');
        }
    }
}
