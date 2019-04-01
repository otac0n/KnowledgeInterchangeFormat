// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Text;

    /// <summary>
    /// A finite set of <see cref="Form">forms</see>.
    /// </summary>
    public class KnowledgeBase : Expression, IEquatable<KnowledgeBase>
    {
        private const int HashCodeSeed = 0x5eea08d0;

        /// <summary>
        /// Initializes a new instance of the <see cref="KnowledgeBase"/> class.
        /// </summary>
        /// <param name="forms">The <see cref="Form">forms</see> in the knowledge base.</param>
        public KnowledgeBase(IEnumerable<Form> forms)
        {
            this.Forms = forms.ToImmutableList();
        }

        /// <summary>
        /// Gets the <see cref="Form">forms</see> in the knowledge base.
        /// </summary>
        public ImmutableList<Form> Forms { get; }

        /// <inheritdoc/>
        public override bool Equals(Expression other) => other is KnowledgeBase knowledgeBase && this.Equals(knowledgeBase);

        /// <inheritdoc/>
        public bool Equals(KnowledgeBase other) => !(other is null) &&
            this.Forms.Count == other.Forms.Count &&
            EquatableUtilities.ListsEqual(this.Forms, other.Forms);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hash = HashCodeSeed;
            EquatableUtilities.Combine(ref hash, EquatableUtilities.HashList(this.Forms));
            return hash;
        }

        /// <inheritdoc/>
        public override void ToString(StringBuilder sb)
        {
            foreach (var form in this.Forms)
            {
                if (sb.Length > 0)
                {
                    sb.AppendLine();
                }

                form.ToString(sb);
            }
        }
    }
}
