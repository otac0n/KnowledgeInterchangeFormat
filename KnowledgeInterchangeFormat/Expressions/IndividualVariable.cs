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
        /// <param name="name">The name of the variable.</param>
        public IndividualVariable(string name)
            : base(name)
        {
        }
    }
}
