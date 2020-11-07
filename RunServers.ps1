# Run PowerShell with administrator permissions
# Execute: Set-ExecutionPolicy RemoteSigned



Set-Location .\DBLPWrapper

start dotnet run


Set-Location ..\GoogleScholarWrapper

start dotnet run


Set-Location ..\IEEEXploreWrapper

start dotnet run


Set-Location ..\BibliographicalSourcesIntegratorWarehouse

start dotnet run



Set-Location ..