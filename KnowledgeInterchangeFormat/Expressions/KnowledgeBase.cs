// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Text;

    /// <summary>
    /// A finite set of <see cref="Form">forms</see>.
    /// </summary>
    public class KnowledgeBase : Expression
    {
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
