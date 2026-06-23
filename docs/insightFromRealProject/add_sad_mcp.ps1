# SAD Course MCP Setup Script
# Run this script in PowerShell to add the course MCP server to Claude Desktop

$configPath = "$env:APPDATA\Claude\claude_desktop_config.json"

# Read existing config or create new
if (Test-Path $configPath) {
    $config = Get-Content $configPath -Raw | ConvertFrom-Json
    Write-Host "Found existing config at: $configPath"
} else {
    Write-Host "No config found, creating new one..."
    $config = [PSCustomObject]@{ mcpServers = [PSCustomObject]@{} }
}

# Ensure mcpServers section exists
if (-not $config.PSObject.Properties['mcpServers']) {
    $config | Add-Member -MemberType NoteProperty -Name 'mcpServers' -Value ([PSCustomObject]@{})
}

# Add the SAD course MCP server
$newServer = [PSCustomObject]@{
    command = "npx"
    args    = @("-y", "sad-mcp@latest")
}

$config.mcpServers | Add-Member -MemberType NoteProperty -Name 'nituz' -Value $newServer -Force

# Save back to file (pretty-printed)
$config | ConvertTo-Json -Depth 10 | Set-Content $configPath -Encoding UTF8

Write-Host ""
Write-Host "✅ Done! 'nituz' MCP server added to Claude Desktop config."
Write-Host ""
Write-Host "Next steps:"
Write-Host "  1. Fully quit Claude Desktop (File → Quit or tray icon → Quit)"
Write-Host "  2. Relaunch Claude Desktop"
Write-Host "  3. A browser window will open — sign in with your BGU email (@post.bgu.ac.il)"
Write-Host "  4. Test by typing: 'List all available course materials for the SAD course'"
Write-Host ""
Write-Host "Current config saved to: $configPath"
