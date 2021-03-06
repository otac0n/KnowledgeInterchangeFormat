// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A string of characters.
    /// </summary>
    public class CharacterString : ListTerm, IEquatable<CharacterString>
    {
        private const int HashCodeSeed = 0x7a0c747c;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterString"/> class.
        /// </summary>
        /// <param name="characters">The string of characters.</param>
        public CharacterString(string characters)
            : base((characters ?? throw new ArgumentNullException(nameof(characters))).Select(c => new CharacterReference(c)), null)
        {
            this.Characters = characters;
        }

        /// <summary>
        /// Gets the string of characters.
        /// </summary>
        public string Characters { get; }

        /// <inheritdoc/>
        public override bool Equals(Expression other) => other is CharacterString characterString && this.Equals(characterString);

        /// <inheritdoc/>
        public bool Equals(CharacterString other) => !(other is null) && this.Characters == other.Characters;

        /// <inheritdoc/>
        public override int GetHashCode() => this.Characters.GetHashCode() ^ HashCodeSeed;

        /// <inheritdoc />
        public override void ToString(StringBuilder sb)
        {
            var chars = this.Characters;
            var len = chars.Length;
            sb.EnsureCapacity(sb.Length + len + 2);
            sb.Append('"');

            for (var i = 0; i < len; i++)
            {
                var c = chars[i];
                switch (c)
                {
                    case '"':
                    case '\\':
                        sb.Append('\\');
                        goto default;
                    default:
                        sb.Append(c);
                        break;
                }
            }

            sb.Append('"');
        }
    }
}
