$outputFile = "InspectCodeOutput\inspect-code-log.xml"

# https://www.jetbrains.com/resharper/download/other.html
if (!(Test-Path "JetBrains.ReSharper.CommandLineTools.2023.3.4.zip" -PathType Leaf)) {
   curl.exe -L --output JetBrains.ReSharper.CommandLineTools.2023.3.4.zip `
      --url https://download.jetbrains.com/resharper/dotUltimate.2023.3.4/JetBrains.ReSharper.CommandLineTools.2023.3.4.Checked.zip
}

if (!(Test-Path "JetBrains.ReSharper.CommandLineTools.2023.3.4" -PathType Container)) {
   Expand-Archive JetBrains.ReSharper.CommandLineTools.2023.3.4.zip -DestinationPath JetBrains.ReSharper.CommandLineTools.2023.3.4
}

JetBrains.ReSharper.CommandLineTools.2023.3.4\InspectCode.exe "ShoppingApp.sln" --build -o="$outputFile" `
   -f=Xml --profile="ShoppingApp.sln.DotSettings" --caches-home="caches" --properties:Configuration=Release

[xml]$xml = Get-Content $outputFile
if ($xml.Report.Issues.ChildNodes.Count -gt 0)
{
   Write-Output ("`nReSharper.CommandLineTools.2023.3.4: code validation failed: `n" + ((Get-Content $outputFile) -join "`n"))
}
else
{
   Write-Output "ReSharper.CommandLineTools.2023.3.4: code validation passed."
}