// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;
    using System.Text;

    /// <summary>
    /// Describes a variable used in a <see cref="QuantifiedSentence"/>.
    /// </summary>
    public class VariableSpecification : Expression, IEquatable<VariableSpecification>
    {
        private const int HashCodeSeed = unchecked((int)0x920f95a1);

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableSpecification"/> class.
        /// </summary>
        /// <param name="variable">The variable used in a <see cref="QuantifiedSentence"/>.</param>
        /// <param name="constant">An optional constant restricting the domain of <see cref="Variable"/>.</param>
        public VariableSpecification(Variable variable, Constant constant)
        {
            this.Variable = variable ?? throw new ArgumentNullException(nameof(variable));
            this.Constant = constant;
        }

        /// <summary>
        /// Gets an optional constant restricting the domain of <see cref="Variable"/>.
        /// </summary>
        public Constant Constant { get; }

        /// <summary>
        /// Gets the variable used in a <see cref="QuantifiedSentence"/>.
        /// </summary>
        public Variable Variable { get; }

        /// <inheritdoc/>
        public override bool Equals(Expression other) => other is VariableSpecification variableSpecification && this.Equals(variableSpecification);

        /// <inheritdoc/>
        public bool Equals(VariableSpecification other) => !(other is null) &&
            this.Constant == other.Constant &&
            this.Variable == other.Variable;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hash = HashCodeSeed;
            EquatableUtilities.Combine(ref hash, this.Constant);
            EquatableUtilities.Combine(ref hash, this.Variable);
            return hash;
        }

        /// <inheritdoc/>
        public override void ToString(StringBuilder sb)
        {
            if (this.Constant != null)
            {
                sb.Append('(');
                this.Variable.ToString(sb);
                sb.Append(' ');
                this.Constant.ToString(sb);
                sb.Append(')');
            }
            else
            {
                this.Variable.ToString(sb);
            }
        }
    }
}
