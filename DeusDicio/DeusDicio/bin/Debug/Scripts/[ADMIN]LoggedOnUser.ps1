param([string[]]$computers)
Write-Host 'Gathering processes...' -ForegroundColor Green
# loop all computers
foreach ($computer in $computers)
{
    # run as job for scaling
    start-job -ArgumentList $computer -ScriptBlock {
    param($computer)
    try
        {
        $output = @()
            # get processes on computer
            $processinfo = @(Get-WmiObject -class win32_process -ComputerName $Computer -EA "Stop")
                if ($processinfo)
                {   
                    # filter processes for output, removing local accounts
                    $processinfo | Foreach-Object {$_.GetOwner().User} | 
                    Where-Object {$_ -ne "NETWORK SERVICE" -and $_ -ne "LOCAL SERVICE" -and $_ -ne "SYSTEM"} |
                    Sort-Object -Unique |
                    ForEach-Object { New-Object psobject -Property @{Computer=$Computer;LoggedOn=$_} } | 
                    Select-Object Computer,LoggedOn | Format-Table
                }#If                
        }
    catch
        {
            "Cannot find any processes running on $computer" | Out-Host
        }
        } | out-null # end scriptblock
}
# wait for all jobs to finish, then receive them
get-job | Wait-Job | Receive-Job
# remove all jobs
get-job | Remove-Job
# prevent exit
read-host -Prompt 'Press any key to continue..'