param([string[]]$computers)
Write-Host 'Script for testing...' -ForegroundColor Green
# loop all computers
foreach ($computer in $computers)
{
    Write-Host $computer    
}
# prevent exit
read-host -Prompt 'Press any key to continue..'