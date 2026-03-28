#!/bin/bash
cd /workspaces/Intento10

# Rename zip to v2
if [ -f "LTDHelper-win-x86-full.zip" ]; then
    mv LTDHelper-win-x86-full.zip LTDHelper-win-x86-v2.zip
    echo "✓ Renamed to LTDHelper-win-x86-v2.zip"
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
git add LTDHelper-win-x86-v2.zip LTDHelper-win-x64-v2.zip LTDHelper/MainWindow.cs LTDHelper/AppTranslator.cs

# Show status
echo ""
echo "Git status:"
git status
