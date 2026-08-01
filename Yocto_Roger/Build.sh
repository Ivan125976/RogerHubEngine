#!/bin/bash

BOLD_GREEN='\e[1;32m'
BOLD_YELLOW='\e[1;33m'
RESET='\e[0m'

echo -e "${BOLD_GREEN}RogerHubEngine app builder${RESET}"
echo -e "=== ${BOLD_GREEN}build .NET${RESET} ==="

dotnet restore

dotnet publish Yocto_Roger.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
dotnet publish Yocto_Roger.csproj -c Release -r win-arm64 --self-contained true -p:PublishSingleFile=true
dotnet publish Yocto_Roger.csproj -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true
dotnet publish Yocto_Roger.csproj -c Release -r linux-arm64 --self-contained true -p:PublishSingleFile=true
dotnet publish Yocto_Roger.csproj -c Release -r osx-x64 --self-contained true -p:PublishSingleFile=true

echo "\e[5;92mdone."
pause