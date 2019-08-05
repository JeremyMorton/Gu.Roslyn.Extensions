namespace Gu.Roslyn.CodeFixExtensions.Tests.DocumentEditorExtTests
{
    using System.Linq;
    using System.Threading.Tasks;
    using Gu.Roslyn.Asserts;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Editing;
    using NUnit.Framework;

    public static class MakeSealed
    {
        [TestCase("class C", "sealed class C")]
        [TestCase("public class C", "public sealed class C")]
        public static async Task EmptyClass(string before, string after)
        {
            var code = @"
namespace N
{
    class C
    {
    }
}".AssertReplace("class C", before);
            var sln = CodeFactory.CreateSolution(code);
            var editor = await DocumentEditor.CreateAsync(sln.Projects.First().Documents.First()).ConfigureAwait(false);

            var expected = @"
namespace N
{
    sealed class C
    {
    }
}".AssertReplace("sealed class C", after);
            _ = editor.Seal(editor.OriginalRoot.Find<ClassDeclarationSyntax>("class C"));
            CodeAssert.AreEqual(expected, editor.GetChangedDocument());
        }

        [TestCase("protected int f;", "private int f;")]
        [TestCase("protected int P { get; }", "private int P { get; }")]
        [TestCase("protected virtual int P { get; }", "private int P { get; }")]
        [TestCase("protected int P { get; private set; }", "private int P { get; set; }")]
        [TestCase("protected void M { }", "private void M { }")]
        [TestCase("protected virtual void M { }", "private void M { }")]
        public static async Task WithProtectedMember(string before, string after)
        {
            var code = @"
namespace N
{
    class C
    {
        protected int f;
    }
}".AssertReplace("protected int f;", before);
            var sln = CodeFactory.CreateSolution(code);
            var editor = await DocumentEditor.CreateAsync(sln.Projects.First().Documents.First()).ConfigureAwait(false);

            var expected = @"
namespace N
{
    sealed class C
    {
        private int f;
    }
}".AssertReplace("private int f;", after);
            _ = editor.Seal(editor.OriginalRoot.Find<ClassDeclarationSyntax>("class C"));
            CodeAssert.AreEqual(expected, editor.GetChangedDocument());
        }
    }
}
