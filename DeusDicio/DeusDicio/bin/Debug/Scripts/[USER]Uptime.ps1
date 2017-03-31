param([string[]]$computers)
Write-Host 'Getting Data...' -ForegroundColor Green
# loop all computers
foreach ($computer in $computers)
{
    # run script on each computer
    Invoke-Command -computername $computer -scriptblock {
        # get time of startup
        $bootup = Get-CimInstance -ClassName win32_operatingsystem | select csname, lastbootuptime
        # get now
        $now = Get-Date 
        # compare startup and now
        $Uptime = $now - $bootup.lastbootuptime        
        # format
        $d = $Uptime.Days 
        $h = $Uptime.Hours 
        $m = $uptime.Minutes 
        $ms= $uptime.Milliseconds
        # output
        "$env:computername Up for: {0} days, {1} hours, {2}.{3} minutes, startuptime $($bootup.lastbootuptime.DateTime)" -f $d,$h,$m,$ms
        } -AsJob | Out-Null

}
# wait for all jobs to finish, then receive them
get-job | Wait-Job | Receive-Job
# remove all jobs
get-job | Remove-Job
# prevent exit
read-host -Prompt 'Press any key to continue..'