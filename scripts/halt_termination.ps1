$continue = $true

$CallSignalingPort2 = [int]$env:AzureSettings__CallSignalingPort + 1

while($continue)
{
    try
    {
        Write-Host "Calling endpoint to check for active calls..."
        $result = Invoke-WebRequest -Uri "http://localhost:$CallSignalingPort2/calls" -UseBasicParsing

        Write-Host "Response content: $($result.Content)"
        $calls = $result.Content | ConvertFrom-Json

        if ($calls.Count -gt 0)
        {
            Write-Host "Active calls detected. Halting termination."
            Start-Sleep -Seconds 60
        }
        else
        {
            Write-Host "No active calls. Proceeding with termination."
            $continue = $false
        }
    }
    catch
    {
        Write-Host "Error while calling endpoint."
    }
}
