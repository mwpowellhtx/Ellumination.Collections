using System;

namespace Ellumination.CodeAnalysis.Verifiers
{
    /// <summary>
    /// Location where the diagnostic appears, as determined by path, line number, and column number.
    /// </summary>
    public struct DiagnosticResultLocation
    {
        /// <summary>
        /// Gets the Path.
        /// </summary>
        internal string Path { get; }

        /// <summary>
        /// Gets the Line
        /// </summary>
        internal int Line { get; }

        /// <summary>
        /// Gets the Column.
        /// </summary>
        internal int Column { get; }

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="path"></param>
        /// <param name="line"></param>
        /// <param name="column"></param>
        public DiagnosticResultLocation(string path, int line, int column)
        {
            void VerifyIndex(string name, int value, int minimumValue = -1)
            {
                if (value >= minimumValue)
                {
                    return;
                }

                throw new ArgumentOutOfRangeException(name, $"{name} ({value}) must be >= {minimumValue}");
            }

            VerifyIndex(nameof(line), line);
            VerifyIndex(nameof(column), column);

            Path = path;
            Line = line;
            Column = column;
        }

        /// <summary>
        /// Returns the Created <see cref="DiagnosticResultLocation"/> given
        /// <paramref name="path"/>, <paramref name="line"/>, and <paramref name="column"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="line"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static DiagnosticResultLocation Create(string path, int line, int column)
            => new DiagnosticResultLocation(path, line, column);
    }
}
