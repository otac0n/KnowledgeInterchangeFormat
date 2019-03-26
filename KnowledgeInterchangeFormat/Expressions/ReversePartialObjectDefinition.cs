// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;
    using System.Text;

    /// <summary>
    /// A partial object definition.
    /// </summary>
    public class ReversePartialObjectDefinition : PartialDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReversePartialObjectDefinition"/> class.
        /// </summary>
        /// <param name="constant">The constant being defined.</param>
        /// <param name="description">An optional description of the definition.</param>
        /// <param name="variable">The individual variable.</param>
        /// <param name="sentence">The defining sentence.</param>
        public ReversePartialObjectDefinition(Constant constant, CharacterString description, IndividualVariable variable, Sentence sentence)
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

            sb.Append(" :<= ");

            this.Sentence.ToString(sb);

            sb.Append(')');
        }
    }
}
