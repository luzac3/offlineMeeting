# .NET 9 Upgrade Plan

## Execution Steps

Execute steps below sequentially one by one in the order they are listed.

1. Validate that a .NET 9 SDK required for this upgrade is installed on the machine and if not, help to get it installed.
2. Ensure that the SDK version specified in global.json files is compatible with the .NET 9 upgrade.
3. Upgrade electron\electron.csproj
4. Upgrade offlineMeeting\offlineMeeting.csproj

## Settings

This section contains settings and data used by execution steps.

### Aggregate NuGet packages modifications across all projects

NuGet packages used across all selected projects or their dependencies that need version update in projects that reference them.

| Package Name     | Current Version | New Version | Description          |
|:----------------------------------------|:---------------:|:-----------:|:----------------------------------------------|
| Microsoft.EntityFrameworkCore   | 9.0.7  | 9.0.11    | Recommended for .NET 9     |
| Microsoft.EntityFrameworkCore.SqlServer | 9.0.7         | 9.0.11      | Recommended for .NET 9      |
| System.Text.Json   | 9.0.7           | 9.0.11   | Recommended for .NET 9       |

### Project upgrade details

This section contains details about each project upgrade and modifications that need to be done in the project.

#### electron\electron.csproj modifications

Project properties changes:
  - Target framework should be changed from `net8.0` to `net9.0`

#### offlineMeeting\offlineMeeting.csproj modifications

Project properties changes:
  - Target framework should be changed from `net8.0` to `net9.0`

NuGet packages changes:
  - Microsoft.EntityFrameworkCore should be updated from `9.0.7` to `9.0.11` (*recommended for .NET 9*)
  - Microsoft.EntityFrameworkCore.SqlServer should be updated from `9.0.7` to `9.0.11` (*recommended for .NET 9*)
  - System.Text.Json should be updated from `9.0.7` to `9.0.11` (*recommended for .NET 9*)
