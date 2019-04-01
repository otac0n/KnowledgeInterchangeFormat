// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// An unrestricted function definition.
    /// </summary>
    public class UnrestrictedFunctionDefinition : UnrestrictedDefinition, IEquatable<UnrestrictedFunctionDefinition>
    {
        private const int HashCodeSeed = unchecked((int)0x9cdaf42c);

        /// <summary>
        /// Initializes a new instance of the <see cref="UnrestrictedFunctionDefinition"/> class.
        /// </summary>
        /// <param name="constant">The constant being defined.</param>
        /// <param name="description">An optional description of the definition.</param>
        /// <param name="sentences">The sentences in the definition.</param>
        public UnrestrictedFunctionDefinition(Constant constant, CharacterString description, IEnumerable<Sentence> sentences)
            : base(constant, description, sentences)
        {
        }

        /// <inheritdoc/>
        public override bool Equals(Expression other) => other is UnrestrictedFunctionDefinition unrestrictedFunctionDefinition && this.Equals(unrestrictedFunctionDefinition);

        /// <inheritdoc/>
        public bool Equals(UnrestrictedFunctionDefinition other) => !(other is null) &&
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
            sb.Append("(deffunction ");

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
