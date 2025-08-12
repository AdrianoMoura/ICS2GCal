![ICS2GCal](https://github.com/AdrianoMoura/ICS2GCal/blob/master/assets/icon.png?raw=true)
# ICS2GCal

Open `.ics` files in **Google Calendar** on Windows.  
Double‑click an `.ics` → ICS2GCal reads it and opens Google Calendar’s “Create event” page prefilled in your default browser.

---

## Download

**Get the latest version here:**  
- 👉 **Installer (recommended):**  
  [https://github.com/AdrianoMoura/ICS2GCal/releases/latest/download/ICS2GCal-Setup.exe](https://github.com/AdrianoMoura/ICS2GCal/releases/latest/download/ICS2GCal-Setup.exe)
- 👉 **Portable (no install):**  
  [https://github.com/AdrianoMoura/ICS2GCal/releases/latest/download/ICS2GCal-Portable.exe](https://github.com/AdrianoMoura/ICS2GCal/releases/latest/download/ICS2GCal-Portable.exe)


### Install (recommended)
1. Run `ICS2GCal-Setup.exe`.

### Use
- Right‑click → **Open with** → **ICS2GCal** (check **Always**).

That’s it — your browser opens Google Calendar with the event filled in.

### Portable version

- Save the Portable version in a know directory
- Right‑click yout ICS file → **Open with** → **Search for the downloaded executable**
---

## Technical details (for developers)

### How it works
- Parses the first `VEVENT` from the `.ics` using **Ical.Net**
- Supports **all‑day** and **timed** events (`DTSTART`, `DTEND`, `DURATION` fallback)
- Builds a URL like:  
  `https://calendar.google.com/calendar/render?action=TEMPLATE&text=...&dates=...&location=...&details=...`
- Opens the URL via the default browser

### Requirements
- **.NET 8 SDK** (for building)
- **Windows 11/10** (for running)
- NuGet: [`Ical.Net`](https://www.nuget.org/packages/Ical.Net)

### Build (single‑file, self‑contained)
```powershell
dotnet publish -c Release
```
Output:
```
.\bin\Release\net8.0\win-x64\publish\ICS2GCal.exe
```

### Installer (Inno Setup)
The script (`installer/ICS2GCal.iss`) registers:
- **Open with** (with icon)
- **Default apps (Capabilities)** entry

Build the installer:
```powershell
& "C:\Program Files (x86)\Inno Setup 6\ISCC.exe" .\installer\ICS2GCal.iss
```

### Roadmap
- Picker for multiple `VEVENT`s in a single `.ics`  
- Modern Win11 context menu via `IExplorerCommand` shell extension

### License
MIT

### Acknowledgements
- [Ical.Net](https://github.com/rianjs/ical.net) — iCalendar parsing
