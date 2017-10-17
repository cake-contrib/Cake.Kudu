$CakeVersion = "0.19.3"
$DotNetChannel = "preview";
$DotNetVersion = "1.0.0-preview2-003121";
$DotNetInstallerUri = "https://raw.githubusercontent.com/dotnet/cli/rel/1.0.0-preview2/scripts/obtain/dotnet-install.ps1";

# Make sure tools folder exists
$PSScriptRoot   = Split-Path $MyInvocation.MyCommand.Path -Parent
$ToolPath       = Join-Path $PSScriptRoot "tools"
$AddinsPath     = Join-Path $ToolPath "Addins"

if (!(Test-Path $ToolPath)) {
    Write-Verbose "Creating tools directory..."
    New-Item -Path $ToolPath -Type directory | Out-Null
}
if (!(Test-Path $AddinsPath)) {
    Write-Verbose "Creating addins directory..."
    New-Item -Path $AddinsPath -Type directory | Out-Null
}

###########################################################################
# INSTALL .NET CORE CLI
###########################################################################

Function Remove-PathVariable([string]$VariableToRemove)
{
    $path = [Environment]::GetEnvironmentVariable("PATH", "User")
    if ($path -ne $null)
    {
        $newItems = $path.Split(';', [StringSplitOptions]::RemoveEmptyEntries) | Where-Object { "$($_)" -inotlike $VariableToRemove }
        [Environment]::SetEnvironmentVariable("PATH", [System.String]::Join(';', $newItems), "User")
    }

    $path = [Environment]::GetEnvironmentVariable("PATH", "Process")
    if ($path -ne $null)
    {
        $newItems = $path.Split(';', [StringSplitOptions]::RemoveEmptyEntries) | Where-Object { "$($_)" -inotlike $VariableToRemove }
        [Environment]::SetEnvironmentVariable("PATH", [System.String]::Join(';', $newItems), "Process")
    }
}

# Get .NET Core CLI path if installed.
$FoundDotNetCliVersion = $null;
if (Get-Command dotnet -ErrorAction SilentlyContinue) {
    $FoundDotNetCliVersion = dotnet --version;
}

if($FoundDotNetCliVersion -ne $DotNetVersion) {
    $InstallPath = Join-Path $PSScriptRoot ".dotnet"
    if (!(Test-Path $InstallPath)) {
        mkdir -Force $InstallPath | Out-Null;
    }
    (New-Object System.Net.WebClient).DownloadFile($DotNetInstallerUri, "$InstallPath\dotnet-install.ps1");
    & $InstallPath\dotnet-install.ps1 -Channel $DotNetChannel -Version $DotNetVersion -InstallDir $InstallPath;

    Remove-PathVariable "$InstallPath"
    $env:PATH = "$InstallPath;$env:PATH"
    $env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
    $env:DOTNET_CLI_TELEMETRY_OPTOUT=1
}

###########################################################################
# INSTALL CAKE
###########################################################################

Add-Type -AssemblyName System.IO.Compression.FileSystem
Function Unzip
{
    param([string]$zipfile, [string]$outpath)

    [System.IO.Compression.ZipFile]::ExtractToDirectory($zipfile, $outpath)
}


# Make sure Cake has been installed.
$CakePath = Join-Path $ToolPath "Cake.CoreCLR.$CakeVersion/Cake.dll"
if (!(Test-Path $CakePath)) {
    Write-Host "Installing Cake..."
    (New-Object System.Net.WebClient).DownloadFile("https://www.nuget.org/api/v2/package/Cake.CoreCLR/$CakeVersion", "$ToolPath\Cake.CoreCLR.zip")
    Unzip "$ToolPath\Cake.CoreCLR.zip" "$ToolPath/Cake.CoreCLR.$CakeVersion"
    Remove-Item "$ToolPath\Cake.CoreCLR.zip"
}

$JsonNetPath = Join-Path $AddinsPath "Newtonsoft.Json/lib/netstandard1.0/Newtonsoft.Json.dll"
if (!(Test-Path $JsonNetPath)) {
    Write-Host "Installing JSON.Net..."
    $ZipPath = "$AddinsPath\Newtonsoft.Json.zip"
    (New-Object System.Net.WebClient).DownloadFile("https://www.nuget.org/api/v2/package/Newtonsoft.Json/9.0.1",  $ZipPath)
    Unzip $ZipPath "$AddinsPath/Newtonsoft.Json"
    Remove-Item $ZipPath
}

###########################################################################
# RUN BUILD SCRIPT
###########################################################################

& dotnet "$CakePath" $args
exit $LASTEXITCODE