param([string[]]$computers)
Write-Host 'Collecting data...' -ForegroundColor Green
# loop all computers
foreach ($computer in $computers)
{
    Invoke-Command -ComputerName $computer -ScriptBlock {Get-NetTCPConnection | where {$_.localaddress -ne '::1' -and $_.localaddress -ne '::' -and $_.localaddress -ne '0.0.0.0' -and $_.localaddress -ne '127.0.0.1'}} -AsJob
}
# wait for all jobs to finish, then receive them
get-job | Wait-Job | Receive-Job
# remove all jobs
get-job | Remove-Job
# prevent exit
read-host -Prompt 'Press any key to continue..'