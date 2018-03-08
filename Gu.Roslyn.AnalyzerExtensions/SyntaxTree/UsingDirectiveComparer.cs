namespace Gu.Roslyn.AnalyzerExtensions.SyntaxTree
{
    using System;
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public class UsingDirectiveComparer : IComparer<UsingDirectiveSyntax>
    {
        public static readonly UsingDirectiveComparer Default = new UsingDirectiveComparer();
        private static readonly StringComparer OrdinalIgnoreCase = StringComparer.OrdinalIgnoreCase;

        public static int Compare(UsingDirectiveSyntax x, UsingDirectiveSyntax y)
        {
            if (TryGetRoot(x.Name, out var xn) &&
                TryGetRoot(y.Name, out var yn))
            {
                var xText = xn.Identifier.ValueText;
                var yText = yn.Identifier.ValueText;
                if (xText != yText)
                {
                    if (xText == "System")
                    {
                        return 1;
                    }

                    if (yText == "System")
                    {
                        return -1;
                    }

                    return OrdinalIgnoreCase.Compare(xText, yText);
                }

                return CompareRecursive(xn.Parent as QualifiedNameSyntax, yn.Parent as QualifiedNameSyntax);
                while (true)
                {
                

                    var xqn = xn.Parent as QualifiedNameSyntax;
                    var yqn = yn.Parent as QualifiedNameSyntax;
                    if (xqn == null && yqn == null)
                    {
                        break;
                    }

                    if (xqn != null && yqn != null)
                    {
                        xn = xqn.Right;
                        yn = yqn.Right;
                        continue;
                    }

                    if (xqn == null)
                    {
                        return -1;
                    }

                    if (yqn == null)
                    {
                        return 1;
                    }
                }
            }

            return 0;
        }

        int IComparer<UsingDirectiveSyntax>.Compare(UsingDirectiveSyntax x, UsingDirectiveSyntax y) => Compare(x, y);

        private static bool TryGetRoot(NameSyntax uds, out SimpleNameSyntax name)
        {
            if (uds is SimpleNameSyntax simpleName)
            {
                name = simpleName;
                return true;
            }

            if (uds is QualifiedNameSyntax qns)
            {
                return TryGetRoot(qns.Left, out name);
            }

            name = null;
            return false;
        }

        private static int CompareRecursive(QualifiedNameSyntax xqn, QualifiedNameSyntax yqn)
        {
            if (xqn == null && yqn == null)
            {
                return 0;
            }
        }
    }
}
