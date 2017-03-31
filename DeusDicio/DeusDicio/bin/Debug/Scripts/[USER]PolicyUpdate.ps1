param([string[]]$computers)

# loop all computers
foreach ($computer in $computers)
{
    # invoke policy refresh on each computer    
    Invoke-Command -ComputerName $computer -ScriptBlock {gpupdate /force} -AsJob | Out-Null
    Write-Host "Sending update command to $computer.." -ForegroundColor Yellow
}
# wait for all jobs to finish, then receive them
get-job | Wait-Job | Receive-Job
# remove all jobs
get-job | Remove-Job
# prevent exit
read-host -Prompt 'Press any key to continue..'