﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp.CodeFixes;
using Xunit;

#pragma warning disable RCS1090

namespace Roslynator.CSharp.Analysis.Tests
{
    public class RCS1190JoinStringExpressionsTests : AbstractCSharpCodeFixVerifier
    {
        public override DiagnosticDescriptor Descriptor { get; } = DiagnosticDescriptors.JoinStringExpressions;

        public override DiagnosticAnalyzer Analyzer { get; } = new JoinStringExpressionsAnalyzer();

        public override CodeFixProvider FixProvider { get; } = new BinaryExpressionCodeFixProvider();

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.JoinStringExpressions)]
        public async Task Test_Literal_Regular()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M(string s)
    {
        s = [|""a"" + ""b"" + ""c""|];
    }
}
", @"
class C
{
    void M(string s)
    {
        s = ""abc"";
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.JoinStringExpressions)]
        public async Task Test_Literal_Regular2()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M(string s)
    {
        s = s + [|""a"" + ""b"" + ""c""|] + s;
    }
}
", @"
class C
{
    void M(string s)
    {
        s = s + ""abc"" + s;
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.JoinStringExpressions)]
        public async Task Test_Literal_Verbatim()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M(string s)
    {
        s = [|@""a"" + @""b"" + @""c""|];
    }
}
", @"
class C
{
    void M(string s)
    {
        s = @""abc"";
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.JoinStringExpressions)]
        public async Task Test_Literal_Verbatim2()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M(string s)
    {
        s = s + [|@""a"" + @""b"" + @""c""|] + s;
    }
}
", @"
class C
{
    void M(string s)
    {
        s = s + @""abc"" + s;
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.JoinStringExpressions)]
        public async Task Test_InterpolatedString_Regular()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M(string s)
    {
        s = [|$""a"" + $""b"" + $""c""|];
    }
}
", @"
class C
{
    void M(string s)
    {
        s = $""abc"";
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.JoinStringExpressions)]
        public async Task Test_InterpolatedString_Regular2()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M(string s)
    {
        s = s + [|$""a"" + $""b"" + $""c""|] + s;
    }
}
", @"
class C
{
    void M(string s)
    {
        s = s + $""abc"" + s;
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.JoinStringExpressions)]
        public async Task Test_InterpolatedString_Verbatim()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M(string s)
    {
        s = [|$@""a"" + $@""b"" + $@""c""|];
    }
}
", @"
class C
{
    void M(string s)
    {
        s = $@""abc"";
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.JoinStringExpressions)]
        public async Task Test_InterpolatedString_Verbatim2()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M(string s)
    {
        s = s + [|$@""a"" + $@""b"" + $@""c""|] + s;
    }
}
", @"
class C
{
    void M(string s)
    {
        s = s + $@""abc"" + s;
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.JoinStringExpressions)]
        public async Task Test_Verbatim_Multiline()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M(string s)
    {
            s = [|@""a"" + @""b
c"" + @""d
e""|];
    }
}
", @"
class C
{
    void M(string s)
    {
            s = @""ab
cd
e"";
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.JoinStringExpressions)]
        public async Task Test_InterpolatedString_Multiline()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M(string s)
    {
            s = [|$@""a"" + $@""b
c"" + $@""d
e""|];
    }
}
", @"
class C
{
    void M(string s)
    {
            s = $@""ab
cd
e"";
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.JoinStringExpressions)]
        public async Task TestNoDiagnostic_Regular_Multiline()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    void M(string s)
    {
        s = ""a""
            + ""b"";
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.JoinStringExpressions)]
        public async Task TestNoDiagnostic_RegularAndVerbatim()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    void M(string s)
    {
        s = ""a"" + @""b"";
        s = @""a"" + ""b"";
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.JoinStringExpressions)]
        public async Task TestNoDiagnostic_LiteralAndInterpolated()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    void M(string s)
    {
        s = ""a"" + $""b"";
        s = $""a"" + ""b"";
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.JoinStringExpressions)]
        public async Task TestNoDiagnostic_LiteralAndInterpolated_Verbatim()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    void M(string s)
    {
        s = @""a"" + $@""b"";
        s = $@""a"" + @""b"";
    }
}
");
        }

        [Fact, Trait(Traits.Analyzer, DiagnosticIdentifiers.JoinStringExpressions)]
        public async Task TestNoDiagnostic_AddExpressionIsNotStringConcatenation()
        {
            await VerifyNoDiagnosticAsync(@"
    class C
    {
        void M(string s)
        {
            s = default(C) + ""a"" + ""b"";
        }

        public static string operator +(C left, string right) => null;
    }
");
        }
    }
}
