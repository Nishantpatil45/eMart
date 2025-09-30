param(
    [string]$SolutionRoot = "D:\Projects\Dot Net\eMart",
    [string]$ProjectPath = "eMart.Service.Api",
    [string]$PublishOutput = "publish\eMart.Service.Api",
    [string]$SiteName = "eMart.Api",
    [string]$AppPoolName = "eMartApiPool",
    [int]$Port = 8090,
    [string]$Environment = "Development",
    [string]$ConnectionString = "Server=localhost;Database=emart;User=root;Password=system;",
    [string]$JwtSecretKey = "this-is-a-secure-key-32-characters-long",
    [string]$JwtIssuer = "https://localhost:$Port",
    [string]$JwtAudience = "https://localhost:$Port",
    [int]$JwtAccessTokenExpiryMinutes = 15,
    [int]$JwtRefreshTokenExpiryDays = 7,
    [switch]$OverwriteAppSettings
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Write-Info($msg) { Write-Host "[INFO]  $msg" -ForegroundColor Cyan }
function Write-Warn($msg) { Write-Host "[WARN]  $msg" -ForegroundColor Yellow }
function Write-Err($msg)  { Write-Host "[ERROR] $msg" -ForegroundColor Red }

# Resolve paths
$projFullPath = Join-Path $SolutionRoot $ProjectPath
$publishFullPath = Join-Path $SolutionRoot $PublishOutput

Write-Info "Project: $projFullPath"
Write-Info "Publish folder: $publishFullPath"

if (!(Test-Path $projFullPath)) { throw "Project path not found: $projFullPath" }

# Restore, build, publish
Write-Info "Restoring..."
pushd $projFullPath
dotnet restore | Write-Host

Write-Info "Building Release..."
dotnet build -c Release | Write-Host

Write-Info "Publishing..."
New-Item -ItemType Directory -Force -Path $publishFullPath | Out-Null
dotnet publish -c Release -o $publishFullPath | Write-Host
popd

# Create/overwrite appsettings.Production.json
$appSettingsProd = Join-Path $publishFullPath 'appsettings.Production.json'
if ((-not (Test-Path $appSettingsProd)) -or $OverwriteAppSettings.IsPresent) {
    Write-Info "Writing appsettings.Production.json"
    $json = @{
        Logging = @{ LogLevel = @{ Default = 'Information'; 'Microsoft.AspNetCore' = 'Warning' } }
        ConnectionStrings = @{ DefaultConnection = $ConnectionString }
        Jwt = @{ 
            SecretKey = $JwtSecretKey
            Issuer = $JwtIssuer
            Audience = $JwtAudience
            AccessTokenExpiryMinutes = $JwtAccessTokenExpiryMinutes
            RefreshTokenExpiryDays = $JwtRefreshTokenExpiryDays
        }
    } | ConvertTo-Json -Depth 5
    $json | Out-File -FilePath $appSettingsProd -Encoding utf8 -Force
} else {
    Write-Warn "appsettings.Production.json already exists. Use -OverwriteAppSettings to overwrite."
}

# Ensure logs folder for optional stdout logging
$logsFolder = Join-Path $publishFullPath 'logs'
if (!(Test-Path $logsFolder)) { New-Item -ItemType Directory -Force -Path $logsFolder | Out-Null }

# Install IIS management module
Import-Module WebAdministration -ErrorAction SilentlyContinue
if (-not (Get-Module WebAdministration)) {
    throw "WebAdministration module not available. Ensure IIS is installed."
}

# Create or reuse App Pool
if (Test-Path IIS:\AppPools\$AppPoolName) {
    Write-Info "App Pool '$AppPoolName' exists. Updating settings..."
} else {
    Write-Info "Creating App Pool '$AppPoolName'..."
    New-WebAppPool -Name $AppPoolName | Out-Null
}

Set-ItemProperty IIS:\AppPools\$AppPoolName -Name managedRuntimeVersion -Value ''
Set-ItemProperty IIS:\AppPools\$AppPoolName -Name managedPipelineMode -Value 'Integrated'
Set-ItemProperty IIS:\AppPools\$AppPoolName -Name startMode -Value 'AlwaysRunning'
Set-ItemProperty IIS:\AppPools\$AppPoolName -Name processModel.idleTimeout -Value ([TimeSpan]::FromMinutes(0))

# Grant folder permissions to App Pool identity
$appPoolIdentity = "IIS AppPool\$AppPoolName"
Write-Info "Granting Modify permission to '$appPoolIdentity' on '$publishFullPath'"
# Build grant string to avoid PowerShell parsing (OI)(CI)
$grantArg = '"{0}":(OI)(CI)M' -f $appPoolIdentity
icacls "$publishFullPath" /grant $grantArg /T | Out-Null

# Create or update Website
if (Test-Path IIS:\Sites\$SiteName) {
    Write-Info "Site '$SiteName' exists. Updating physical path and binding..."
    Set-ItemProperty IIS:\Sites\$SiteName -Name physicalPath -Value $publishFullPath
} else {
    Write-Info "Creating Website '$SiteName'..."
    New-Website -Name $SiteName -PhysicalPath $publishFullPath -Port $Port -Force | Out-Null
}

Set-ItemProperty IIS:\Sites\$SiteName -Name applicationPool -Value $AppPoolName

# Ensure binding
$bindingPattern = ('*:' + $Port + ':*')
$existingBinding = (Get-WebBinding -Name $SiteName -Protocol 'http' -ErrorAction SilentlyContinue | Where-Object { $_.bindingInformation -like $bindingPattern })
if (-not $existingBinding) {
    Write-Info "Adding HTTP binding on port $Port"
    New-WebBinding -Name $SiteName -Protocol http -Port $Port -IPAddress '*'
}

# Set ASPNETCORE_ENVIRONMENT
Write-Info "Setting ASPNETCORE_ENVIRONMENT=$Environment"
Set-WebConfigurationProperty -Filter "/system.webServer/aspNetCore/environmentVariables" -PSPath "IIS:\Sites\$SiteName" -Name "." -Value @{ name = 'ASPNETCORE_ENVIRONMENT'; value = $Environment } -ErrorAction SilentlyContinue | Out-Null
Add-WebConfigurationProperty -Filter "/system.webServer/aspNetCore/environmentVariables" -PSPath "IIS:\Sites\$SiteName" -Name "." -Value @{ name = 'ASPNETCORE_ENVIRONMENT'; value = $Environment } -ErrorAction SilentlyContinue | Out-Null

# Optionally enable stdout logging for first run
$webConfigPath = Join-Path $publishFullPath 'web.config'
if (Test-Path $webConfigPath) {
    try {
        [xml]$webConfig = Get-Content $webConfigPath -ErrorAction Stop
        $aspNetCore = $webConfig.configuration.'system.webServer'.aspNetCore
        if ($aspNetCore) {
            $aspNetCore.stdoutLogEnabled = 'false'
            $aspNetCore.stdoutLogFile = '.\\logs\\stdout'
            $webConfig.Save($webConfigPath)
        }
    } catch { Write-Warn "Could not update web.config stdout settings: $_" }
}

# Restart site
Write-Info "Restarting site '$SiteName'"
Stop-Website -Name $SiteName -ErrorAction SilentlyContinue | Out-Null
Start-Website -Name $SiteName | Out-Null

Write-Host ""
Write-Host "==================================================" -ForegroundColor Green
Write-Host "Deployment complete." -ForegroundColor Green
Write-Host "Site: http://localhost:$Port/swagger" -ForegroundColor Green
Write-Host "App Pool: $AppPoolName" -ForegroundColor Green
Write-Host "Folder: $publishFullPath" -ForegroundColor Green
Write-Host "==================================================" -ForegroundColor Green


