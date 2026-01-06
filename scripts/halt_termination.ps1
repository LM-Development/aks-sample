$continue = $true

$CallSignalingPort2 = [int]$env:AzureSettings__CallSignalingPort + 1

while($continue)
{
    try
    {
        $result = Invoke-WebRequest -Uri "http://localhost:$CallSignalingPort2/calls" -UseBasicParsing
        $calls = $result.Content | ConvertFrom-Json

        if ($calls.Count -gt 0)
        {
            Start-Sleep -Seconds 60
        }
        else
        {
            $continue = $false
        }
    }
    catch
    {
        "Error while calling endpoint."
    }
}
