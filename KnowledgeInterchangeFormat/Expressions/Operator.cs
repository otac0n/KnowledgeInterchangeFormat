// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    /// <summary>
    /// Represents an operator.
    /// </summary>
    public class Operator : WordTerm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Operator"/> class.
        /// </summary>
        /// <param name="name">The name of the operator.</param>
        public Operator(string name)
            : base(name)
        {
        }
    }
}
