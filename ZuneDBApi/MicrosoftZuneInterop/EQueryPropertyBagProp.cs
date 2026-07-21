// Declared in the global namespace to match the original ZuneDBApi.dll metadata
// (ILSpy reports this type with an empty namespace). Physically placed under
// MicrosoftZuneInterop/ since IQueryPropertyBag and QueryPropertyBag are its only
// consumers.
//
// TODO: this native enum carries no visible members in the managed metadata
// (get_type_members on ZuneDBApi.dll returns only the implicit `value__` field),
// so its named constants cannot be recovered via ILSpy. See
// logs/MicrosoftZuneInterop/QueryPropertyBag.md (Unknown Q4) — resolving this
// requires Ghidra analysis of the native kPropIdMap table, which embeds the enum
// values directly next to their name strings.
public enum EQueryPropertyBagProp
{
}
