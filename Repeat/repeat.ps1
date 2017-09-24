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
$folders = Get-ChildItem -Path $startFolder -Exclude $targetFolder | ?{ $_.PSIsContainer }
$folders|ForEach-Object {Move-Item $_* $startFolder\$targetFolder}
$zipFiles= Get-ChildItem -Path $startFolder
$zipFiles |Where-Object {$_.Extension -eq $archiveExtension}|ForEach-Object {Move-Item $_.FullName $startFolder\$targetFolder}
$pathTo7zip="C:\Program Files\7-Zip\7z.exe"
$archives="x ""$startFolder\$targetFolder\*$archiveExtension"""
$output="-o""$startFolder\$targetFolder\*"""
Start-Process -FilePath $pathTo7zip -ArgumentList @($archives,$output) -Wait
$targetZipFiles= Get-ChildItem -Path $startFolder\$targetFolder
$targetZipFiles |Where-Object {$_.Extension -eq $archiveExtension}|ForEach-Object {Remove-Item $_.FullName}
