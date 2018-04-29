namespace Gu.Roslyn.AnalyzerExtensions.Tests.Symbols
{
    using Gu.Roslyn.Asserts;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using NUnit.Framework;

    public class TypeSymbolTests
    {
        public class Is
        {
            [Test]
            public void Sandbox()
            {
                var code = @"
namespace RoslynSandbox
{
    public class Foo
    {
        public int Bar { get; }
    }
}";
                var syntaxTree = CSharpSyntaxTree.ParseText(code);
                var compilation = CSharpCompilation.Create("test", new[] { syntaxTree }, new[] { MetadataReference.CreateFromFile(typeof(int).Assembly.Location), });
                var semanticModel = compilation.GetSemanticModel(syntaxTree);
                var type = semanticModel.GetDeclaredSymbol(syntaxTree.FindPropertyDeclaration("Bar")).Type;
                var containingCompilation = type.ContainingModule.GlobalNamespace.ContainingCompilation;
            }

            [TestCase("int value1, int value2")]
            [TestCase("int value1, int? value2")]
            [TestCase("int value1, double value2")]
            [TestCase("int value1, System.IComparable value2")]
            [TestCase("int value1, System.IComparable<int> value2")]
            [TestCase("int value1, object value2")]
            [TestCase("System.Collections.Generic.IEnumerable<int> value1, System.Collections.Generic.IEnumerable<int> value2")]
            [TestCase("System.Collections.Generic.IEnumerable<int> value1, System.Collections.IEnumerable value2")]
            public void WhenTrueUsingCompilation(string parameters)
            {
                var code = @"
namespace RoslynSandbox
{
    public class Foo
    {
        public Foo(int value1, int value2)
        {
        }
    }
}";
                code = code.AssertReplace("int value1, int value2", parameters);
                var syntaxTree = CSharpSyntaxTree.ParseText(code);
                var compilation = CSharpCompilation.Create("test", new[] { syntaxTree }, MetadataReferences.FromAttributes());
                var semanticModel = compilation.GetSemanticModel(syntaxTree);
                var ctor = semanticModel.GetDeclaredSymbol(syntaxTree.FindConstructorDeclaration("Foo"));
                var type1 = ctor.Parameters[0].Type;
                var type2 = ctor.Parameters[1].Type;
                Assert.AreEqual(true, type1.Is(type2, compilation));
            }

            [TestCase("int value1, int value2")]
            [TestCase("int value1, int? value2")]
            [TestCase("int value1, System.IComparable value2")]
            [TestCase("int value1, System.IComparable<int> value2")]
            [TestCase("int value1, object value2")]
            [TestCase("System.Collections.Generic.IEnumerable<int> value1, System.Collections.Generic.IEnumerable<int> value2")]
            [TestCase("System.Collections.Generic.IEnumerable<int> value1, System.Collections.IEnumerable value2")]
            public void WhenTrue(string parameters)
            {
                var code = @"
namespace RoslynSandbox
{
    public class Foo
    {
        public Foo(int value1, int value2)
        {
        }
    }
}";
                code = code.AssertReplace("int value1, int value2", parameters);
                var syntaxTree = CSharpSyntaxTree.ParseText(code);
                var compilation = CSharpCompilation.Create("test", new[] { syntaxTree }, MetadataReferences.FromAttributes());
                var semanticModel = compilation.GetSemanticModel(syntaxTree);
                var ctor = semanticModel.GetDeclaredSymbol(syntaxTree.FindConstructorDeclaration("Foo"));
                var type1 = ctor.Parameters[0].Type;
                var type2 = ctor.Parameters[1].Type;
                Assert.AreEqual(true, type1.Is(type2));
            }

            [TestCase("int value", "System.Int32")]
            [TestCase("int value", "System.IComparable")]
            [TestCase("int value", "System.IComparable`1")]
            [TestCase("System.Collections.Generic.IEnumerable<int> value", "System.Collections.Generic.IEnumerable`1")]
            [TestCase("System.Collections.Generic.IEnumerable<int> value", "System.Collections.IEnumerable")]
            public void WhenTrueQualifiedType(string parameters, string typeName)
            {
                var code = @"
namespace RoslynSandbox
{
    public class Foo
    {
        public Foo(int value)
        {
        }
    }
}";
                code = code.AssertReplace("int value", parameters);
                var syntaxTree = CSharpSyntaxTree.ParseText(code);
                var compilation = CSharpCompilation.Create("test", new[] { syntaxTree }, MetadataReferences.FromAttributes());
                var semanticModel = compilation.GetSemanticModel(syntaxTree);
                var ctor = semanticModel.GetDeclaredSymbol(syntaxTree.FindConstructorDeclaration("Foo"));
                var type = ctor.Parameters[0].Type;
                var qualifiedType = new QualifiedType(typeName);
                Assert.AreEqual(true, type.Is(qualifiedType));
            }

            [Test]
            public void Inheritance()
            {
                var code = @"
namespace RoslynSandbox
{
    class A
    {
    }

    class B : A
    {
    }
}";
                var syntaxTree = CSharpSyntaxTree.ParseText(code);
                var compilation = CSharpCompilation.Create("test", new[] { syntaxTree }, MetadataReferences.FromAttributes());
                var semanticModel = compilation.GetSemanticModel(syntaxTree);
                var a = semanticModel.GetDeclaredSymbol(syntaxTree.FindClassDeclaration("A"));
                var b = semanticModel.GetDeclaredSymbol(syntaxTree.FindClassDeclaration("B"));
                Assert.AreEqual(false, a.Is(b));
                Assert.AreEqual(true, b.Is(a));
            }

            [Test]
            public void InheritsGeneric()
            {
                var code = @"
namespace RoslynSandbox
{
    class A<T>
    {
    }

    class B : A<int>
    {
    }
}";
                var syntaxTree = CSharpSyntaxTree.ParseText(code);
                var compilation = CSharpCompilation.Create("test", new[] { syntaxTree }, MetadataReferences.FromAttributes());
                var semanticModel = compilation.GetSemanticModel(syntaxTree);
                var a = semanticModel.GetDeclaredSymbol(syntaxTree.FindClassDeclaration("A"));
                var b = semanticModel.GetDeclaredSymbol(syntaxTree.FindClassDeclaration("B"));
                Assert.AreEqual(false, a.Is(b));
                Assert.AreEqual(true, b.Is(a));
            }
        }
    }
}
