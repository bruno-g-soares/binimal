# Binimal

<p align="center">
  <img src="assets/preview/app.png" width="128" alt="Binimal icon">
</p>

**Binimal** is a tiny, open-source Recycle Bin utility for the Windows notification area. It provides the useful part of the classic MiniBin experience without PowerShell, an installer, telemetry, or administrator privileges.

## Features

- Dynamic tray icon: blue when empty, orange paper when full
- Left-click to open the Recycle Bin
- Right-click to open or empty it
- Uses Windows' standard confirmation before permanently emptying files
- Optional **Start with Windows** toggle
- One portable executable, currently under 100 KB
- One running instance per Windows session
- Checks Recycle Bins across available local drives
- No network access, telemetry, updater, background service, or PowerShell

## Requirements

- Windows 11 22H2 or newer
- No separate runtime installation: Binimal targets .NET Framework 4.8.1, included with current Windows 11 releases

It may also run on Windows 10 systems with .NET Framework 4.8.1 installed, but Windows 11 is the supported target.

## Install

1. Download the latest `Binimal-*-win.zip` from [Releases](https://github.com/bruno-g-soares/binimal/releases).
2. Extract `Binimal.exe` to a permanent folder, such as `%LOCALAPPDATA%\Programs\Binimal`.
3. Run `Binimal.exe`.
4. If desired, right-click its tray icon and enable **Start with Windows**.
5. In Windows 11, use **Settings → Personalization → Taskbar → Other system tray icons** to keep Binimal visible outside the overflow menu.

### Unsigned application notice

Binimal is intentionally unsigned because this community project does not purchase a commercial code-signing certificate. Windows SmartScreen may show **Windows protected your PC** for a newly downloaded release. Select **More info → Run anyway** only after confirming that you downloaded it from this repository.

Every release includes a SHA-256 checksum. The complete build workflow and source are public.

## Usage

| Action | Result |
|---|---|
| Left-click tray icon | Open Recycle Bin |
| Right-click → Open Recycle Bin | Open Recycle Bin |
| Right-click → Empty Recycle Bin | Show Windows confirmation, then empty it |
| Right-click → Start with Windows | Toggle per-user startup |
| Right-click → Exit | Close Binimal |

## Privacy and security

Binimal:

- does not connect to the internet;
- does not collect or transmit data;
- does not request administrator privileges;
- does not run scripts or bypass PowerShell execution policy;
- writes only one optional value under the current user's Windows `Run` registry key when **Start with Windows** is enabled.

Recycle Bin status and emptying use the documented Windows Shell APIs `SHQueryRecycleBinW` and `SHEmptyRecycleBinW`.

## Build from source

Prerequisites:

- .NET 8 SDK (used as the build toolchain)
- Python 3 and Pillow only when regenerating icon files

```powershell
dotnet restore Binimal.sln
dotnet build Binimal.sln --configuration Release
dotnet test Binimal.sln --configuration Release --no-build
```

The portable executable is produced at:

```text
src\Binimal.App\bin\Release\net481\Binimal.exe
```

Regenerate the original icon assets with:

```powershell
python tools\generate_icons.py
```

## Project structure

```text
src/Binimal.App/       Windows tray application and native Shell integration
src/Binimal.Core/      Testable application behavior
tests/                  xUnit tests
assets/source/          Original editable SVG artwork
tools/generate_icons.py Deterministic ICO generator
```

## Contributing

Issues and pull requests are welcome. Keep Binimal focused: small, transparent, dependency-light, and dedicated to basic Recycle Bin tray functionality.

## Licence

Code and original artwork are available under the [MIT Licence](LICENSE).
