// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;

    /// <summary>
    /// Represents an operator.
    /// </summary>
    public class Operator : WordTerm, IEquatable<Operator>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Operator"/> class.
        /// </summary>
        /// <param name="id">The id of the operator.</param>
        /// <param name="name">The optional display name of the operator.</param>
        public Operator(string id, string name = null)
            : base(id, name)
        {
        }

        /// <inheritdoc />
        public override bool Equals(Expression other) => other is Operator @operator && this.Equals(@operator);

        /// <inheritdoc />
        public bool Equals(Operator other) => !(other is null) && this.Id == other.Id;
    }
}
