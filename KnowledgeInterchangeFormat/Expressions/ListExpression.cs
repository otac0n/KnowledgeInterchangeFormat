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
    public class ListExpression : Expression, IEquatable<ListExpression>
    {
        private const int HashCodeSeed = 0x1309c9b1;

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
        public override bool Equals(Expression other) => other is ListExpression listExpression && this.Equals(listExpression);

        /// <inheritdoc/>
        public bool Equals(ListExpression other) => !(other is null) &&
            this.Items.Count == other.Items.Count &&
            EquatableUtilities.ListsEqual(this.Items, other.Items);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hash = HashCodeSeed;
            EquatableUtilities.Combine(ref hash, EquatableUtilities.HashList(this.Items));
            return hash;
        }

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
