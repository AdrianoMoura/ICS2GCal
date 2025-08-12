; ICS2GCal - Inno Setup (EN)

[Setup]
AppId=F3C3C5C7-1CDE-4F12-9A8C-7B9B0E6E5A11
AppName=ICS2GCal
AppVersion=1.0.1
AppPublisher=AdrianoMoura
AppPublisherURL=https://github.com/AdrianoMoura/ICS2GCal
AppSupportURL=https://github.com/AdrianoMoura/ICS2GCal/issues
VersionInfoCompany=AdrianoMoura
VersionInfoDescription=Open ICS files in Google Calendar
VersionInfoProductName=ICS2GCal
VersionInfoVersion=1.0.1.0
DefaultDirName={localappdata}\ICS2GCal
DefaultGroupName=ICS2GCal
OutputDir=.
OutputBaseFilename=ICS2GCal-Setup
Compression=zip
SolidCompression=no
PrivilegesRequired=lowest
DisableDirPage=yes
DisableProgramGroupPage=yes
SetupIconFile=assets\icon.ico
UninstallDisplayIcon={app}\ICS2GCal.exe

[Files]
; Main executable and icon shipped with the app
Source: "bin\Release\net8.0\win-x64\publish\ICS2GCal.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "assets\icon.ico"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
; Start Menu shortcut (uses the same icon)
Name: "{group}\ICS2GCal"; Filename: "{app}\ICS2GCal.exe"; IconFilename: "{app}\icon.ico"

[Registry]
; ---- ProgID (file handler) ----
Root: HKCU; Subkey: "Software\Classes\ICS2GCal.ics"; ValueType: string; ValueData: "ICS2GCal ICS Handler"; Flags: uninsdeletekey
Root: HKCU; Subkey: "Software\Classes\ICS2GCal.ics\DefaultIcon"; ValueType: string; ValueData: "{app}\icon.ico"
Root: HKCU; Subkey: "Software\Classes\ICS2GCal.ics\shell\open\command"; ValueType: string; ValueData: """{app}\ICS2GCal.exe"" ""%1"""

; ---- Applications\ICS2GCal.exe (for 'Open with') ----
Root: HKCU; Subkey: "Software\Classes\Applications\ICS2GCal.exe"; ValueType: string; ValueName: "FriendlyAppName"; ValueData: "ICS2GCal"; Flags: uninsdeletekey
Root: HKCU; Subkey: "Software\Classes\Applications\ICS2GCal.exe\DefaultIcon"; ValueType: string; ValueData: "{app}\icon.ico"
Root: HKCU; Subkey: "Software\Classes\Applications\ICS2GCal.exe\shell\open\command"; ValueType: string; ValueData: """{app}\ICS2GCal.exe"" ""%1"""
; Explicitly declare support for .ics
Root: HKCU; Subkey: "Software\Classes\Applications\ICS2GCal.exe\SupportedTypes"; ValueType: string; ValueName: ".ics"; ValueData: ""

; ---- Suggest in 'Open with' (class + Explorer preferences) ----
; Class-level suggestion
Root: HKCU; Subkey: "Software\Classes\.ics\OpenWithProgids"; ValueType: binary; ValueName: "ICS2GCal.ics"; ValueData: 00
; Explorer user preference list
Root: HKCU; Subkey: "Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts\.ics\OpenWithProgids"; ValueType: binary; ValueName: "ICS2GCal.ics"; ValueData: 00
; Hygiene: remove any blocks if present
Root: HKCU; Subkey: "Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts\.ics"; ValueType: none; ValueName: "NoOpenWith"; Flags: deletevalue
Root: HKCU; Subkey: "Software\Classes\Applications\ICS2GCal.exe"; ValueType: none; ValueName: "NoOpenWith"; Flags: deletevalue

; ---- Capabilities → appear in 'Default apps' ----
Root: HKCU; Subkey: "Software\ICS2GCal\Capabilities"; ValueType: string; ValueName: "ApplicationName"; ValueData: "ICS2GCal"
Root: HKCU; Subkey: "Software\ICS2GCal\Capabilities"; ValueType: string; ValueName: "ApplicationDescription"; ValueData: "Handles .ics files and sends them to Google Calendar"
Root: HKCU; Subkey: "Software\ICS2GCal\Capabilities\FileAssociations"; ValueType: string; ValueName: ".ics"; ValueData: "ICS2GCal.ics"
Root: HKCU; Subkey: "Software\RegisteredApplications"; ValueType: string; ValueName: "ICS2GCal"; ValueData: "Software\ICS2GCal\Capabilities"; Flags: uninsdeletevalue

[UninstallDelete]
; Clean app folder on uninstall
Type: filesandordirs; Name: "{app}"
