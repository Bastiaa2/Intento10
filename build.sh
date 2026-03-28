#!/bin/bash
cd /workspaces/Intento10
echo "Starting build..."
dotnet publish LTDHelper1.csproj -p:PublishProfile=Properties/PublishProfiles/LTDHelper-win-x86.pubxml -c Release
echo "Build completed"
echo "Creating zip..."
cd /workspaces/Intento10/bin/Release
zip -r ../../LTDHelper-win-x86-full.zip publish-win-x86/
echo "Zip created successfully"
ls -lh ../../LTDHelper-win-x86-full.zip
