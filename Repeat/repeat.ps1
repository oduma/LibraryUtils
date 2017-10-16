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

function GetFileName([string] $startFolder, [string] $torrentFolder, [string] $torrentContentLine)
{
    $torrentLineParts = $torrentContentLine.Split('(')
    $torrentFileName=$torrentLineParts[0].Trim()
    $torrentFileName = $torrentFileName -replace " +\d+$", ""
    return "$startFolder\$torrentFolder\$torrentFileName"
}
function GetTorrentExpectedContents([string] $startFolder, [string] $torrentFile)
{
    $torrentFileContentLines=cmd /c ".\dumptorrent-1.2.exe" "$startFolder\$torrentFile"
    $startedFiles =$false
    [System.Collections.ArrayList] $torrentFileContent=@("")
    $torrentFolder=""
    foreach($torrentFileContentLine in $torrentFileContentLines)
    {
        if($torrentFileContentLine.StartsWith("Name:"))
        {
            $torrentFolder=$torrentFileContentLine.Replace("Name:           ","")
        }
        if($torrentFileContentLine -eq "Files:")
        {
            $startedFiles=$true
        }
        if($torrentFileContentLine -ne "Files:" -And $startedFiles)
        {
            $torrentFileName=GetFileName $startFolder $torrentFolder $torrentFileContentLine.Trim()
            [void]$torrentFileContent.Add($torrentFileName)
        }
    }
    $retValue = New-Object PSObject -Property @{ TorrentFolder=$torrentFolder; TorrentContent= $torrentFileContent.ToArray()}
    return $retValue
}

function FilesMissing([System.Collections.ArrayList] $filesInTorrent)
{
    foreach($fileInTorrent in $filesInTorrent)
    {

        if(([string]::IsNullOrEmpty($fileInTorrent) -ne $true) -And ([System.IO.File]::Exists($fileInTorrent) -ne $true))
        {
            if([System.IO.Directory]::Exists($fileInTorrent) -ne $true)
            {
                Write-Host "Cannot find: $fileInTorrent"
                return $true
            }
        }
    }
    return $false
}
function GetExcludeList()
{
    $torrentFiles= Get-ChildItem -Path $startFolder -Filter "*.torrent"
    [System.Collections.ArrayList] $foldersToExclude = @($targetFolder)
    [System.Collections.ArrayList] $torrentsToExclude = @("")
    $retValue = New-Object PSObject -Property @{ FoldersToExclude=$foldersToExclude; TorrentsToExclude= $torrentsToExclude}
    foreach($torrentFile in $torrentFiles)
    {
        $torrentExpectedContents= GetTorrentExpectedContents $startFolder $torrentFile
        $inWork=FilesMissing -filesInTorrent $torrentExpectedContents.TorrentContent
        if($inWork)
        {
            [void]$retValue.FoldersToExclude.Add($torrentExpectedContents.TorrentFolder)
            [void]$retValue.TorrentsToExclude.Add($torrentFile)
        }
    }
    return $retValue
}

$excludes=GetExcludeList
Write-Host $excludes.FoldersToExclude
Write-Host $excludes.TorrentsToExclude
$folders = Get-ChildItem -Path $startFolder -Exclude $excludes.FoldersToExclude | ?{ $_.PSIsContainer }
$folders|ForEach-Object {Move-Item $_* $startFolder\$targetFolder}
$torrentFiles=Get-ChildItem -Path "$startFolder\*.torrent" -Exclude $excludes.TorrentsToExclude
$torrentFiles|ForEach-Object {Remove-Item -LiteralPath $_.FullName}

$zipFiles= Get-ChildItem -Path $startFolder
$zipFiles |Where-Object {$_.Extension -eq $archiveExtension}|ForEach-Object {Move-Item $_.FullName $startFolder\$targetFolder}
$pathTo7zip="C:\Program Files\7-Zip\7z.exe"
$archives="x ""$startFolder\$targetFolder\*$archiveExtension"""
$output="-o""$startFolder\$targetFolder\*"""
Start-Process -FilePath $pathTo7zip -ArgumentList @($archives,$output) -Wait
$targetZipFiles= Get-ChildItem -Path $startFolder\$targetFolder
$targetZipFiles |Where-Object {$_.Extension -eq $archiveExtension}|ForEach-Object {Remove-Item $_.FullName}
#now move them in the right folders
$pathTot2f=".\Sciendo.T2F.exe"
Write-Host "Executing: $pathTot2f $targetFolder, Move, -i %a\%l\%n - %t, -c %l\%n - %a - %t"
Start-Process -FilePath $pathTot2f -ArgumentList @("$targetFolder","Move","-i","%a\%l\%n - %t","-c","%l\%n - %a - %t") -Wait
