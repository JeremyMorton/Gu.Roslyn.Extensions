namespace Gu.Roslyn.AnalyzerExtensions
{
    using System;
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;

    /// <inheritdoc />
    public sealed class AssemblySymbolComparer : IEqualityComparer<IAssemblySymbol>
    {
        /// <summary> The default instance. </summary>
        public static readonly AssemblySymbolComparer Default = new AssemblySymbolComparer();

        private AssemblySymbolComparer()
        {
        }

        /// <summary> Compares equality by name. </summary>
        /// <param name="x">The first instance.</param>
        /// <param name="y">The other instance.</param>
        /// <returns>True if the instances are found equal.</returns>
        public static bool Equals(IAssemblySymbol x, IAssemblySymbol y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x == null ||
                y == null)
            {
                return false;
            }

            return x.MetadataName == y.MetadataName;
        }

        //// ReSharper disable once UnusedMember.Global
        //// ReSharper disable UnusedParameter.Global
#pragma warning disable SA1313 // Parameter names must begin with lower-case letter
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable SA1600 // Elements must be documented
#pragma warning disable CA1707 // Identifiers should not contain underscores
        [Obsolete("Should only be called with arguments of type IAssemblySymbol.", error: true)]
        public static new bool Equals(object _, object __) => throw new InvalidOperationException("This is hidden so that it is not called by accident.");
#pragma warning restore CA1707 // Identifiers should not contain underscores
#pragma warning restore SA1600 // Elements must be documented
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning restore SA1313 // Parameter names must begin with lower-case letter
        //// ReSharper restore UnusedParameter.Global

        /// <inheritdoc />
        bool IEqualityComparer<IAssemblySymbol>.Equals(IAssemblySymbol x, IAssemblySymbol y) => Equals(x, y);

        /// <inheritdoc />
        public int GetHashCode(IAssemblySymbol obj)
        {
            return obj?.MetadataName.GetHashCode() ?? 0;
        }
    }
}
