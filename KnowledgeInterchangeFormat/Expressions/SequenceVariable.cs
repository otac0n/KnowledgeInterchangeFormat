// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    /// <summary>
    /// A sequence variable.
    /// </summary>
    public class SequenceVariable : Variable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SequenceVariable"/> class.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        public SequenceVariable(string name)
            : base(name)
        {
        }
    }
}
