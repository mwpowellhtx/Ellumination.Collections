using System;
using System.Diagnostics;
using CodeGeneration.Roslyn;

/// <summary>
/// Signals to the Analyzers and Generators that further action could or should be taken.
/// Basically this is the Enumerations analog to the <see cref="FlagsAttribute"/>, and signals
/// the Generators to auto-generate boilerplate code making it possible for Bitwise operators
/// to be used in your application.
/// </summary>
/// <inheritdoc />
[AttributeUsage(AttributeTargets.Class, Inherited = false)
// No, this is not a typo; it really is AttributeAttribute...
 , CodeGenerationAttribute("Ellumination.Collections.FlagsEnumerationGenerator"
                           + ", Ellumination.Collections.Enumerations.Generators")
 , Conditional("CodeGeneration")]
// ReSharper disable once CheckNamespace, UnusedMember.Global
public sealed class FlagsEnumerationAttribute : Attribute
{
}
