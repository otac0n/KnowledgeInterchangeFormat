// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;
    using System.Text;

    /// <summary>
    /// A complete logical definition.
    /// </summary>
    public class CompleteLogicalDefinition : CompleteDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompleteLogicalDefinition"/> class.
        /// </summary>
        /// <param name="constant">The constant being defined.</param>
        /// <param name="description">An optional description of the definition.</param>
        /// <param name="sentence">The defining sentence.</param>
        public CompleteLogicalDefinition(Constant constant, CharacterString description, Sentence sentence)
            : base(constant, description)
        {
            this.Sentence = sentence ?? throw new ArgumentNullException(nameof(sentence));
        }

        /// <summary>
        /// Gets the defining sentence.
        /// </summary>
        public Sentence Sentence { get; }

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

            sb.Append(" := ");

            this.Sentence.ToString(sb);

            sb.Append(')');
        }
    }
}
