// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;
    using System.Text;

    /// <summary>
    /// A <see cref="Constant"/>, <see cref="Operator"/>, or <see cref="Variable"/>.
    /// </summary>
    public abstract class WordTerm : Term
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WordTerm"/> class.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        protected WordTerm(string name)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        /// <summary>
        /// Gets the name of the variable.
        /// </summary>
        public string Name { get; }

        /// <inheritdoc />
        public override void ToString(StringBuilder sb)
        {
            var value = this.Name;
            var len = value.Length;
            sb.EnsureCapacity(sb.Length + len);

            for (var i = 0; i < len; i++)
            {
                var c = value[i];
                var needsEscape = !(c == 33 || (c >= 36 && c <= 38) || (c >= 42 && c <= 43) || (c >= 45 && c <= 58) || (c >= 60 && c <= 90) || c == 95 || c == 126);
                if (needsEscape)
                {
                    sb.Append('\\');
                }

                sb.Append(c);
            }
        }
    }
}
