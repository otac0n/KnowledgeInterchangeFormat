// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;

    /// <summary>
    /// Used to define constants.
    /// </summary>
    public abstract class Definition : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Definition"/> class.
        /// </summary>
        /// <param name="constant">The constant being defined.</param>
        /// <param name="description">An optional description of the definition.</param>
        protected Definition(Constant constant, CharacterString description)
        {
            this.Constant = constant ?? throw new ArgumentNullException(nameof(constant));
            this.Description = description;
        }

        /// <summary>
        /// Gets the constant being defined.
        /// </summary>
        public Constant Constant { get; }

        /// <summary>
        /// Gets an optional description of the definition.
        /// </summary>
        public CharacterString Description { get; }
    }
}
