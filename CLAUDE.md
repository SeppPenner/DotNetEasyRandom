# Project rules for Claude

## What this is

DotNetEasyRandom is a small class library that wraps `System.Random` so that the borders of an
integer range can be included or excluded explicitly, and so that a `double` from an open interval
is one call away. It is published as the NuGet package
[HaemmerElectronics.SeppPenner.DotNetEasyRandom](https://www.nuget.org/packages/HaemmerElectronics.SeppPenner.DotNetEasyRandom/),
`GeneratePackageOnBuild` is on, so every build writes the package.

One solution `src/DotNetEasyRandom.sln` with exactly two projects:

- `src/DotNetEasyRandom/DotNetEasyRandom.csproj`, the library that becomes the package.
- `src/DotNetEasyRandom.Tests/DotNetEasyRandom.Tests.csproj`, MSTest, added in version 1.0.14.0.

There is no example project, the README carries the usage sample.

Layout inside `src/DotNetEasyRandom`:

- `EasyRandom.cs` plus `IEasyRandom.cs`: the whole public surface. `Next()`, `Next(int)`,
  `NextBytes(byte[])` and `NextDouble()` only forward to the wrapped `Random`.
  `Next(int, int, IncludeType)` and `NextDoubleExcludedBoth(double, double)` are the two methods
  that add something, both of them do their work in a private helper (`NextWithoutBothBorders`,
  `NextDoubleWithoutZero`). Every input check is a `private static` method of its own
  (`CheckIntegers`, `CheckDoubles`, `CheckIfEqual`, `CheckIfBigger`, `CheckIfMaximum`,
  `CheckIfMinimum`, `CheckIfNeighbours`), each overloaded for `int` and `double` where both make
  sense. Keep new checks in that shape.
- `IncludeType.cs`: the enumeration `None`, `StartOnly`, `EndOnly`, `Both`, without explicit
  values.
- `Exceptions/`: `EqualToIntMaxValueException`, `MinMustBeGreaterThanMaxException`,
  `NoValueBetweenParametersException` and `ParametersMustNotBeEqualException`. All four are
  `[Serializable]`, derive from `Exception` and have the three usual constructors. They are part of
  the public API of the package.
- `GlobalUsings.cs`: all usings of the project, currently only `DotNetEasyRandom.Exceptions`.

Layout inside `src/DotNetEasyRandom.Tests`:

- `EasyRandomTests.cs`: the only test class, 21 tests. One test per include type that collects the
  distinct results of 1000 draws, one test per rejected input, and the tests for the three methods
  that only forward to the wrapped `Random`. The tests that cover a fixed defect name the version
  the defect was in.
- `GlobalUsings.cs`: all usings of the test project. `DotNetEasyRandom` itself is not among them,
  the namespace of the test project is nested inside it and an explicit using would be reported as
  unnecessary by `IDE0005`.
- The class under test is always built with the seed 12, the seed the README uses. That way a test
  drawing 1000 values draws the same 1000 values on every machine and can never fail on one only.

Repository root: `README.md` (the only user documentation, with the usage sample), `Changelog.md`,
`License.txt` (MIT), `Icon.png` (shipped inside the package), `BuildAndPushPackage.bat`,
`Delete-BIN-OBJ-Folders.bat`, `.gitattributes` and `.gitignore`. The `.editorconfig` sits one level
down in `src`. There is no `Updating.md`, no `HowToUse.md`, no screenshots and no `.github` folder.

## Build

```powershell
dotnet build src/DotNetEasyRandom.sln
```

```powershell
dotnet test src/DotNetEasyRandom.sln
```

- Multi-targeting `net10.0;net11.0`, no `RuntimeIdentifiers`, so building needs the .NET 11 SDK.
  Nothing in the code is platform specific. `net9.0` was dropped in version 1.0.14.0 because it
  is out of support since May 2026, the same step the sibling repository `CSharpLanguageManager`
  took in its version 1.1.9.0. `net8.0` was dropped after 1.0.14.0.
- All build properties live directly in `DotNetEasyRandom.csproj`. There is **no**
  `Directory.Build.props` in this repository, unlike the sibling repository
  `CSharpLanguageManager`.
- `GeneratePackageOnBuild` is `true`, so a `Release` build drops
  `HaemmerElectronics.SeppPenner.DotNetEasyRandom.<version>.nupkg` and the matching `.snupkg` into
  `src/DotNetEasyRandom/bin/Release`. That is the folder `BuildAndPushPackage.bat` pushes from.
- `GenerateDocumentationFile` is `true` since version 1.0.14.0, so `DotNetEasyRandom.xml` sits next
  to the assembly in `lib/net10.0` and `lib/net11.0` of the package and consumers see the XML
  comments in IntelliSense. Together with `TreatWarningsAsErrors` this means a public member
  without a doc comment breaks the build (`CS1591`).
- `TreatWarningsAsErrors` is enabled, so every warning breaks the build, NuGet warnings (`NU****`)
  from restore included. A clean build reports zero warnings, keep it that way.
- `NU1803` (HTTP source usage during restore) is the one warning suppressed via `NoWarn`. Fix
  warnings instead of extending that list. `NuGetAudit` and `NuGetAuditMode=all` are on, so a
  vulnerable transitive package fails the build too.
- Versions come from GitVersion.MsBuild out of the git tags, for example `1.0.14-3` for the third
  commit after tag `1.0.13`. Never edit a version property or an assembly version by hand.
- Restore needs nuget.org. If a private feed is configured globally on the machine and answers 404
  for public packages, restore fails with `NU1301`. Then build with an explicit source:
  `dotnet build src/DotNetEasyRandom.sln --source https://api.nuget.org/v3/index.json`.
- Tests are MSTest, in the single test project `src/DotNetEasyRandom.Tests`, which follows the same
  package set as the sibling repositories: `Microsoft.NET.Test.Sdk`, `MSTest.TestAdapter`,
  `MSTest.TestFramework`, `coverlet.collector` and `GitVersion.MsBuild`. The project targets
  `net11.0` only, the newest framework the library targets. `IsPackable` is `false`, so it
  produces no package of its own and `BuildAndPushPackage.bat` has nothing extra to push.
  `dotnet test` runs 21 tests, they need no network and no file on disk. Never claim a test run
  happened without running it.

## Code conventions

Follow the surrounding code, it is consistent throughout every file:

- File header comment block with `<copyright file="..." company="Hämmer Electronics">` and a
  `<summary>`, then the file-scoped namespace.
- XML doc comments on every type and every member, private members included, no exceptions.
  Implementations of an interface member additionally carry `<inheritdoc cref="..."/>` and
  `<seealso cref="..."/>` pointing at that interface, and they repeat the `<summary>` instead of
  relying on the inherited one.
- `Nullable`, `ImplicitUsings` and `LangVersion latest` are enabled.
- New `using` directives go into `GlobalUsings.cs`, inside the existing
  `#pragma warning disable IDE0065` block, never at the top of a file. The editorconfig requires
  usings inside the namespace (`csharp_using_directive_placement=inside_namespace:warning`), which
  global usings cannot satisfy, that is what the pragma is for. Do not add other pragmas. The
  comment text in that block is German because Visual Studio generated it, leave it alone.
- Fields, properties, methods and events are always accessed with `this.` qualification
  (`dotnet_style_qualification_for_*` at severity `warning`), which is why the `private static`
  check methods are called without a prefix and the instance member `this.random` always with one.
- `src/.editorconfig` also enforces braces everywhere, no multiple blank lines, four spaces, CRLF,
  UTF-8 without BOM, file scoped namespaces, `System` usings sorted first and `IDE0005` as warning.
  Analyzer warnings are fixed, not silenced.

## Known quirks

Do not silently "clean up" these, they are existing behaviour of a published package:

- **`MinMustBeGreaterThanMaxException` says the opposite of its reason.** It is thrown when
  `minValue > maxValue`, so the name describes the broken state instead of the requirement. The
  type is public API since 2016, renaming it would be a breaking change, so it stays.
- **`EqualToIntMaxValueException` is used for the `double` checks too.** `CheckIfMaximum(double)`
  and `CheckIfMinimum(double)` throw it for `double.MaxValue` and `double.MinValue`, and even the
  minimum check throws the "max value" type. Only the message says which border was hit.
- **`int.MaxValue` is rejected, `int.MinValue` is not.** `CheckIntegers` runs `CheckIfMaximum` on
  both parameters, so `Next(1, int.MaxValue, IncludeType.Both)` throws instead of returning a
  value. The reason is the `maxValue + 1` inside `Next`. Nothing rejects `int.MinValue`, it works.
- **The equality epsilon is a literal, not `double.Epsilon`.** `CheckIfEqual(double, double)` and
  the border checks compare against `0.000000000000000000000000000000000000000000001`. Two values
  that differ by less than that count as equal.
- **A range of `double.MinValue + 1` to `double.MaxValue - 1` returns infinity.** Both border
  checks pass, then `maxValue - minValue` overflows inside `NextDoubleExcludedBoth`. Unfixed,
  callers do not pass such ranges.
- **`IncludeType.None` throws for neighbouring parameters.** `Next(1, 2, IncludeType.None)` has no
  result that could be returned, both candidates are excluded borders. Since version 1.0.14.0 that
  is a `NoValueBetweenParametersException` instead of the wrong value 2 that came back before.
- **An undefined `IncludeType` throws an `ArgumentOutOfRangeException`.** That is the only exception
  of this library that is not one of the four own types, and it is deliberate: an unknown enum value
  is a programming error of the caller, not a rejected input value.
- **`NextDoubleExcludedBoth` resamples the zero.** `Random.NextDouble()` returns a value from
  `[0, 1)`, and a zero would return `minValue`, which the method promises to exclude. The private
  `NextDoubleWithoutZero` therefore draws again on a zero. What remains is a range so narrow that
  `value * (maxValue - minValue)` rounds to zero, then `minValue` still comes back. Do not turn this
  into a loop over the result, for neighbouring `double` values no valid result exists and the loop
  would never end.
- **The exceptions have no inner exception.** `InnerException` is `null` everywhere since version
  1.0.14.0, before that every throw site passed a second `Exception` with the very same message. The
  constructors taking an inner exception stay, they are public API.
- **The README sample uses `_random` fields.** That contradicts the `this.` qualification of the
  library code. It is a sample for consumers, leave it alone.
- **AppVeyor badge without CI in the repository.** `README.md` links an AppVeyor build that is
  configured outside of this repository. There is no `.github` folder and no pipeline file here.
- **`src/DotNetEasyRandom.sln.DotSettings`** is tracked and holds nothing but a ReSharper user
  dictionary with the single entry `H_00E4mmer`. Leave it alone.
- **`.gitattributes` sets `* text=auto`**, every other rule of the Visual Studio template below it
  is commented out. `core.autocrlf` is `true` on this machine, so the working tree has CRLF and the
  repository has LF. Write new files with CRLF and UTF-8 without BOM.
- **`Delete-BIN-OBJ-Folders.bat`** deletes every `bin` and `obj` below its own folder and is the
  first thing `BuildAndPushPackage.bat` does again inline. Both files skip `node_modules`, which
  this repository does not have.

## Releasing

**Every new version is released completely.** Changelog, README, nuget.org, GitHub Packages and a
GitHub release belong to every version, none of these steps is optional and none of them waits for
a separate request.

1. Make the change.
2. Add an entry at the top of `Changelog.md` in the existing format:
   `* **Version 1.0.14.0 (2026-08-11)** : Short description.`
3. Copy that same text into `<PackageReleaseNotes>` in `DotNetEasyRandom.csproj`, in the format
   `Version 1.0.14.0 (2026-08-11): Short description.` without the leading bullet. The two must
   say the same thing, the property is the release note shown on nuget.org.
4. Update `README.md` for every version. The `## Available for` list has to match the target
   frameworks, and nothing there may describe a state the package has left behind.
5. Commit that.
6. Tag the commit with the plain version number, no `v` prefix (`1.0.13`, `1.0.12`, ...). The
   existing tags are lightweight tags, create new ones the same way.
7. Push the commits and the tag.
8. Only then build the package, and build it while the tag is the newest commit. GitVersion takes
   the version from the tag plus the distance to it, so a build before the tag or after another
   commit produces a prerelease version like `1.0.15-3+Branch.master.Sha...`, and that version
   would end up on nuget.org. `git describe --tags` has to answer with the plain tag.
9. Publish to nuget.org with `BuildAndPushPackage.bat`, started from the repository root, its first
   line is `cd .\src`. It deletes all `bin` and `obj` folders, restores, builds `Release` and
   pushes the `.nupkg` and the `.snupkg` to nuget.org with `%NUGET_API_KEY%` and to the NuGet
   source named `github` with `%GITHUB_API_KEY%`. It ends with a `PAUSE`, so it wants a console.
   Call it with its full path: the Claude Code shell sets `NoDefaultCurrentDirectoryInExePath=1`,
   so cmd does not find a bare `BuildAndPushPackage.bat` in the current directory.
   **Every line of that script below `@ECHO on` needs its leading `@`.** `ECHO` is on there, so a
   line without it is written to the output before it runs, and for the four push lines that means
   the expanded API keys land in the console and in any log that catches it. The script says
   `Upload success.` no matter what the pushes answered, so judge a push by the lines above it.
   Because it always rebuilds, it is only correct as long as no commit sits on top of the tag. If
   one does, push the package that was built from the tagged state instead, by its file name and
   not with the wildcard, because `GeneratePackageOnBuild` leaves a `.nupkg` of every intermediate
   build in `bin`:
   `dotnet nuget push HaemmerElectronics.SeppPenner.DotNetEasyRandom.1.0.14.nupkg -s "nuget.org" --skip-duplicate -k %NUGET_API_KEY%`
   in `src/DotNetEasyRandom/bin/Release`. That command takes the matching `.snupkg` with it. A
   push to nuget.org cannot be taken back, the version is burned even after unlisting. nuget.org
   needs a few minutes to validate a new version before it is listed.
10. Publish the same `.nupkg` to GitHub Packages,
    `https://nuget.pkg.github.com/SeppPenner/index.json`. The script only reaches it if a NuGet
    source named `github` is registered on the machine, `dotnet nuget list source` shows that. On
    2026-11-24 only nuget.org was registered here, so the two `github` lines of the script fail
    and the push has to be done by hand in `src/DotNetEasyRandom/bin/Release`:
    `dotnet nuget push HaemmerElectronics.SeppPenner.DotNetEasyRandom.1.0.14.nupkg -s https://nuget.pkg.github.com/SeppPenner/index.json --skip-duplicate --api-key <token>`.
    The API key is a GitHub token with the scope `write:packages`, for example from
    `gh auth token`. If `gh auth status` lists no `write:packages`, the user has to run
    `gh auth refresh -h github.com -s write:packages,read:packages`, it needs a browser login.
    GitHub Packages takes no symbol packages, push the `.nupkg` only.
11. Create the GitHub release:
    `gh release create 1.0.14 -R SeppPenner/DotNetEasyRandom --verify-tag --latest --title "Version 1.0.14.0 (2026-08-11)" --notes-file <file>`.
    The notes are the changelog sentence plus the line
    `NuGet: https://www.nuget.org/packages/HaemmerElectronics.SeppPenner.DotNetEasyRandom/1.0.14`.
    Write credits as `` `@name` `` in backticks, a bare `@name` notifies that person again.

The version in `Changelog.md` has four parts (`1.0.14.0`), the tag has three (`1.0.14`).
GitVersion turns the tag into the assembly version, so an untagged commit produces something like
`1.0.14-3+Branch.master.Sha...`.

nuget.org and GitHub Packages hold every version from 1.0.2 to 1.0.15. The tag 1.0.1 of 2019 was
never published and stays that way. Every tag has a GitHub release, the one of 1.0.1 has no NuGet
link. The missing versions on GitHub Packages and the releases were added on 2026-11-24.

## Git

- Include the full GitHub pull request or issue URL in the body of each related commit on a `Refs:`
  line. Use the [GitHub issue skill](.claude/skills/github-issue/SKILL.md) for issue work and closeout.
- **Never amend a commit.** No `git commit --amend`, not for a typo in the message, not to add a
  forgotten file, not even when the commit is still local. Write a follow-up commit instead. The
  release versions come from tags on exact commits, an amended commit leaves its tag pointing at a
  commit that no longer exists in the branch.

## Writing style

- Commit messages are written **in English only**: short, precise subject line, explanatory body
  when needed.
- Code comments and comments in project files such as `.csproj` are **always English**, regardless
  of the language used in the conversation.
- **No em dashes or en dashes** (`—`, `–`), neither in prose, commit messages, code comments nor
  documentation. Use a regular hyphen, comma, colon, parentheses or a separate sentence.
- German texts (documentation, chat replies) always use real umlauts and ß, never ASCII
  transliterations such as `ae`, `oe`, `ue` or `ss`. Identifiers, file names and configuration keys
  stay unchanged where umlauts are technically undesirable.
