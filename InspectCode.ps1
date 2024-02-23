$outputFile = "InspectCodeOutput\inspect-code-log.xml"

# https://www.jetbrains.com/resharper/download/other.html
if (!(Test-Path "JetBrains.ReSharper.CommandLineTools.2022.2.3.zip" -PathType Leaf)) {
   curl.exe -L --output JetBrains.ReSharper.CommandLineTools.2022.2.3.zip `
      --url https://download.jetbrains.com/resharper/dotUltimate.2022.2.3/JetBrains.ReSharper.CommandLineTools.2022.2.3.Checked.zip
}

if (!(Test-Path "JetBrains.ReSharper.CommandLineTools.2022.2.3" -PathType Container)) {
   Expand-Archive JetBrains.ReSharper.CommandLineTools.2022.2.3.zip -DestinationPath JetBrains.ReSharper.CommandLineTools.2022.2.3
}

JetBrains.ReSharper.CommandLineTools.2022.2.3\InspectCode.exe "ShoppingApp.sln" --build -o="$outputFile" -f=Xml --profile="ShoppingApp.sln.DotSettings" --caches-home="caches"

[xml]$xml = Get-Content $outputFile
if ($xml.Report.Issues.ChildNodes.Count -gt 0)
{
   Write-Output ("`nReSharper.CommandLineTools.2022.2.3: code validation failed: `n" + ((Get-Content $outputFile) -join "`n"))
}
else
{
   Write-Output "ReSharper.CommandLineTools.2022.2.3: code validation passed."
}