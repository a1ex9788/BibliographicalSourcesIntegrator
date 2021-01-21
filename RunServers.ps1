# Run PowerShell with administrator permissions
# Execute: Set-ExecutionPolicy RemoteSigned


Set-Location .\Releases\DBLPWrapper
Start-Process .\DBLPWrapper.exe

Set-Location ..\GoogleScholarWrapper
Start-Process .\GoogleScholarWrapper.exe

Set-Location ..\IEEEXploreWrapper
Start-Process .\IEEEXploreWrapper.exe

Set-Location ..\BibliographicalSourcesIntegratorWarehouse
Start-Process .\BibliographicalSourcesIntegratorWarehouse.exe