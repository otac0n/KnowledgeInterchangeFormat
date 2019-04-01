// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace KnowledgeInterchangeFormat
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Provides utility methods for composing comparable objects.
    /// </summary>
    internal static class EquatableUtilities
    {
        /// <summary>
        /// Combines a running current hash code with an other hash code.
        /// </summary>
        /// <param name="hash">The current hash code.</param>
        /// <param name="other">The other hash code to add.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Combine(ref int hash, object other) => hash = (hash * 33) + (other is null ? 0 : other.GetHashCode());

        public static int HashList(IEnumerable<object> items)
        {
            if (items is null)
            {
                return 0;
            }
            else
            {
                var hash = 17;

                foreach (var item in items)
                {
                    Combine(ref hash, item);
                }

                return hash;
            }
        }

        public static bool ListsEqual<T>(IEnumerable<T> a, IEnumerable<T> b)
        {
            IEnumerator<T> aEnumerator = null;
            try
            {
                aEnumerator = a.GetEnumerator();
                IEnumerator<T> bEnumerator = null;
                try
                {
                    bEnumerator = b.GetEnumerator();

                    do
                    {
                        var aNext = aEnumerator.MoveNext();
                        var bNext = bEnumerator.MoveNext();
                        if (aNext != bNext)
                        {
                            return false;
                        }

                        if (!aNext)
                        {
                            break;
                        }

                        if (!object.Equals(aEnumerator.Current, bEnumerator.Current))
                        {
                            return false;
                        }
                    }
                    while (true);

                    return true;
                }
                finally
                {
                    if (aEnumerator is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }
                }
            }
            finally
            {
                if (aEnumerator is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }
    }
}
