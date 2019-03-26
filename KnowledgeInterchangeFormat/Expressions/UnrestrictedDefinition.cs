// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;

    /// <summary>
    /// Defines an unrestricted <see cref="Definition"/>.
    /// </summary>
    public abstract class UnrestrictedDefinition : Definition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnrestrictedDefinition"/> class.
        /// </summary>
        /// <param name="constant">The constant being defined.</param>
        /// <param name="description">An optional description of the definition.</param>
        /// <param name="sentences">The sentences in the definition.</param>
        protected UnrestrictedDefinition(Constant constant, CharacterString description, IEnumerable<Sentence> sentences)
            : base(constant, description)
        {
            this.Sentences = (sentences ?? throw new ArgumentNullException(nameof(sentences))).ToImmutableList();
        }

        /// <summary>
        /// Gets the sentences in the definition.
        /// </summary>
        public ImmutableList<Sentence> Sentences { get; }
    }
}
