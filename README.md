# Conferences / Talks

I am available to deliver talks on our Enumerations as well as general Collections approach. Remote is preferred, we can coordinate from one to however many attendees our agreed upon conferencing solution may support.

# NuGet Package Situation Report

The following are the currently deployed packages for our Collections projects.

[![ImmutableBitArray package](https://img.shields.io/nuget/v/Kingdom.Collections.ImmutableBitArray.svg?label=Kingdom.Collections.ImmutableBitArray%20NuGet%20Package)](https://nuget.org/packages/Kingdom.Collections.ImmutableBitArray)
[![Enumerations package](https://img.shields.io/nuget/v/Kingdom.Collections.Enumerations.svg?label=Kingdom.Collections.Enumerations%20NuGet%20Package)](https://nuget.org/packages/Kingdom.Collections.Enumerations)
[![Enumerations Tests package](https://img.shields.io/nuget/v/Kingdom.Collections.Enumerations.Tests.svg?label=Kingdom.Collections.Enumerations.Tests%20NuGet%20Package)](https://nuget.org/packages/Kingdom.Collections.Enumerations.Tests)
[![Enumerations Attributes package](https://img.shields.io/nuget/v/Kingdom.Collections.Enumerations.Attributes.svg?label=Kingdom.Collections.Enumerations.Attributes%20NuGet%20Package)](https://nuget.org/packages/Kingdom.Collections.Enumerations.Attributes)
[![Enumerations Analyzers package](https://img.shields.io/nuget/v/Kingdom.Collections.Enumerations.Analyzers.svg?label=Kingdom.Collections.Enumerations.Analyzers%20NuGet%20Package)](https://nuget.org/packages/Kingdom.Collections.Enumerations.Analyzers)
[![Enumerations Generators package](https://img.shields.io/nuget/v/Kingdom.Collections.Enumerations.Generators.svg?label=Kingdom.Collections.Enumerations.Generators%20NuGet%20Package)](https://nuget.org/packages/Kingdom.Collections.Enumerations.Generators)
[![Enumerations BuildTime package](https://img.shields.io/nuget/v/Kingdom.Collections.Enumerations.BuildTime.svg?label=Kingdom.Collections.Enumerations.BuildTime%20NuGet%20Package)](https://nuget.org/packages/Kingdom.Collections.Enumerations.BuildTime)

We will append additional Collections shields over time.

# Collections

For lack of a better name, we opted to rename the suite *"Collections"*, which includes `ImmutableBitArray`, and the derivational work, `Enumerations`.

## Breaking Changes

### Bidirectionals Refactored

We refactored the `BidirectionalList` to the `Kingdom.Collections.Generic` namespace instead of `Kingdom.Collections` where it was before.

We also added a `BidirectionalDictionary` in addition to the `BidirectionalList`. This operates along similar lines as the list except that your *Add* and *Remove* callbacks accept both a *key* as well as the *value*.

## ImmutableBitArray

Initially, we wanted to use the .NET Framework [System.Collections.BitArray](http://msdn.microsoft.com/en-us/library/system.collections.bitarray.aspx) for a couple of our applications, but soon discovered that it was neither [immutable](http://en.wikipedia.org/wiki/Immutable_object) nor [idempotent](http://en.wikipedia.org/wiki/Idempotence) under certain circumstances, especially for some key bitwise operations. Effectively, some operations that should return a new instance do not, which is incorrect behavior. We may rename the collection, and consequently the assembly, after all, to better reflect the *Idempotent* attribute that we found was the most critical; but for now, we am running with the name *Immutable*.

The operations are fairly self explanatory. The goals were clear getting started: we wanted to establish a basic moral equivalence, so-called, but for the afore mentioned immutability and idempotency concerns. We will continue adding new operations, and will continue to flesh it out, or as issues and requests are submitted, or contributors want to add to the body of effort.

We took a little time to improve performance by representing the *Immutable Bit Array* in terms of a collection of `Byte`. This took a bit of effort, but we think the performance is about as strong as can be at present. Chiefly, there was also a trade off in terms of *Shift* capability involved in that there is no advantage spending the calories on figuring out the byte-wise shifts involved. Instead, we opted to simply treat the Shift in terms of a `Boolean` collection, which works out pretty well performance-wise.

Eventually, we may reconsider whether the application of the term *idempotent* is really that accurate. Upon further analysis, it seems to me the focus of whether something is idempotent has to do with the function itself not mutating the thing it is operating on, regardless of the outcome. And, while true, the *ones complement* operator, indeed *any* such operators, should leave the original *operand* untouched, this is not really the same thing, we think. We will need to study the issue a bit further to better name it, we think.

## Enumerations

We wanted to support collections of [Java Enum](http://docs.oracle.com/javase/7/docs/api/java/lang/Enum.html) -esque `Enumerations` framework for .NET. Instead of [simple integral values](http://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/enum), we wanted to attach additional domain specific properties to each kind of Enumerated value. This is not supported in .NET, at least not directly, unlike Java, which supports class like behavior directly from enumerated values.

We decided to host our `Enumerations` framework as part of the `Collections` suite because we utilize `ImmutableBitArray` for [`FlagsAttribute`](https://docs.microsoft.com/en-us/dotnet/api/system.flagsattribute) style `Enumerations`.

This version of the framework saw a major recasting of the framework in order to better support not only [*Keyed* `Enumerations`](/mwpowellhtx/Kingdom.Collections/tree/master/src/Kingdom.Collections.Enumerations/Keyed/Enumerations.Keyed.Derived.cs), i.e. for [`Ordinal`](/mwpowellhtx/Kingdom.Collections/tree/master/src/Kingdom.Collections.Enumerations/Keyed/Ordinals/Enumerations.Ordinal.cs) as well as [`Flags` (*Bitwise*)](/mwpowellhtx/Kingdom.Collections/tree/master/src/Kingdom.Collections.Enumerations/Keyed/Flags/Enumerations.Flags.Ctors.cs) style applications, but also [*Unkeyed* `Enumerations`](/mwpowellhtx/Kingdom.Collections/tree/master/src/Kingdom.Collections.Enumerations/Unkeyed/Enumerations.Unkeyed.Derived.cs). This involved fundamental refactoring of the assets involved, and we think we have a much stronger, more concice implementation as a result. This is reflected not only in the framework itself, but the benefits also work out very nicely in terms of the unit testing involved.

### Enumeration Unit Testing

Instead of containing unit testing within the suite as a done deal, we opted to expose a robust set of unit tests for purposes of vetting ***your*** applications of ``Enumeration` or `Enumeration<T>`. This is key, because a lot can be told by the story of your own applications.

We decided to drop [NUnit](http://nunit.org/) support from the project altogether in favor of [xunit](http://xunit.github.io/). We are are also employing our xunit extension methods quite effectively throughout the solution.

### FlagsEnumerationAttribute enabled Code Generation

Our motivation here was to establish an seamless `Enumeration<T>` experience that looks and feels more or less like the language leven `enum` and `FlagsAttribute`. As such, we wanted to enable automatic code generation of the boilerplate code that is necessary to override the bitwise operators with appropriate `Enumeration<T>` based counterparts.

We wanted to pursue this in terms of a *Visual Studio Extension* at first, but soon discovered that the better choice was to do so in terms of a *.NET Standard Analyzer and Code Fix*. As the name implies, this heavily depended upon first making the transition into *.NET Core/Standard*. As it turns out, this is doable, but not so simple on the surface, not least of all with respect to *Core/Standard* confusion throughout the industry today. Less so for me today, but the migration paths even within *Core/Standard* versions are still a bit muddy waters for me.

At the present time, there are a couple of aspects in the delivery. First, there is a *Code Fix* enabled by the *Analyzer* when the `FlagsEnumerationAttribute` is applied. This determines whether the target `Enumeration<T> class` is declared `partial`, and provides a corresponding fix for when it has not.

The second is the code generation itself, around which the integration nuances are not fully resolved. Unit testing of which was also a primary motivation with the subsequent [*Code Analysis*](#net-code-analysis-code-fixes-and-other-fallout) section. Under the hood, code generation depends upon the [`CodeGeneration.Roslyn`](/AArnott/CodeGeneration.Roslyn) project, and ultimately upon command line `code-gen` bits.

At the time of this writing, *CodeGeneration.Roslyn* integration nuances were not fully working and are still to be determine. To be clear, and to be fair, we do not mean that *CodeGeneration.Roslyn* itself is not working; only in terms of our solution level comprehension of said bits. However, we are fairly confident that the `FlagsEnumerationAttribute` generator itself is working, and have commited the unit tests that prove this to be the case.

### .NET Code Analysis, Code Fixes, and other fallout

Our pursuit of the *Analyzer and Code Fix* extensibility solution also led me to discover a couple of areas that deserved serious refactoring, thereby improving upon the boilerplate project template. Chiefly, [*Analyzer Diagnostics*](/mwpowellhtx/Kingdom.Collections/tree/master/src/Kingdom.CodeAnalysis.Verifiers.Diagnostics) and [*Code Fixes*](/mwpowellhtx/Kingdom.Collections/tree/master/src/Kingdom.CodeAnalysis.Verifiers.CodeFixes), their helpers, etc, deserve their own projects with separately delivered packages. This is especially true for code generation unit testing, which depends solely upon the analyzer diagnostics alone, wholely separate from the code fixes themselves.

In addition, there were [also a couple of extension methods](/mwpowellhtx/Kingdom.Collections/tree/master/src/Kingdom.CodeAnalysis.Verification) that We found helpful to more *fluently* verify that the code is properly generated. We intentionally steered clear of assuming any dependencies on [*xunit*](/xunit/xunit), or any other, test framework, at this level. Although, we left things fairly open to inject assertions via extension method *predicates*.

Ultimately, we will likely reposition these in a repository dedicated to this notion, but for the time being the projects live here at the point of discovery, at least until the immediate dust has settled a bit.

## Data Structures

As it turns out, there is not much work that is truly required to support Data Structure Patterns such as [*Stacks*](https://en.wikipedia.org/wiki/Stack_%28abstract_data_type%29), [*Queues*](https://en.wikipedia.org/wiki/Queue_%28abstract_data_type%29), and even one of our favorites, [*Deques*](https://en.wikipedia.org/wiki/Double-ended_queue), or *Double-ended Queues*. Additionally, the unit testing around these follows an extremely cohesive testing paradigm, which makes it that much easier to support.

This being said, we decided to implement our `Deque` solution as a first class interface based approach. This required a bit of recasting at the unit test level in order to better accommodate the implementation. But we think we have a stronger solution for the effort. We may circle around and revisit `Queue` as well as `Stack`, however, we wanted to prioritize `Deque` because this pretty much supports the best of both of these data structures.

## Future Goals

Re-writing any of these assemblies in terms of [*C++ CLI*](https://en.wikipedia.org/wiki/C%2B%2B/CLI) may be a non-starter at least in the near and medium term after all. From reading various blogs, etc, it seems as though it is not on the *Microsoft* agenda to migrate any *C++ CLI* support in terms of *.NET Core* or *.NET Standard* support.

We do still want to consider furnishing first class collection objects, not just syntactic sugar in the form of collection extension methods, but this effort is not high on our list of priorities at the moment. It is a work in progress at the time of this posting.

Thank you so much and enjoy!
