#!/bin/bash
set -e

cd /workspaces/Intento10

echo "=== Preparando v2 para release ==="

# Crear zip ligero solo con el ejecutable para no exceder limites de GitHub
if [ -f "bin/Release/publish-win-x86/LTDHelper.exe" ]; then
    rm -f LTDHelper-win-x86-v2.zip
    zip -j LTDHelper-win-x86-v2.zip bin/Release/publish-win-x86/LTDHelper.exe >/dev/null
    echo "✓ Creado LTDHelper-win-x86-v2.zip"
fi

# Renombrar x64 también si existe
if [ -f "LTDHelper-win-x64.zip" ]; then
    mv LTDHelper-win-x64.zip LTDHelper-win-x64-v2.zip
    echo "✓ Renombrado a LTDHelper-win-x64-v2.zip"
fi

echo ""
echo "=== Agregando archivos a git ==="
git add .gitignore LTDHelper1.csproj .vscode/tasks.json deploy-v2.sh version-release.sh LTDHelper-win-x86-v2.zip LTDHelper-win-x64-v2.zip LTDHelper/MainWindow.cs LTDHelper/AppTranslator.cs RELEASES.md 2>/dev/null || git add .gitignore LTDHelper1.csproj .vscode/tasks.json deploy-v2.sh version-release.sh LTDHelper-win-x86-v2.zip LTDHelper/MainWindow.cs LTDHelper/AppTranslator.cs RELEASES.md

echo "✓ Archivos agregados"

echo ""
echo "=== Estado actual ==="
git status

echo ""
echo "=== Haciendo commit ==="
git commit -m "v2: Add BuyMarketplaceOffer fallback, /debug command, and improved diagnostics

Features:
- Dual-format BuyMarketplaceOffer fallback (attempts with/without price)
- Result code interpretation (1=success, else=rejection)
- /debug on|off command to toggle diagnostic output
- Conditional diagnostics based on DebugEnabled flag
- Multi-language support (EN/ES/PT)
- Ignore generated build outputs and create lightweight release zip
- RELEASES.md documentation

Executable: LTDHelper-win-x86-v2.zip
Ready for testing with G-Earth extension"

echo "✓ Commit creado"

echo ""
echo "=== Haciendo push a main ==="
git push origin main

echo "✓ Push completado"
echo ""
echo "✅ v2 está en vivo! 🚀"
