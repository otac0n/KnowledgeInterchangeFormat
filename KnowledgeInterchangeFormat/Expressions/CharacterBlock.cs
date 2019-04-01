// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A block of characters.
    /// </summary>
    public class CharacterBlock : ListTerm, IEquatable<CharacterBlock>
    {
        private const int HashCodeSeed = 0x6deb67fe;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterBlock"/> class.
        /// </summary>
        /// <param name="characters">The block of characters.</param>
        public CharacterBlock(string characters)
            : base((characters ?? throw new ArgumentNullException(nameof(characters))).Select(c => new CharacterReference(c)), null)
        {
            this.Characters = characters;
        }

        /// <summary>
        /// Gets the block of characters.
        /// </summary>
        public string Characters { get; }

        /// <inheritdoc/>
        public override bool Equals(Expression other) => other is CharacterBlock characterBlock && this.Equals(characterBlock);

        /// <inheritdoc/>
        public bool Equals(CharacterBlock other) => !(other is null) && this.Characters == other.Characters;

        /// <inheritdoc/>
        public override int GetHashCode() => this.Characters.GetHashCode() ^ HashCodeSeed;

        /// <inheritdoc/>
        public override void ToString(StringBuilder sb)
        {
            var chars = this.Characters;
            var len = chars.Length.ToString(CultureInfo.InvariantCulture);
            sb.EnsureCapacity(sb.Length + len.Length + chars.Length + 2);
            sb.Append('#');
            sb.Append(len);
            sb.Append('q');
            sb.Append(chars);
        }
    }
}
