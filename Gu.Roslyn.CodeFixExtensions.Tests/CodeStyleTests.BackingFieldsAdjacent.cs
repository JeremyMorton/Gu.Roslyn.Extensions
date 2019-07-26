namespace Gu.Roslyn.CodeFixExtensions.Tests
{
    using Microsoft.CodeAnalysis.CSharp;
    using NUnit.Framework;

    public partial class CodeStyleTests
    {
        public class BackingFieldsAdjacent
        {
            [Test]
            public void DefaultsToStyleCopEmptyClass()
            {
                var syntaxTree = CSharpSyntaxTree.ParseText(@"
namespace N
{
    public class C
    {
    }
}");
                var compilation = CSharpCompilation.Create("test", new[] { syntaxTree });
                var semanticModel = compilation.GetSemanticModel(syntaxTree);
                Assert.AreEqual(false, CodeStyle.BackingFieldsAdjacent(semanticModel, out _));
            }

            [Test]
            public void DefaultsToStyleCopWhenOneProperty()
            {
                var syntaxTree = CSharpSyntaxTree.ParseText(@"
namespace N
{
    public class C
    {
        private int value1;

        public int Value1
        {
            get => this.value1;
            set => this.value1 = value;
        }
    }
}");
                var compilation = CSharpCompilation.Create("test", new[] { syntaxTree });
                var semanticModel = compilation.GetSemanticModel(syntaxTree);
                Assert.AreEqual(false, CodeStyle.BackingFieldsAdjacent(semanticModel, out _));
            }

            [Test]
            public void FindsAdjacentInCompilation()
            {
                var syntaxTree1 = CSharpSyntaxTree.ParseText(@"
namespace N
{
    public class C
    {
    }
}");

                var syntaxTree2 = CSharpSyntaxTree.ParseText(@"
namespace N
{
    public class C
    {
        public C()
        {
        }

        private int value1;

        public int Value1
        {
            get => this.value1;
            set => this.value1 = value;
        }
    }
}");
                var compilation = CSharpCompilation.Create("test", new[] { syntaxTree1, syntaxTree2 });
                var semanticModel = compilation.GetSemanticModel(syntaxTree1);
                Assert.AreEqual(true, CodeStyle.BackingFieldsAdjacent(semanticModel, out var newLine));
                Assert.AreEqual(true, newLine);
            }

            [Test]
            public void FindsStyleCopInCompilation()
            {
                var syntaxTree1 = CSharpSyntaxTree.ParseText(@"
namespace N
{
    public class C
    {
    }
}");

                var syntaxTree2 = CSharpSyntaxTree.ParseText(@"
namespace N
{
    public class C
    {
        private int value1;

        public C()
        {
        }

        public int Value1
        {
            get => this.value1;
            set => this.value1 = value;
        }
    }
}");
                var compilation = CSharpCompilation.Create("test", new[] { syntaxTree1, syntaxTree2 });
                var semanticModel = compilation.GetSemanticModel(syntaxTree1);
                Assert.AreEqual(false, CodeStyle.BackingFieldsAdjacent(semanticModel, out _));
            }

            [Test]
            public void WhenStyleCop()
            {
                var syntaxTree = CSharpSyntaxTree.ParseText(@"
namespace N
{
    public class C
    {
        private int value1;

        public C()
        {
        }

        public int Value1
        {
            get => this.value1;
            set => this.value1 = value;
        }
    }
}");
                var compilation = CSharpCompilation.Create("test", new[] { syntaxTree });
                var semanticModel = compilation.GetSemanticModel(syntaxTree);
                Assert.AreEqual(false, CodeStyle.BackingFieldsAdjacent(semanticModel, out _));
            }

            [Test]
            public void WhenAdjacentNoEmptyLine()
            {
                var syntaxTree = CSharpSyntaxTree.ParseText(@"
namespace N
{
    public class C
    {
        public C()
        {
        }

        private int value1;
        public int Value1
        {
            get => this.value1;
            set => this.value1 = value;
        }
    }
}");
                var compilation = CSharpCompilation.Create("test", new[] { syntaxTree });
                var semanticModel = compilation.GetSemanticModel(syntaxTree);
                Assert.AreEqual(true, CodeStyle.BackingFieldsAdjacent(semanticModel, out var newLineBetween));
                Assert.AreEqual(false, newLineBetween);
            }

            [Test]
            public void WhenAdjacentWithEmptyLine()
            {
                var syntaxTree = CSharpSyntaxTree.ParseText(@"
namespace N
{
    public class C
    {
        public C()
        {
        }

        private int value1;

        public int Value1
        {
            get => this.value1;
            set => this.value1 = value;
        }
    }
}");
                var compilation = CSharpCompilation.Create("test", new[] { syntaxTree });
                var semanticModel = compilation.GetSemanticModel(syntaxTree);
                Assert.AreEqual(true, CodeStyle.BackingFieldsAdjacent(semanticModel, out var newLineBetween));
                Assert.AreEqual(true, newLineBetween);
            }
        }
    }
}
