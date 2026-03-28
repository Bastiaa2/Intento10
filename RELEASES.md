# LTDHelper Releases

## v3 (Actual)
**Release Date**: March 28, 2026

### Features
- ✅ Marketplace parser updated (no strict status filter)
- ✅ Better diagnostics when offer parsing fails (`/debug on`)
- ✅ Keeps best valid offer by `offerId` + `price <= max`
- ✅ Build artifact cleanup policy (`.gitignore`)

### Testing Focus
1. Enable `/debug on`
2. Start flow with `/iniciar`
3. Verify: with max price `10`, an offer at `9` is detected and attempted
4. If parsing fails, check debug chat for `Parse offers failed`

### Notes
- Legacy release ZIP artifacts were removed from repository tracking.
- Recommended distribution remains via release assets, not repository binaries.

---

## v2
**Release Date**: March 28, 2026

### Features
- ✅ Marketplace auto-buyer (replaces old catalog buyer)
- ✅ Dual-format BuyMarketplaceOffer fallback
- ✅ Result code interpretation
- ✅ `/debug on|off` command for diagnostics
- ✅ Multi-language support (EN, ES, PT)
- ✅ Configuration wizard
- ✅ Balance checking

### Download
- `LTDHelper-win-x86-v2.zip` - 32-bit executable
- `LTDHelper-win-x64-v2.zip` - 64-bit executable (pending)

### Key Changes from v1
1. **Fallback Logic**: TryBuyOfferAsync() attempts 2 packet formats
2. **Diagnostics**: User-controllable via `/debug on|off`
3. **Language Strings**: Added PurchaseRejected, NoOfferAtPrice, DebugEnabled/Disabled
4. **Code Quality**: Improved error handling and async/await patterns

### Testing Instructions
1. Extract zip in Windows
2. Load in G-Earth extension
3. Type `/debug on` for diagnostics
4. Type `/iniciar` and follow wizard
5. Observe: if purchase fails, error code will show in game chat

### Known Issues
- None yet (awaiting user testing feedback)

### Next in v3
- Will be determined based on user testing results
- Potential: additional packet format support, timeout adjustments, etc.
