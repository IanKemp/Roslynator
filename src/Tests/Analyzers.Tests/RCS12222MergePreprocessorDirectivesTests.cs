﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslynator.CSharp;
using Roslynator.CSharp.Analysis;
using Roslynator.CSharp.CodeFixes;
using Roslynator.Tests.CSharp;
using Xunit;

#pragma warning disable RCS1090

namespace Roslynator.Analyzers.Tests
{
    public class RCS12222MergePreprocessorDirectivesTests : CSharpCodeFixVerifier
    {
        public override DiagnosticDescriptor Descriptor { get; } = DiagnosticDescriptors.MergePreprocessorDirectives;

        public override DiagnosticAnalyzer Analyzer { get; } = new MergePreprocessorDirectivesAnalyzer();

        public override CodeFixProvider FixProvider { get; } = new DirectiveTriviaCodeFixProvider();

        [Fact]
        public async Task TestDiagnosticWithFix_Disable()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M()
    {
[|#pragma warning disable RCS0|]
#pragma warning disable RCS1, RCS2,
    
#pragma warning disable RCS3, RCS4, RCS5
#pragma warning restore RCS0
    }
}
", @"
class C
{
    void M()
    {
#pragma warning disable RCS0, RCS1, RCS2, RCS3, RCS4, RCS5
#pragma warning restore RCS0
    }
}
");
        }

        [Fact]
        public async Task TestDiagnosticWithFix_Restore()
        {
            await VerifyDiagnosticAndFixAsync(@"
class C
{
    void M()
    {
[|#pragma warning restore RCS0|]
#pragma warning restore RCS1, RCS2,
    
#pragma warning restore RCS3, RCS4, RCS5
#pragma warning disable RCS0
    }
}
", @"
class C
{
    void M()
    {
#pragma warning restore RCS0, RCS1, RCS2, RCS3, RCS4, RCS5
#pragma warning disable RCS0
    }
}
");
        }

        [Fact]
        public async Task TestNoDiagnostic()
        {
            await VerifyNoDiagnosticAsync(@"
class C
{
    void M()
    {
#pragma warning disable RCS0
    }
#pragma warning disable RCS0
#pragma warning restore RCS0
}
");
        }
    }
}
