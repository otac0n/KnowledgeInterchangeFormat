// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Text;

    /// <summary>
    /// A complete function definition.
    /// </summary>
    public class CompleteFunctionDefinition : CompleteDefinition, IEquatable<CompleteFunctionDefinition>
    {
        private const int HashCodeSeed = unchecked((int)0xb6c7a883);

        /// <summary>
        /// Initializes a new instance of the <see cref="CompleteFunctionDefinition"/> class.
        /// </summary>
        /// <param name="constant">The constant being defined.</param>
        /// <param name="description">An optional description of the definition.</param>
        /// <param name="parameters">The parameters of the function.</param>
        /// <param name="sequenceVariable">An optional <see cref="SequenceVariable"/> containing additional parameters.</param>
        /// <param name="term">The defining term.</param>
        public CompleteFunctionDefinition(Constant constant, CharacterString description, IEnumerable<IndividualVariable> parameters, SequenceVariable sequenceVariable, Term term)
            : base(constant, description)
        {
            this.Parameters = (parameters ?? throw new ArgumentNullException(nameof(parameters))).ToImmutableList();
            this.SequenceVariable = sequenceVariable;
            this.Term = term ?? throw new ArgumentNullException(nameof(term));
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
        /// Gets the defining term.
        /// </summary>
        public Term Term { get; }

        /// <inheritdoc/>
        public override bool Equals(Expression other) => other is CompleteFunctionDefinition completeFunctionDefinition && this.Equals(completeFunctionDefinition);

        /// <inheritdoc/>
        public bool Equals(CompleteFunctionDefinition other) => !(other is null) &&
            this.Parameters.Count == other.Parameters.Count &&
            this.Constant == other.Constant &&
            this.Description == other.Description &&
            this.Term == other.Term &&
            this.SequenceVariable == other.SequenceVariable &&
            EquatableUtilities.ListsEqual(this.Parameters, other.Parameters);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hash = HashCodeSeed;
            EquatableUtilities.Combine(ref hash, this.Constant);
            EquatableUtilities.Combine(ref hash, this.Description);
            EquatableUtilities.Combine(ref hash, EquatableUtilities.HashList(this.Parameters));
            EquatableUtilities.Combine(ref hash, this.SequenceVariable);
            EquatableUtilities.Combine(ref hash, this.Term);
            return hash;
        }

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

            sb.Append(":= ");

            this.Term.ToString(sb);

            sb.Append(')');
        }
    }
}
