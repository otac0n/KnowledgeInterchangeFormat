// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;

    /// <summary>
    /// An <see cref="ExplicitRelationalSentence"/> or an <see cref="ImplicitRelationalSentence"/>.
    /// </summary>
    public abstract class RelationalSentence : Sentence
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RelationalSentence"/> class.
        /// </summary>
        /// <param name="relation">The term identifying the relation.</param>
        /// <param name="arguments">The arguments to the relation.</param>
        /// <param name="sequenceVariable">An optional <see cref="SequenceVariable"/> containing additional arguments.</param>
        public RelationalSentence(Term relation, IEnumerable<Term> arguments, SequenceVariable sequenceVariable)
        {
            this.Relation = relation ?? throw new ArgumentNullException(nameof(relation));
            this.Arguments = (arguments ?? throw new ArgumentNullException(nameof(arguments))).ToImmutableList();
            this.SequenceVariable = sequenceVariable;
        }

        /// <summary>
        /// Gets the arguments to the relation.
        /// </summary>
        public ImmutableList<Term> Arguments { get; }

        /// <summary>
        /// Gets the term identifying the relation.
        /// </summary>
        public Term Relation { get; }

        /// <summary>
        /// Gets an optional <see cref="SequenceVariable"/> containing additional arguments.
        /// </summary>
        public SequenceVariable SequenceVariable { get; }
    }
}
