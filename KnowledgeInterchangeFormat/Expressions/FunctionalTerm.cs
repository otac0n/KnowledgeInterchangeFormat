// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;

    /// <summary>
    /// An <see cref="ExplicitFunctionalTerm"/> or an <see cref="ImplicitFunctionalTerm"/>.
    /// </summary>
    public abstract class FunctionalTerm : Term
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionalTerm"/> class.
        /// </summary>
        /// <param name="function">The term identifying the function.</param>
        /// <param name="arguments">The arguments to the function.</param>
        /// <param name="sequenceVariable">An optional <see cref="SequenceVariable"/> containing additional arguments.</param>
        public FunctionalTerm(Term function, IEnumerable<Term> arguments, SequenceVariable sequenceVariable)
        {
            this.Function = function ?? throw new ArgumentNullException(nameof(function));
            this.Arguments = (arguments ?? throw new ArgumentNullException(nameof(arguments))).ToImmutableList();
            this.SequenceVariable = sequenceVariable;
        }

        /// <summary>
        /// Gets the arguments to the function.
        /// </summary>
        public ImmutableList<Term> Arguments { get; }

        /// <summary>
        /// Gets the term identifying the function.
        /// </summary>
        public Term Function { get; }

        /// <summary>
        /// Gets an optional <see cref="SequenceVariable"/> containing additional arguments.
        /// </summary>
        public SequenceVariable SequenceVariable { get; }
    }
}
