// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    /// <summary>
    /// Defines a partial <see cref="Definition"/>.
    /// </summary>
    public abstract class PartialDefinition : Definition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PartialDefinition"/> class.
        /// </summary>
        /// <param name="constant">The constant being defined.</param>
        /// <param name="description">An optional description of the definition.</param>
        protected PartialDefinition(Constant constant, CharacterString description)
            : base(constant, description)
        {
        }
    }
}
