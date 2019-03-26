// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Text;

    /// <summary>
    /// A partial function definition.
    /// </summary>
    public class PartialFunctionDefinition : PartialDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PartialFunctionDefinition"/> class.
        /// </summary>
        /// <param name="constant">The constant being defined.</param>
        /// <param name="description">An optional description of the definition.</param>
        /// <param name="parameters">The parameters of the function.</param>
        /// <param name="sequenceVariable">An optional <see cref="SequenceVariable"/> containing additional parameters.</param>
        /// <param name="variable">The individual variable.</param>
        /// <param name="sentence">The defining sentence.</param>
        public PartialFunctionDefinition(Constant constant, CharacterString description, IEnumerable<IndividualVariable> parameters, SequenceVariable sequenceVariable, IndividualVariable variable, Sentence sentence)
            : base(constant, description)
        {
            this.Parameters = (parameters ?? throw new ArgumentNullException(nameof(parameters))).ToImmutableList();
            this.SequenceVariable = sequenceVariable;
            this.Variable = variable ?? throw new ArgumentNullException(nameof(variable));
            this.Sentence = sentence ?? throw new ArgumentNullException(nameof(sentence));
        }

        /// <summary>
        /// Gets the parameters of the function.
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

        /// <summary>
        /// Gets the individual variable.
        /// </summary>
        public IndividualVariable Variable { get; }

        /// <inheritdoc/>
        public override void ToString(StringBuilder sb)
        {
            sb.Append("(deffunction ");

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

            sb.Append(":-> ");

            this.Variable.ToString(sb);

            sb.Append(" :=> ");

            this.Sentence.ToString(sb);

            sb.Append(')');
        }
    }
}
