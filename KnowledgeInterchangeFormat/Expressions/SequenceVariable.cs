// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;

    /// <summary>
    /// A sequence variable.
    /// </summary>
    public class SequenceVariable : Variable, IEquatable<SequenceVariable>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SequenceVariable"/> class.
        /// </summary>
        /// <param name="id">The id of the variable.</param>
        /// <param name="name">The optional display name of the variable.</param>
        public SequenceVariable(string id, string name = null)
            : base(id, name)
        {
        }

        /// <inheritdoc />
        public override bool Equals(Expression other) => other is SequenceVariable sequenceVariable && this.Equals(sequenceVariable);

        /// <inheritdoc />
        public bool Equals(SequenceVariable other) => !(other is null) && this.Id == other.Id;
    }
}
