#!/bin/bash
cd /workspaces/Intento10

# Create lightweight x86 zip from the published exe
if [ -f "bin/Release/publish-win-x86/LTDHelper.exe" ]; then
    rm -f LTDHelper-win-x86-v2.zip
    zip -j LTDHelper-win-x86-v2.zip bin/Release/publish-win-x86/LTDHelper.exe >/dev/null
    echo "✓ Created LTDHelper-win-x86-v2.zip"
fi

# Check if old x64 zip exists and version it
if [ -f "LTDHelper-win-x64.zip" ]; then
    mv LTDHelper-win-x64.zip LTDHelper-win-x64-v2.zip
    echo "✓ Renamed to LTDHelper-win-x64-v2.zip"
fi

# Show final files
echo ""
echo "Current release files:"
ls -lh LTDHelper-win-x86-v2.zip LTDHelper-win-x64-v2.zip 2>/dev/null || ls -lh LTDHelper-win-x86-v2.zip

# Add to git
git add .gitignore LTDHelper1.csproj .vscode/tasks.json deploy-v2.sh version-release.sh LTDHelper-win-x86-v2.zip LTDHelper-win-x64-v2.zip LTDHelper/MainWindow.cs LTDHelper/AppTranslator.cs RELEASES.md

# Show status
echo ""
echo "Git status:"
git status
