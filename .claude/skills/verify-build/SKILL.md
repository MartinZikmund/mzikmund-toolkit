---
name: verify-build
description: Build both the library solution (src) and the sample solution (samples) to confirm changes compile against both Uno.Sdk versions. Use after non-trivial edits, before opening a PR, or when asked to "verify" or "check the build".
---

# verify-build

This repo has two solutions pinned to different Uno.Sdk versions. A change can compile against one and break the other, so verification must build both.

## Steps

1. Restore workloads for each solution (skip if already done in this session):

   ```powershell
   dotnet workload restore src/MZikmund.Toolkit.sln
   dotnet workload restore samples/MZikmund.Toolkit.Sample/MZikmund.Toolkit.Sample.sln
   ```

2. Build the library:

   ```powershell
   dotnet build src/MZikmund.Toolkit.sln -c Debug
   ```

3. Build the sample app:

   ```powershell
   dotnet build samples/MZikmund.Toolkit.Sample/MZikmund.Toolkit.Sample.sln -c Debug
   ```

4. Report: pass/fail for each solution, with any errors quoted verbatim. If only one side fails, call out that this is the kind of bug the two-solution split is meant to surface — the fix usually belongs in whichever solution broke, not both.

## Notes

- These are full multi-targeted builds (Android/iOS/macCatalyst/Windows/WASM/Desktop). Expect them to take a few minutes the first time.
- If a workload error appears (e.g. "workload `android` is not installed"), run the matching `dotnet workload install <id>` and retry — don't silently skip the platform.
- Don't run `dotnet test` — there's no test project yet.
