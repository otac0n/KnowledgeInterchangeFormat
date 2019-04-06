// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat
{
    using System.Collections.Generic;

    public partial class KifParser
    {
        private readonly HashSet<string> operators = new HashSet<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="KifParser"/> class.
        /// </summary>
        public KifParser()
            : this(new Options())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KifParser"/> class.
        /// </summary>
        /// <param name="options">The parsing options.</param>
        public KifParser(Options options)
        {
            this.operators.Add("NOT");
            this.operators.Add("AND");
            this.operators.Add("OR");
            this.operators.Add("=>");
            this.operators.Add("<=");
            this.operators.Add("<=>");

            if (options.ExplicitOperators)
            {
                this.operators.Add("VALUE");
                this.operators.Add("HOLDS");
            }

            if (options.ListOperator)
            {
                this.operators.Add("LISTOF");
            }

            if (options.QuoteOperator)
            {
                this.operators.Add("QUOTE");
            }

            if (options.LogicalOperators)
            {
                this.operators.Add("IF");
                this.operators.Add("COND");
            }

            if (options.EqualityOperators)
            {
                this.operators.Add("=");
                this.operators.Add("/=");
            }

            if (options.QuantifiedOperators)
            {
                this.operators.Add("FORALL");
                this.operators.Add("EXISTS");
            }

            if (options.DefinitionOperators)
            {
                this.operators.Add("DEFOBJECT");
                this.operators.Add("DEFFUNCTION");
                this.operators.Add("DEFRELATION");
                this.operators.Add("DEFLOGICAL");
                this.operators.Add(":=");
                this.operators.Add(":->");
                this.operators.Add(":<=");
                this.operators.Add(":=>");
            }
        }

        /// <summary>
        /// Contains the supported parsing options.
        /// </summary>
        public class Options
        {
            /// <summary>
            /// Gets or sets a value indicating whether equality operators (<c>deffunction</c>, <c>deflogical</c>, <c>defobject</c>, and <c>defrelation</c>) are supported.
            /// </summary>
            public bool DefinitionOperators { get; set; } = true;

            /// <summary>
            /// Gets or sets a value indicating whether equality operators (<c>=</c> and <c>/=</c>) are supported.
            /// </summary>
            public bool EqualityOperators { get; set; } = true;

            /// <summary>
            /// Gets or sets a value indicating whether explicit operators (<c>value</c> and <c>holds</c>) are supported.
            /// </summary>
            public bool ExplicitOperators { get; set; } = true;

            /// <summary>
            /// Gets or sets a value indicating whether the list operator (<c>listof</c>) is supported.
            /// </summary>
            public bool ListOperator { get; set; } = true;

            /// <summary>
            /// Gets or sets a value indicating whether logical operators (<c>if</c> and <c>cond</c>) are supported.
            /// </summary>
            public bool LogicalOperators { get; set; } = true;

            /// <summary>
            /// Gets or sets a value indicating whether quantified operators (<c>forall</c> and <c>exists</c>) are supported.
            /// </summary>
            public bool QuantifiedOperators { get; set; } = true;

            /// <summary>
            /// Gets or sets a value indicating whether the quote operator (<c>quote</c>) is supported.
            /// </summary>
            /// <remarks>
            /// Quotes created using the <c>'</c> operator are still supported, but can be trivially detected with an <see cref="ExpressionTreeWalker"/>.
            /// </remarks>
            public bool QuoteOperator { get; set; } = true;
        }
    }
}
