
; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "BCS Gujrat"
#define MyAppVersion "0.2"
#define MyAppPublisher "Crystal Power, Inc."
#define MyAppURL "https://www.example.com/"
#define MyAppExeName "CrystalPowerBCS.exe"
#define MyAppAssocName MyAppName + ""
#define MyAppAssocExt ".myp"
#define MyAppAssocKey StringChange(MyAppAssocName, " ", "") + MyAppAssocExt

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{4922132B-A4CA-4A12-8F9C-B00B9697B85B}}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\Crystal Power
DefaultGroupName={#MyAppName}
OutputDir=C:\Users\Beyond Root\Desktop\Build\Bcs_Gujrat\CPBCSEXE
OutputBaseFilename=CrystalPowerGujrat
SetupIconFile=C:\Users\Beyond Root\Desktop\Build\Bcs_Gujrat\Build\Untitled design.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern
DirExistsWarning=auto 
PrivilegesRequired=admin

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Dirs]
Name: {app}; Permissions: users-full

[Files]
//Source: "C:\path\to\database\*"; DestDir: "{app}\Data"; Flags: recursesubdirs onlyifdoesntexist
Source: "C:\Users\Beyond Root\Desktop\Build\Bcs_Gujrat\Build\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Beyond Root\Desktop\Build\Bcs_Gujrat\Build\*"; DestDir: "{app}"; Excludes: "crystalPowerDb.db"; Flags: ignoreversion recursesubdirs 

[Registry]
Root: HKA; Subkey: "Software\Classes\{#MyAppAssocExt}\OpenWithProgids"; ValueType: string; ValueName: "{#MyAppAssocKey}"; ValueData: ""; Flags: uninsdeletevalue
Root: HKA; Subkey: "Software\Classes\{#MyAppAssocKey}"; ValueType: string; ValueName: ""; ValueData: "{#MyAppAssocName}"; Flags: uninsdeletekey
Root: HKA; Subkey: "Software\Classes\{#MyAppAssocKey}\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\{#MyAppExeName},0"
Root: HKA; Subkey: "Software\Classes\{#MyAppAssocKey}\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\{#MyAppExeName}"" ""%1"""
Root: HKA; Subkey: "Software\Classes\Applications\{#MyAppExeName}\SupportedTypes"; ValueType: string; ValueName: ".myp"; ValueData: ""

[Icons]
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: postinstall skipifsilent runasoriginaluser

[Code]

function InitializeSetup(): Boolean;
var
  ExistingAppPath: String;
  ResultCode: Integer;
  ConfirmResult: Integer;
begin
  if not IsAdminLoggedOn then
  begin
    // Display a message to run the installer with administrator privileges
    MsgBox('Please run this installer with administrator privileges.' + #13#10 + 'Right-click on the installer executable and choose "Run as administrator."', mbInformation, MB_OK);

    // Abort the installation if the user doesn't have administrator privileges
    Result := False;
    Exit;
  end;

  // Specify the path to the existing installed application
  ExistingAppPath := ExpandConstant('{pf}\Crystal Power\{#MyAppExeName}');

  // Check if the application is already installed
  if FileExists(ExistingAppPath) then
  begin
    // Application is already installed; prompt the user to confirm reinstallation
    ConfirmResult := MsgBox('The application is already installed. Do you want to proceed with the installation?', mbConfirmation, MB_YESNO);
    if ConfirmResult = IDNO then
    begin
      Result := False; // User chose not to proceed with installation
      Exit;
    end;

    // Try to close the running application gracefully
    if not ShellExec('', ExistingAppPath, '', '', SW_SHOWNORMAL, ewNoWait, ResultCode) then
    begin
      MsgBox('Failed to close the running application. Please close it manually and then click "Yes" to proceed with the installation.', mbInformation, MB_OK);
      Result := False; // Abort installation
      Exit;
    end;

    // Wait for a short time to allow the application to close
    Sleep(2000);

    // Uninstall the existing application
    if not Exec(ExistingAppPath, '/SILENT', '', SW_SHOW, ewWaitUntilTerminated, ResultCode) then
    begin
      MsgBox('Failed to uninstall the existing application with error code: ' + IntToStr(ResultCode), mbError, MB_OK);
      Result := False; // Abort installation
      Exit;
    end;
  end;

  // Continue with the installation
  Result := True;
end;


