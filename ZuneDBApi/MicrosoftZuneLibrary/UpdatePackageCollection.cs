using System;
using System.Collections.Generic;

namespace MicrosoftZuneLibrary;

// Original wraps a native IFirmwareUpdateCollection*. Not reverse engineered — see
// logs/MicrosoftZuneLibrary/FirmwareUpdater.md.
//
// The original exposes two *named* indexed properties — the default "Item" (get-only)
// and a separate "Selected" (get/set) — which is legal IL but has no C# indexer syntax
// (C# only supports one default indexer per type). The original decompiled ZuneShell
// call sites (UIFirmwareUpdater.cs) call them as plain methods (`get_Item(index)`,
// `set_Selected(index, value)`), which is exactly how C# requires named indexers to be
// invoked — so they're declared here as ordinary methods with those exact names
// instead of a C# `this[int]` indexer.
public class UpdatePackageCollection : IDisposable
{
    private List<FirmwareUpdatePackage> m_packages = new();

    public FirmwareUpdatePackage GamesPackage => null;

    public FirmwareUpdatePackage FirmwarePackage => null;

    public int Count => m_packages.Count;

    public FirmwareUpdatePackage get_Item(int index) => m_packages[index];

    public void set_Selected(int index, bool value)
    {
        m_packages[index].Selected = value;
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
