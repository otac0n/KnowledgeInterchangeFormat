// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;

    /// <summary>
    /// An <see cref="ExistentiallyQuantifiedSentence"/> or a <see cref="UniversallyQuantifiedSentence"/>.
    /// </summary>
    public abstract class QuantifiedSentence : Sentence
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuantifiedSentence"/> class.
        /// </summary>
        /// <param name="variables">The variables used in the quantified sentence.</param>
        /// <param name="quantified">The quantified sentence.</param>
        protected QuantifiedSentence(IEnumerable<VariableSpecification> variables, Sentence quantified)
        {
            this.Variables = (variables ?? throw new ArgumentNullException(nameof(variables))).ToImmutableList();
            this.Quantified = quantified ?? throw new ArgumentNullException(nameof(quantified));
        }

        /// <summary>
        /// Gets the quantified sentence.
        /// </summary>
        public Sentence Quantified { get; }

        /// <summary>
        /// Gets the variables used in the quantified sentence.
        /// </summary>
        public ImmutableList<VariableSpecification> Variables { get; }
    }
}
