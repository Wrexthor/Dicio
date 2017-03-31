param([string[]]$computers)
#$computers = 'srv', 'localhost'
write-host 'Pinging..' -ForegroundColor Green
# loop all computers
foreach ($computer in $computers)
{
    # ping each computer with 1 packet as job, no output
    write-host "Pinging $computer..." -ForegroundColor Yellow
    Test-Connection -Count 1 -ComputerName $computer -AsJob | Out-Null    
}
# wait for all jobs to finish, then receive them
$job = get-job | Wait-Job | Receive-Job
foreach ($j in $job)
{
    if ($j.ResponseTime -eq $null) {Write-Host "$(($j.Address).ToUpper()) not responding, IP: $($j.IPV4Address)"  -ForegroundColor Red}
    else {Write-Host "$(($j.Address).ToUpper()) responding, response time: $($j.ResponseTime)ms, IP: $($j.IPV4Address)" -ForegroundColor Green}
}

# remove all jobs
get-job | remove-job
# prevent exit
read-host -Prompt 'Press any key to continue..'