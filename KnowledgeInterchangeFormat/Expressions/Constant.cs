// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;

    /// <summary>
    /// A constant <see cref="Term"/>.
    /// </summary>
    public class Constant : WordTerm, IEquatable<Constant>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Constant"/> class.
        /// </summary>
        /// <param name="id">The id of the constant.</param>
        /// <param name="name">The optional display name of the constant.</param>
        public Constant(string id, string name = null)
            : base(id, name)
        {
        }

        /// <inheritdoc />
        public override bool Equals(Expression other) => other is Constant constant && this.Equals(constant);

        /// <inheritdoc />
        public bool Equals(Constant other) => !(other is null) && this.Id == other.Id;
    }
}
