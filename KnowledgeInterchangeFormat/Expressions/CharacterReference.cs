// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System.Text;

    /// <summary>
    /// A reference to a single character.
    /// </summary>
    public class CharacterReference : Term
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterReference"/> class.
        /// </summary>
        /// <param name="character">The literal characger.</param>
        public CharacterReference(char character)
        {
            this.Character = character;
        }

        /// <summary>
        /// Gets the literal character.
        /// </summary>
        public char Character { get; }

        /// <inheritdoc/>
        public override void ToString(StringBuilder sb)
        {
            sb.Append("#\\");
            sb.Append(this.Character);
        }
    }
}
