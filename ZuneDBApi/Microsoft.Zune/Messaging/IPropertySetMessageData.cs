using System;

namespace Microsoft.Zune.Messaging;

// Original: `int GetPropertySet(IMSMediaSchemaPropertySet** ppPropSet)` — a native
// Windows Media Player property-set COM interface, not reverse engineered — see
// logs/Microsoft.Zune/Messaging/PlaylistMessageData.md. Signature adapted to `out
// IntPtr` since the pointer-to-pointer native type has no managed representation here;
// no code in this solution calls GetPropertySet directly (PlaylistMessageData is only
// ever used as this interface's type, never invoked), so this simplification is
// internal-only.
public interface IPropertySetMessageData
{
    int GetPropertySet(out IntPtr ppPropSet);
}
