// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;
    using System.Text;

    /// <summary>
    /// A pair consisting of a logical condition and a resulting value.
    /// </summary>
    public class LogicalPair : Expression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicalPair"/> class.
        /// </summary>
        /// <param name="condition">The condition to be tested.</param>
        /// <param name="value">The resulting value when the condition is true.</param>
        public LogicalPair(Sentence condition, Term value)
        {
            this.Condition = condition ?? throw new ArgumentNullException(nameof(condition));
            this.Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets the condition to be tested.
        /// </summary>
        public Sentence Condition { get; }

        /// <summary>
        /// Gets the resulting value when the condition is true.
        /// </summary>
        public Term Value { get; }

        /// <inheritdoc/>
        public override void ToString(StringBuilder sb)
        {
            this.Condition.ToString(sb);
            sb.Append(' ');
            this.Value.ToString(sb);
        }
    }
}
