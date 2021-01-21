# Run PowerShell with administrator permissions
# Execute: Set-ExecutionPolicy RemoteSigned


Set-Location .\BibliographicalSourcesIntegratorWarehouse
dotnet publish -p:PublishProfile=FolderProfile -o:..\Releases\BibliographicalSourcesIntegratorWarehouse

Set-Location ..\DBLPWrapper
dotnet publish -p:PublishProfile=FolderProfile -o:..\Releases\DBLPWrapper

Set-Location ..\GoogleScholarWrapper
dotnet publish -p:PublishProfile=FolderProfile -o:..\Releases\GoogleScholarWrapper

Set-Location ..\IEEEXploreWrapper
dotnet publish -p:PublishProfile=FolderProfile -o:..\Releases\IEEEXploreWrapper