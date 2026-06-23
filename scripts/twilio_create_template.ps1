<#
.SYNOPSIS
    Creates the "edd_trip_offer" WhatsApp template (7 variables) in your Twilio account via the
    Content API, and optionally submits it to WhatsApp/Meta for approval.

.DESCRIPTION
    The Twilio CLI's auto-generated `content:create` command can't take the nested template body as
    flags, so this script calls the Content API directly (https://content.twilio.com/v1/Content)
    with HTTP Basic auth (Account SID : Auth Token). On success it prints the Content SID ("HX...")
    — paste that into the app's Settings screen (Twilio "Template SID") or app.config Twilio.ContentSid.

    Credentials are read from -AccountSid/-AuthToken, else the TWILIO_ACCOUNT_SID / TWILIO_AUTH_TOKEN
    environment variables, else you are prompted. Nothing is written to disk.

.EXAMPLE
    # Connect the CLI once (interactive — answer Y to "make active"):
    #   twilio profiles:create ACxxxxxxxx --auth-token <token> -p edd
    # Then create the template (reads creds from the prompt):
    pwsh ./scripts/twilio_create_template.ps1

.EXAMPLE
    # Create AND submit for WhatsApp approval (needs a connected WhatsApp sender / WABA):
    $env:TWILIO_ACCOUNT_SID='ACxxxx'; $env:TWILIO_AUTH_TOKEN='xxxx'
    pwsh ./scripts/twilio_create_template.ps1 -SubmitApproval
#>
[CmdletBinding()]
param(
    [string] $AccountSid = $env:TWILIO_ACCOUNT_SID,
    [string] $AuthToken  = $env:TWILIO_AUTH_TOKEN,
    [string] $FriendlyName = 'edd_trip_offer',
    [string] $Language = 'en',
    [ValidateSet('UTILITY','MARKETING')]
    [string] $Category = 'UTILITY',
    [switch] $SubmitApproval
)

$ErrorActionPreference = 'Stop'

if ([string]::IsNullOrWhiteSpace($AccountSid)) { $AccountSid = Read-Host 'Twilio Account SID (AC...)' }
if ([string]::IsNullOrWhiteSpace($AuthToken)) {
    $sec = Read-Host 'Twilio Auth Token' -AsSecureString
    $AuthToken = [System.Net.NetworkCredential]::new('', $sec).Password
}
$cred = [pscredential]::new($AccountSid, (ConvertTo-SecureString $AuthToken -AsPlainText -Force))

# The template body. {{1}}..{{7}} are filled at send time by DispatchService.SendOffer (same order).
$body = "Hello {{1}}, new trip offer from Transfers TLV:`n" +
        "🚗 · · · 📍`n" +
        "⚫ Pickup: {{2}} at {{3}}`n" +
        "⚫ Destination: {{4}}`n" +
        "Passengers: {{5}} | Pay: {{6}}`n" +
        "Accept or decline here: {{7}}`n" +
        "— Transfers TLV"

# Sample values shown in the Twilio/Meta approval preview (one per variable).
$payload = @{
    friendly_name = $FriendlyName
    language      = $Language
    variables     = @{
        '1' = 'John'
        '2' = 'Ben Gurion Airport T3'
        '3' = '2026-06-16 14:30'
        '4' = 'Tel Aviv, Rothschild 22'
        '5' = '3'
        '6' = '250 ILS'
        '7' = 'https://wholestay.example/approve/abc123'
    }
    types = @{
        'twilio/text' = @{ body = $body }
    }
} | ConvertTo-Json -Depth 8

Write-Host "Creating Content template '$FriendlyName' ($Language)..." -ForegroundColor Cyan
$content = Invoke-RestMethod -Method Post -Uri 'https://content.twilio.com/v1/Content' `
    -Authentication Basic -Credential $cred `
    -ContentType 'application/json' -Body $payload

$contentSid = $content.sid
Write-Host ""
Write-Host "Content SID: $contentSid" -ForegroundColor Green
Write-Host "  -> paste into Settings (Twilio 'Template SID') or app.config Twilio.ContentSid"
Write-Host ""

if ($SubmitApproval) {
    # The approval 'name' must be lowercase letters/digits/underscores.
    $approvalName = ($FriendlyName.ToLowerInvariant() -replace '[^a-z0-9_]', '_')
    $approvalBody = @{ name = $approvalName; category = $Category } | ConvertTo-Json
    Write-Host "Submitting '$approvalName' for WhatsApp approval (category $Category)..." -ForegroundColor Cyan
    $approval = Invoke-RestMethod -Method Post `
        -Uri "https://content.twilio.com/v1/Content/$contentSid/ApprovalRequests/whatsapp" `
        -Authentication Basic -Credential $cred `
        -ContentType 'application/json' -Body $approvalBody
    Write-Host "Approval submitted. Status:" -ForegroundColor Green
    $approval | ConvertTo-Json -Depth 6
    Write-Host ""
    Write-Host "Check status later with:" -ForegroundColor DarkGray
    Write-Host "  twilio api:content:v1:content-and-approvals:list -o json | ConvertFrom-Json | Where-Object sid -eq $contentSid"
}
