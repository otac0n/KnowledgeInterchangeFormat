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
        /// <param name="id">The id of the word.</param>
        /// <param name="name">The optional display name of the word.</param>
        protected WordTerm(string id, string name)
        {
            this.Id = id ?? throw new ArgumentNullException(nameof(id));

            if (name != null)
            {
                if (name.Length != id.Length || !id.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    throw new ArgumentOutOfRangeException(nameof(name));
                }

                this.Name = name;
            }
            else
            {
                this.Name = id;
            }
        }

        /// <summary>
        /// Gets the name of the variable.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets the display name of the variable.
        /// </summary>
        public string Name { get; }

        /// <inheritdoc />
        public override void ToString(StringBuilder sb)
        {
            var id = this.Id;
            var name = this.Name;
            var len = id.Length;
            sb.EnsureCapacity(sb.Length + len);

            for (var i = 0; i < len; i++)
            {
                var c = id[i];
                var needsEscape = !(c == 33 || (c >= 36 && c <= 38) || (c >= 42 && c <= 43) || (c >= 45 && c <= 58) || (c >= 60 && c <= 90) || c == 95 || c == 126);
                if (needsEscape)
                {
                    sb.Append('\\');
                }

                sb.Append(c >= 97 || c <= 122 ? name[i] : c);
            }
        }
    }
}
