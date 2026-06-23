@echo off
set VERSION=2.2.1
echo RogerHubEngine app builder
echo === build .NET ===

dotnet publish Yocto_Roger.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
dotnet publish Yocto_Roger.csproj -c Release -r win-arm64 --self-contained true -p:PublishSingleFile=true
dotnet publish Yocto_Roger.csproj -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true
dotnet publish Yocto_Roger.csproj -c Release -r linux-arm64 --self-contained true -p:PublishSingleFile=true

echo === build VPK ===

vpk pack --runtime win-x64 --packId Yocto_Roger-win-x64 --packVersion %VERSION% --packDir bin\Release\net9.0\win-x64\publish --mainExe Yocto_Roger.exe

vpk pack --runtime win-arm64 --packId Yocto_Roger-win-arm64 --packVersion %VERSION% --packDir bin\Release\net9.0\win-arm64\publish --mainExe Yocto_Roger.exe

echo === build linux zip ===

powershell Compress-Archive -Force bin\Release\net9.0\linux-x64\publish Releases\Yocto_Roger-linux-x64-%VERSION%.zip
powershell Compress-Archive -Force bin\Release\net9.0\linux-arm64\publish Releases\Yocto_Roger-linux-arm64-%VERSION%.zip

echo done.
pause