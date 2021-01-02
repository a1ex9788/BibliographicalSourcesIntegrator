# Run PowerShell with administrator permissions
# Execute: Set-ExecutionPolicy RemoteSigned


function RunServe([string] $Location)
{
    Set-Location $Location

    start dotnet run
}

function Wait1Sec
{
    Start-Sleep -s 1
}


RunServe ".\BibliographicalSourcesIntegratorWarehouse"

Wait1Sec

RunServe "..\DBLPWrapper"

Wait1Sec

RunServe "..\GoogleScholarWrapper"

Wait1Sec

RunServe "..\IEEEXploreWrapper"