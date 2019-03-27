// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    /// <summary>
    /// A constant <see cref="Term"/>.
    /// </summary>
    public class Constant : WordTerm
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
    }
}
