#!/bin/bash
set -e

cd /workspaces/Intento10

echo "=== Preparando v2 para release ==="

# Renombrar zip a v2
if [ -f "LTDHelper-win-x86-full.zip" ]; then
    mv LTDHelper-win-x86-full.zip LTDHelper-win-x86-v2.zip
    echo "✓ Renombrado a LTDHelper-win-x86-v2.zip"
fi

# Renombrar x64 también si existe
if [ -f "LTDHelper-win-x64.zip" ]; then
    mv LTDHelper-win-x64.zip LTDHelper-win-x64-v2.zip
    echo "✓ Renombrado a LTDHelper-win-x64-v2.zip"
fi

echo ""
echo "=== Agregando archivos a git ==="
git add LTDHelper-win-x86-v2.zip LTDHelper-win-x64-v2.zip LTDHelper/MainWindow.cs LTDHelper/AppTranslator.cs RELEASES.md 2>/dev/null || git add LTDHelper-win-x86-v2.zip LTDHelper/MainWindow.cs LTDHelper/AppTranslator.cs RELEASES.md

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
- RELEASES.md documentation

Executable: LTDHelper-win-x86-v2.zip (118MB)
Ready for testing with G-Earth extension"

echo "✓ Commit creado"

echo ""
echo "=== Haciendo push a main ==="
git push origin main

echo "✓ Push completado"
echo ""
echo "✅ v2 está en vivo! 🚀"
