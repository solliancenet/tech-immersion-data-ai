<# Custom Script for Windows #>
$Computername = $env:COMPUTERNAME
$Username = $config.public.vmLocalUserName          
$PlainPassword = $config.private.vmLocalUserPassword 
$Password = ConvertTo-SecureString $PlainPassword -AsPlainText -Force

#Create User
$ADSIComp = [adsi]"WinNT://$Computername"
$NewUser = $ADSIComp.Children | ? {$_.SchemaClassName -eq 'User' -and $_.Name -eq $Username};
if (!$NewUser) {
  $NewUser = $ADSIComp.Create('User', $Username)
}

#Create password
$BSTR = [system.runtime.interopservices.marshal]::SecureStringToBSTR($Password)
$_password = [system.runtime.interopservices.marshal]::PtrToStringAuto($BSTR)

#Set password on account 
$NewUser.SetPassword(($_password))
$NewUser.SetInfo()

##Add user to Local Administrators Group
#$AdminGroup = [ADSI]"WinNT://$Computername/Administrators,group"
#$User = [ADSI]"WinNT://$Computername/$Username,user"
#if (!($AdminGroup.Members() | ? {$_.Name() -eq $Username})) { $AdminGroup.Add($User.Path) }

#Add user to Local Remote Desktop Users Group
$LocalGroup = [ADSI]"WinNT://$Computername/Remote Desktop Users,group"
$User = [ADSI]"WinNT://$Computername/$Username,user"
if (!($LocalGroup.Members() | ? {$_.Name() -eq $Username})) {  $LocalGroup.Add($User.Path)}

#Set account to never expire
$User.UserFlags.value = $user.UserFlags.value -bor 0x10000
$User.CommitChanges()

#Setting logon picture to block colour
$registryPath = "HKLM:\Software\Policies\Microsoft\Windows\System"
$Name = "DisableLogonBackgroundImage"
$value = "1"
if (!(Test-Path $registryPath)) {
  New-Item -Path $registryPath -Force | Out-Null
  New-ItemProperty -Path $registryPath -Name $name -Value $value -PropertyType DWORD -Force | Out-Null
}
else {
  New-ItemProperty -Path $registryPath -Name $name -Value $value -PropertyType DWORD -Force | Out-Null
}  

# Move TaskAppList solution to desktop
$slnFile = "labfiles.zip"
$slnSourceLocation = "D:\Temp"
$slnDestinationLocation = "C:\lab-files"
if (!(Test-Path $slnDestinationLocation)) {
  New-Item $slnDestinationLocation -type directory
}
$shell = new-object -com shell.application
$zip = $shell.NameSpace("$slnSourceLocation\$slnFile")
foreach ($item in $zip.items()) {
  $shell.Namespace($slnDestinationLocation).copyhere($item, 0x14)
}

#Cleanup 
[Runtime.InteropServices.Marshal]::ZeroFreeBSTR($BSTR) 
Remove-Variable Password, BSTR, _password

#Set AutoadminLogin and Run Once
New-ItemProperty -Path 'HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon' -Name AutoAdminLogon -Value 1 -Force
New-ItemProperty -Path 'HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon' -Name AutoLogonCount -Value 1 -Force
New-ItemProperty -Path 'HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon' -Name DefaultUserName -Value "$Computername\$Username" -Force
New-ItemProperty -Path 'HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon' -Name DefaultPassword -Value $PlainPassword -Force
$RunOnceKey = "HKLM:\Software\Microsoft\Windows\CurrentVersion\RunOnce"
Set-ItemProperty $RunOnceKey "NextRun" "C:\Windows\System32\WindowsPowerShell\v1.0\Powershell.exe -ExecutionPolicy Unrestricted -File $temploc\PostReboot.ps1"

#Remove OneDrive from System Tray
New-Item -Path 'HKLM:\SOFTWARE\Policies\Microsoft\Windows' -Name OneDrive -Force
New-ItemProperty -Path 'HKLM:\SOFTWARE\Policies\Microsoft\Windows\OneDrive' -Name DisableFileSyncNGSC -value 1 -Force

#Remove Control Panel and Settings
New-Item -Path "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\"-Name "Explorer" -Force
New-ItemProperty -Path "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer" -Name "NoControlPanel" -Value "1" -PropertyType DWORD -Force | Out-Null

#WindowsUpdates Download
New-ItemProperty -Path 'HKLM:\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU' -Name NoAutoUpdate -value 1 -Force
New-ItemProperty -Path 'HKLM:\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU' -Name AUOptions -value 2 -Force

#Load default registry hive
& reg load HKLM\DEFAULT C:\Users\Default\NTUSER.DAT

#Allow InternetExplorer toolbars when running in InPrivate mode
New-Item -Path 'HKLM:\DEFAULT\SOFTWARE\Policies\Microsoft' -Name 'Internet Explorer' -Force
New-Item -Path 'HKLM:\DEFAULT\SOFTWARE\Policies\Microsoft\Internet Explorer' -Name 'Safety' -Force
New-Item -Path 'HKLM:\DEFAULT\SOFTWARE\Policies\Microsoft\Internet Explorer\Safety' -Name 'PrivacIE' -Force
New-ItemProperty -Path 'HKLM:\DEFAULT\SOFTWARE\Policies\Microsoft\Internet Explorer\Safety\PrivacIE' -Name DisableToolbars -Value 0 -PropertyType DWORD -Force

#Disable Balloon Notifications
New-ItemProperty -Path 'HKLM:\DEFAULT\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced' -Name EnableBalloonTips -Value 0 -PropertyType DWORD -Force

#Unload default registry hive
& reg unload HKLM\DEFAULT

#Reboot
write-output "Restart Server"
& shutdown /r /t 30 /f