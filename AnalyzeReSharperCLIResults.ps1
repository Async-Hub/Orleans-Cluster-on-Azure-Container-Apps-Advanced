$outputFile = "inspect-code-output\report.xml"

[xml]$xml = Get-Content $outputFile
if ($xml.Report.Issues.ChildNodes.Count -gt 0)
{
   Write-Output ("`nReSharper.CommandLineTools: code validation failed: `n" + ((Get-Content $outputFile) -join "`n"))
}
else
{
   Write-Output "ReSharper.CommandLineTools: code validation passed."
}