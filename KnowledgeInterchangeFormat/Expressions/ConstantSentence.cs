// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat.Expressions
{
    using System;
    using System.Text;

    /// <summary>
    /// A <see cref="Constant"/> used in a <see cref="Sentence"/>.
    /// </summary>
    public class ConstantSentence : Sentence
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantSentence"/> class.
        /// </summary>
        /// <param name="constant">The constant used in this sentence.</param>
        public ConstantSentence(Constant constant)
        {
            this.Constant = constant ?? throw new ArgumentNullException(nameof(constant));
        }

        /// <summary>
        /// Gets the constant used in this sentence.
        /// </summary>
        public Constant Constant { get; }

        /// <inheritdoc/>
        public override void ToString(StringBuilder sb) => this.Constant.ToString(sb);
    }
}
