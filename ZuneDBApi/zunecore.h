#pragma once
#include "IMCPlayer.h"

#define WIN32_LEAN_AND_MEAN
#include "windows.h"

typedef int(_cdecl WmpCoreInitialize_t)(void);
static WmpCoreInitialize_t* WmpCoreInitialize;

typedef int(_cdecl WmpCoreDeinitialize_t)(void);
static WmpCoreDeinitialize_t* WmpCoreDeinitialize;

typedef int (_cdecl CWmpPlayer_GetInstance_t)(IMCPlayer**);
static CWmpPlayer_GetInstance_t* CWmpPlayer_GetInstance;

static HMODULE g_ZuneCore;

bool Initialize_ZuneCore();
void Cleanup_ZuneCore();
