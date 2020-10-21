# Run PowerShell with administrator permissions
# Execute: Set-ExecutionPolicy RemoteSigned


Set-Location .\BibliographicalSourcesIntegratorWarehouse

start dotnet run


Set-Location ..\DBLPExtractor

start dotnet run


Set-Location ..\GoogleScholarExtractor

start dotnet run


Set-Location ..\IEEEXploreExtractor

start dotnet run



Set-Location ..