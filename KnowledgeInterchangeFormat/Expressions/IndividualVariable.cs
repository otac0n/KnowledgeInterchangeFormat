// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    /// <summary>
    /// An individual variable.
    /// </summary>
    public class IndividualVariable : Variable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IndividualVariable"/> class.
        /// </summary>
        /// <param name="id">The name of the variable.</param>
        /// <param name="name">The optional display name of the variable.</param>
        public IndividualVariable(string id, string name = null)
            : base(id, name)
        {
        }
    }
}
