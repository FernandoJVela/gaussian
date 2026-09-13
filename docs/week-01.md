# Week 1 — Complex Numbers

## Goal

Implement a generic complex number type using .NET Generic Math.

## Mathematical concepts

- Complex arithmetic
- Complex conjugate
- Magnitude
- Phase
- Polar representation
- Euler's formula
- Roots of unity

## Implementation

`Complex<T>` uses:

```csharp
where T : IFloatingPoint<T>

# Week 1 — Errors Found and Resolved

During the implementation of Gaussian Bench v0, several issues were encountered and resolved, mainly related to **.NET 10, generic math, xUnit v3, FsCheck, and GitHub Actions**.

### 1. Incorrect generic constraints in `Complex<T>`

The initial implementation of `Complex<T>` used generic constraints that did not expose all the mathematical operations required. Operations such as `T.Sqrt` and `T.Atan2` were not available with the initial constraints.

This was solved by using:

```csharp
where T : IFloatingPointIeee754<T>
```

This allowed the implementation to use operations such as `Sqrt`, `Atan2`, `Pi`, and other floating-point mathematical functions in a generic way.

### 2. Conflict between xUnit v2 and xUnit v3

After adding `FsCheck.Xunit.v3`, the test project contained references to both xUnit v2 and xUnit v3. This caused conflicts such as:

```text
FactAttribute exists in both xunit.core and xunit.v3.core
```

The problem was solved by migrating completely to **xUnit v3** and removing the old xUnit v2 dependencies, including `Microsoft.NET.Test.Sdk`, `xunit.runner.visualstudio`, and `coverlet.collector`.

### 3. Microsoft Testing Platform configuration

Initially, `dotnet test` attempted to use the old VSTest mechanism, which was not compatible with the .NET 10 configuration being used.

The solution was to add a `global.json` file and configure:

```json
{
  "sdk": {
    "version": "10.0.112",
    "rollForward": "latestPatch"
  },
  "test": {
    "runner": "Microsoft.Testing.Platform"
  }
}
```

This enabled the new Microsoft Testing Platform test runner.

### 4. xUnit v3 test project had to be executable

After enabling Microsoft Testing Platform, xUnit v3 reported that the test project had to be executable:

```text
xUnit.net v3 test projects must be executable
```

The problem was solved by adding:

```xml
<OutputType>Exe</OutputType>
```

to the test project's `.csproj` file.

### 5. `PositiveInt` could not be found

One of the property-based tests used `PositiveInt`, but the FsCheck namespace was not imported.

The problem was solved by adding:

```csharp
using FsCheck;
```

along with the required xUnit/FsCheck namespaces.

### 6. GitHub SSH authentication error

When attempting to push the repository using:

```bash
git push -u origin master
```

GitHub returned:

```text
Permission denied (publickey)
```

The problem was related to SSH authentication between the local machine and GitHub, rather than the Gaussian Bench code itself.

The solution involved configuring an SSH key and associating the public key with the GitHub account.

### 7. GitHub Actions could not find the required .NET SDK

The GitHub Actions workflow successfully reached `dotnet restore`, but `global.json` required a specific SDK version:

```text
10.0.112
```

while the GitHub Actions runner did not have that exact SDK version installed.

The runner had other .NET 10 SDK versions available, but SDK resolution failed because the version specified in `global.json` could not be found.

The issue highlighted the importance of keeping the local development environment and CI environment aligned and making the SDK version an explicit, reproducible project dependency.

### Week 1 Result

At the end of this stage, Gaussian Bench has:

- .NET 10 and generic math.
- A generic `Complex<T>` implementation.
- Unit tests using xUnit v3.
- Property-based testing using FsCheck.
- Microsoft Testing Platform.
- `global.json` for SDK and test-runner configuration.
- An initial GitHub Actions CI workflow.
- Automated build and test execution.

The local test suite successfully reached:

```text
Passed!
total: 12
failed: 0
succeeded: 12
skipped: 0
```

### Key Lesson

The main lesson from this week was that **making the code compile is only one part of software engineering**.

A reliable project also requires:

- controlled dependencies and SDK versions,
- a properly configured testing framework,
- automated validation,
- reproducible environments,
- and a CI pipeline that verifies the project independently of the developer's machine.

These problems were valuable because they exposed the infrastructure and tooling concerns that exist beyond the application code itself.