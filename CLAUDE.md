# Project rules for Claude

## What this is

DotNetEasyRandom is a small class library that wraps `System.Random` so that the borders of an
integer range can be included or excluded explicitly, and so that a `double` from an open interval
is one call away. It is published as the NuGet package
[HaemmerElectronics.SeppPenner.DotNetEasyRandom](https://www.nuget.org/packages/HaemmerElectronics.SeppPenner.DotNetEasyRandom/),
`GeneratePackageOnBuild` is on, so every build writes the package.

One solution `src/DotNetEasyRandom.sln` with exactly one project,
`src/DotNetEasyRandom/DotNetEasyRandom.csproj`. There is no example project and no test project.

Layout inside `src/DotNetEasyRandom`:

- `EasyRandom.cs` plus `IEasyRandom.cs`: the whole public surface. `Next()`, `Next(int)`,
  `NextBytes(byte[])` and `NextDouble()` only forward to the wrapped `Random`.
  `Next(int, int, IncludeType)` and `NextDoubleExcludedBoth(double, double)` are the two methods
  that add something. Every input check is a `private static` method of its own
  (`CheckIntegers`, `CheckDoubles`, `CheckIfEqual`, `CheckIfBigger`, `CheckIfMaximum`,
  `CheckIfMinimum`), each overloaded for `int` and `double`. Keep new checks in that shape.
- `IncludeType.cs`: the enumeration `None`, `StartOnly`, `EndOnly`, `Both`, without explicit
  values.
- `Exceptions/`: `EqualToIntMaxValueException`, `MinMustBeGreaterThanMaxException` and
  `ParametersMustNotBeEqualException`. All three are `[Serializable]`, derive from `Exception` and
  have the three usual constructors. They are part of the public API of the package.
- `GlobalUsings.cs`: all usings of the project, currently only `DotNetEasyRandom.Exceptions`.

Repository root: `README.md` (the only user documentation, with the usage sample), `Changelog.md`,
`License.txt` (MIT), `Icon.png` (shipped inside the package), `BuildAndPushPackage.bat`,
`Delete-BIN-OBJ-Folders.bat`, `.gitattributes` and `.gitignore`. The `.editorconfig` sits one level
down in `src`. There is no `Updating.md`, no `HowToUse.md`, no screenshots and no `.github` folder.

## Build

```powershell
dotnet build src/DotNetEasyRandom.sln
```

- Multi-targeting `net8.0;net10.0`, no `RuntimeIdentifiers`. Nothing in the code is platform
  specific. `net9.0` was dropped in version 1.0.14.0 because it is out of support since May 2026,
  the same step the sibling repository `CSharpLanguageManager` took in its version 1.1.9.0.
- All build properties live directly in `DotNetEasyRandom.csproj`. There is **no**
  `Directory.Build.props` in this repository, unlike the sibling repository
  `CSharpLanguageManager`.
- `GeneratePackageOnBuild` is `true`, so a `Release` build drops
  `HaemmerElectronics.SeppPenner.DotNetEasyRandom.<version>.nupkg` and the matching `.snupkg` into
  `src/DotNetEasyRandom/bin/Release`. That is the folder `BuildAndPushPackage.bat` pushes from.
- `GenerateDocumentationFile` is `true` since version 1.0.14.0, so `DotNetEasyRandom.xml` sits next
  to the assembly in `lib/net8.0` and `lib/net10.0` of the package and consumers see the XML
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
- There are no tests. A behaviour change is verified by a throwaway project that references the
  built library, not by `dotnet test`.

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

## Known defects

These are wrong, not quirky. They are open, so do not build on the behaviour described here.

- **`Next(1, 2, IncludeType.None)` returns 2.** `None` maps to
  `this.random.Next(minValue + 1, maxValue)`, which is `Next(2, 2)` here, and `Random.Next(x, x)`
  returns `x`. So the excluded upper border comes back whenever the two parameters are neighbours,
  and that is the only case in which no valid value exists at all.
- **The `switch` in `Next(int, int, IncludeType)` has no `default` arm.** `returnValue` is
  initialized to `0`, so an undefined enum value such as `(IncludeType)42` silently returns 0
  instead of being rejected.
- **`NextDoubleExcludedBoth` includes the lower border.** `Random.NextDouble()` returns a value
  from `[0, 1)`, so the result is from `[minValue, maxValue)`. `maxValue` is really excluded,
  `minValue` is not, although the method name and the README promise both.
- **The message of `MinMustBeGreaterThanMaxException` states the reason for the throw as the
  requirement.** `Next(5, 1, IncludeType.Both)` says "The parameter 5 must be greater than the
  parameter 1", which is exactly what is the case. It has to say "must be less than".
- **The exceptions carry a copy of their own message as inner exception.** Every throw site builds
  `new XException(message, new Exception(message))`, so the same text shows up twice and the inner
  exception names no cause. The sibling repository `CSharpLanguageManager` dropped the same pattern
  in its version 1.1.8.0.

## Releasing

1. Make the change.
2. Add an entry at the top of `Changelog.md` in the existing format:
   `* **Version 1.0.14.0 (2026-08-11)** : Short description.`
3. Copy that same text into `<PackageReleaseNotes>` in `DotNetEasyRandom.csproj`, in the format
   `Version 1.0.14.0 (2026-08-11): Short description.` without the leading bullet. The two must
   say the same thing, the property is the release note shown on nuget.org.
4. Commit that.
5. Tag the commit with the plain version number, no `v` prefix (`1.0.13`, `1.0.12`, ...). The
   existing tags are lightweight tags, create new ones the same way.
6. **Then** build and push the package, in that order: GitVersion reads the tag, so a package built
   before the tag exists gets a prerelease version such as `1.0.14-3` burned in.
   `BuildAndPushPackage.bat` deletes all `bin` and `obj` folders, restores, builds `Release` and
   pushes the `.nupkg` and `.snupkg` to nuget.org and to the GitHub package feed. It needs
   `NUGET_API_KEY` and `GITHUB_API_KEY` in the environment and ends with a `PAUSE`.
7. Push the commits and the tag.

The version in `Changelog.md` has four parts (`1.0.14.0`), the tag has three (`1.0.14`).
GitVersion turns the tag into the assembly version, so an untagged commit produces something like
`1.0.14-3+Branch.master.Sha...`.

## Git

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
