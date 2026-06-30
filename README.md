# Coding Tracker

A console app that tracks coding sessions (or any activity) and saves them to a SQLite database.

## Prerequisites

You need the **.NET 10 SDK** installed on your system.

| Platform | Install Guide |
|----------|-------------------------|
| **Windows** | Run `winget install Microsoft.DotNet.SDK.10` in PowerShell, or download from [dotnet.microsoft.com](https://dotnet.microsoft.com/download/dotnet/10.0) |
| **macOS** | Run `brew install --cask dotnet-sdk10` via Homebrew, or download the `.pkg` from [dotnet.microsoft.com](https://dotnet.microsoft.com/download/dotnet/10.0) |
| **Linux (Debian/Ubuntu)** | Run `sudo apt update && sudo apt install dotnet-sdk-10.0` |
| **Linux (Fedora/RHEL)** | Run `sudo dnf install dotnet-sdk-10.0` |
| **Linux (Arch)** | Run `sudo pacman -S dotnet-sdk-10.0` |


Verify installation:

```bash
dotnet --version
```

You should see `10.0.x`.

## How to Use

1. Clone the repository:

   ```bash
   git clone https://github.com/OpaliteArchitect/coding-tracker
   ```

2. Run the app:

   ```bash
   dotnet run --project "coding-tracker/Coding Tracker/Coding Tracker.csproj"
   ```

3. Navigate the UI:
   - Use **up and down arrow keys** to move up and down.
   - Press **Enter** to confirm.
   - Toggle with **Spacebar** to check/uncheck a row.
   - Press **Escape** or **Left Arrow** to cancel or go back.

### Features

- Start and end timed sessions
- View past sessions in a formatted table
- Manually add sessions
- Delete existing sessions
- All data is stored locally in a SQLite database

## License

MIT License. See [LICENSE](LICENSE) for the full text.

Copyright (c) 2025 OpaliteArchitect
```
