// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Text;

    /// <summary>
    /// An <see cref="Expression"/> that contains a list of other expressions.
    /// </summary>
    public class ListExpression : Expression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListExpression"/> class.
        /// </summary>
        /// <param name="items">The items contained in the list.</param>
        public ListExpression(IEnumerable<Expression> items)
        {
            this.Items = (items ?? throw new ArgumentNullException(nameof(items))).ToImmutableList();
        }

        /// <summary>
        /// Gets the items contained in the list.
        /// </summary>
        public ImmutableList<Expression> Items { get; }

        /// <inheritdoc/>
        public override void ToString(StringBuilder sb)
        {
            sb.Append('(');

            var first = true;
            foreach (var item in this.Items)
            {
                if (!first)
                {
                    sb.Append(' ');
                }

                item.ToString(sb);
                first = false;
            }

            sb.Append(')');
        }
    }
}
