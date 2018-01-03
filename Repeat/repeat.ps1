param([string]$startFolder,[string]$targetFolder,[string]$archiveExtension)
if([string]::IsNullOrEmpty($startFolder))
{
    $startFolder="C:\Code\work\Sciendo\Repeat"
}
if([string]::IsNullOrEmpty($targetFolder))
{
    $targetFolder="Tar get"
}
if([string]::IsNullOrEmpty($archiveExtension))
{
    $archiveExtension=".zip"
}

######Move all the archives
$zipFiles= Get-ChildItem -Path $startFolder
$zipFiles |Where-Object {$_.Extension -eq $archiveExtension}|ForEach-Object {Move-Item $_.FullName $startFolder\$targetFolder}

######Unarchive all copied archives
$pathTo7zip="C:\Program Files\7-Zip\7z.exe"
$archives="x ""$startFolder\$targetFolder\*$archiveExtension"""
$output="-o""$startFolder\$targetFolder\*"""
Start-Process -FilePath $pathTo7zip -ArgumentList @($archives,$output) -Wait

######Delete all archives
$targetZipFiles= Get-ChildItem -Path $startFolder\$targetFolder
$targetZipFiles |Where-Object {$_.Extension -eq $archiveExtension}|ForEach-Object {Remove-Item $_.FullName}

######now move them in the right folders
$pathTot2f=".\Sciendo.T2F.exe"
$targetPath="""$startFolder\$targetFolder"""
Write-Host "Executing: $pathTot2f $targetPath"
Start-Process -FilePath $pathTot2f -ArgumentList @("$targetPath")  -Wait
