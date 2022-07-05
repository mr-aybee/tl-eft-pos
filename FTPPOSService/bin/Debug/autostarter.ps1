$registryPath = "HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\Run"
$Name = "FTPPOSService"
$currentPath = Get-Location
$value = -join($currentPath ,"\FTPPOSService.exe")
New-ItemProperty -Path $registryPath -Name $name -PropertyType String -Value $value