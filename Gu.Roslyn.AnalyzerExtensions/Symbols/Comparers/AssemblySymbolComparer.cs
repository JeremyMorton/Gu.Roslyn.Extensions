namespace Gu.Roslyn.AnalyzerExtensions
{
    using System;
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;

    public class AssemblySymbolComparer : IEqualityComparer<IAssemblySymbol>
    {
        public static readonly AssemblySymbolComparer Default = new AssemblySymbolComparer();

        private AssemblySymbolComparer()
        {
        }

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
        [Obsolete("Should only be called with arguments of type IAssemblySymbol.", error: true)]
        public static new bool Equals(object _, object __) => throw new InvalidOperationException("This is hidden so that it is not called by accident.");
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