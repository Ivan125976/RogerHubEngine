@echo off

echo RogerHubEngine app builder
echo === build .NET ===

dotnet restore

dotnet publish Yocto_Roger.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
dotnet publish Yocto_Roger.csproj -c Release -r win-arm64 --self-contained true -p:PublishSingleFile=true
dotnet publish Yocto_Roger.csproj -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true
dotnet publish Yocto_Roger.csproj -c Release -r linux-arm64 --self-contained true -p:PublishSingleFile=true
dotnet publish Yocto_Roger.csproj -c Release -r osx-x64 --self-contained true -p:PublishSingleFile=true

echo done.
pause