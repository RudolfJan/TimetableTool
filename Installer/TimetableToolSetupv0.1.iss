; Script generated by the Inno Script Studio Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "TimetableTool"
#define MyAppVersion "0.1 alpha"
#define MyAppPublisher "Holland Hiking"
#define MyAppURL "http://www.hollandhiking.nl/trainsimulator"
#define MyAppExeName "TimetableTool.exe"
; #define DataDirName= "{code:GetDataDir}"
; #define DefaultDirName="{code:GetInstallDir}"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{0C16F964-B19F-4E40-AFF0-F5AFB66EED9E}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
LicenseFile=License.txt
InfoBeforeFile=Readme.txt
OutputDir= Output
OutputBaseFilename=TimetableToolSetup
Compression=lzma
SolidCompression=yes
WizardImageFile=Setup.bmp
WizardImageBackColor=clInfoBk
WizardImageStretch=False
AppCopyright=2020 Rudolf Heijink
DisableWelcomePage=no
DisableDirPage=no

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 0,6.1

[Files]
; NOTE: Don't use "Flags: ignoreversion" on any shared system files
Source: "..\TimetableTool.Desktop\bin\Release\netcoreapp3.1\TimetableTool.exe"; DestDir: "{app}"; DestName: "{#MyAppExeName}"
Source: "..\TimetableTool.Desktop\bin\Release\netcoreapp3.1\appsettings.json"; DestDir: "{app}"
Source: "..\TimetableTool.Desktop\bin\Release\netcoreapp3.1\TimetableTool.runtimeconfig.json"; DestDir: "{app}"
Source: "..\TimetableTool.Desktop\bin\Release\netcoreapp3.1\TimetableTool.runtimeconfig.dev.json"; DestDir: "{app}"
Source: "..\TimetableTool.Desktop\bin\Release\netcoreapp3.1\TimetableTool.deps.json"; DestDir: "{app}"
Source: "..\TimetableTool.Desktop\bin\Release\netcoreapp3.1\*.dll"; DestDir: "{app}"
Source: "..\Manual\TimetableTool Manual.pdf"; DestDir: "{userdocs}\TimetableTool"
Source: "..\TimetableTool.Desktop\appsettings.json"; DestDir: "{app}"
Source: "..\TimetableTool.Desktop\bin\Release\netcoreapp3.1\SQL\*.sql"; DestDir: "{app}\SQL"
Source: "TimetableDb.db"; DestDir: "{userdocs}\TimetableTool"
Source: "*.txt"; DestDir: "{userdocs}\TimetableTool"
Source: "..\TimetableTool.Desktop\bin\Release\netcoreapp3.1\runtimes\win-x86\native\netstandard2.0\SQLite.Interop.dll"; DestDir: "{app}\runtimes\win-x86\native\netstandard2.0"
Source: "..\TimetableTool.Desktop\bin\Release\netcoreapp3.1\runtimes\win-x86\native\sni.dll"; DestDir: "{app}\runtimes\win-x86\native"
;Source: "..\TimetableTool.Desktop\bin\Release\netcoreapp3.1\cs-CZ\Xceed.Wpf.AvalonDock.resources.dll"; DestDir: "{app}\cs-CZ"
;Source: "..\TimetableTool.Desktop\bin\Release\netcoreapp3.1\de\Xceed.Wpf.AvalonDock.resources.dll"; DestDir: "{app}\de"
;Source: "..\TimetableTool.Desktop\bin\Release\netcoreapp3.1\es\Xceed.Wpf.AvalonDock.resources.dll"; DestDir: "{app}\es"
;Source: "..\TimetableTool.Desktop\bin\Release\netcoreapp3.1\fr\Xceed.Wpf.AvalonDock.resources.dll"; DestDir: "{app}\fr"
;Source: "..\TimetableTool.Desktop\bin\Release\netcoreapp3.1\hu\Xceed.Wpf.AvalonDock.resources.dll"; DestDir: "{app}\hu"
;Source: "..\TimetableTool.Desktop\bin\Release\netcoreapp3.1\pt-BR\Xceed.Wpf.AvalonDock.resources.dll"; DestDir: "{app}\pt-BR"
;Source: "..\TimetableTool.Desktop\bin\Release\netcoreapp3.1\it\Xceed.Wpf.AvalonDock.resources.dll"; DestDir: "{app}\it"
;Source: "..\TimetableTool.Desktop\bin\Release\netcoreapp3.1\ro\Xceed.Wpf.AvalonDock.resources.dll"; DestDir: "{app}\ro"
;Source: "..\TimetableTool.Desktop\bin\Release\netcoreapp3.1\ru\Xceed.Wpf.AvalonDock.resources.dll"; DestDir: "{app}\ru"
;Source: "..\TimetableTool.Desktop\bin\Release\netcoreapp3.1\sv\Xceed.Wpf.AvalonDock.resources.dll"; DestDir: "{app}\sv"
;Source: "..\TimetableTool.Desktop\bin\Release\netcoreapp3.1\ro\Xceed.Wpf.AvalonDock.resources.dll"; DestDir: "{app}\ro"
;Source: "..\TimetableTool.Desktop\bin\Release\netcoreapp3.1\zh-Hans\Xceed.Wpf.AvalonDock.resources.dll"; DestDir: "{app}\zh-Hans"
  

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:ProgramOnTheWeb,{#MyAppName}}"; Filename: "{#MyAppURL}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: quicklaunchicon

[Code]

[Run]
Filename: "{app}\{#MyAppExeName}"; Flags: nowait postinstall skipifsilent 32bit; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"

[Registry]

