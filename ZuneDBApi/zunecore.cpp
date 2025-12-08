#include "pch.h"
#include "zunecore.h"

#define WIN32_LEAN_AND_MEAN
#include "windows.h"

#define stringify_literal( x ) # x

#define typeof_func(funcName) funcName ## _t
#define TryGetProcAddress_ZuneCore(funcName) funcName = (typeof_func(funcName)*)GetProcAddress(g_ZuneCore, stringify_literal(funcName)); \
    if (!funcName) { return false; }

using namespace System;

bool Initialize_ZuneCore()
{
    if (g_ZuneCore)
    {
        return true;
    }

    g_ZuneCore = LoadLibraryA("zunecore.dll");
    if (!g_ZuneCore)
    {
        Console::WriteLine("Failed to load zunecore.dll");
        return false;
    }

    TryGetProcAddress_ZuneCore(WmpCoreInitialize);
    TryGetProcAddress_ZuneCore(WmpCoreDeinitialize);
    TryGetProcAddress_ZuneCore(CWmpPlayer_GetInstance);

    return true;
}

void Cleanup_ZuneCore()
{
    if (g_ZuneCore)
    {
        FreeLibrary(g_ZuneCore);

        g_ZuneCore = nullptr;

        WmpCoreInitialize = nullptr;
        WmpCoreDeinitialize = nullptr;
        CWmpPlayer_GetInstance = nullptr;
    }
}
