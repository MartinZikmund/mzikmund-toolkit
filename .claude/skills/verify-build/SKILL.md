---
name: verify-build
description: Build the consolidated solution (library + samples + tests) and run the test runner. Use after non-trivial edits, before opening a PR, or when asked to "verify" or "check the build".
---

# verify-build

The repo uses a single root `MZikmund.Toolkit.slnx` covering the library, the sample app, and the test project. This skill builds it end-to-end and runs the tests.

## Steps

1. Restore workloads (skip if already done in this session):

   ```powershell
   dotnet workload restore MZikmund.Toolkit.slnx
   ```

2. Build the solution:

   ```powershell
   dotnet build MZikmund.Toolkit.slnx -c Debug
   ```

3. Run the tests via the Microsoft Testing Platform exe (`dotnet test` is not supported on .NET 10 SDK against MTP without the new opt-in):

   ```powershell
   tests/MZikmund.Toolkit.WinUI.Tests/bin/Debug/net10.0/MZikmund.Toolkit.WinUI.Tests.exe
   ```

4. Report: pass/fail with any errors or test failures quoted verbatim.

## Notes

- These are full multi-targeted builds (Android/iOS/macCatalyst/Windows/WASM/Desktop). Expect them to take a few minutes the first time.
- If a workload error appears (e.g. "workload `android` is not installed"), run the matching `dotnet workload install <id>` and retry — don't silently skip the platform.
