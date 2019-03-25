// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System.Text;

    /// <summary>
    /// An <see cref="IndividualVariable"/> or a <see cref="SequenceVariable"/>.
    /// </summary>
    public abstract class Variable : WordTerm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Variable"/> class.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        protected Variable(string name)
            : base(name)
        {
        }
    }
}
