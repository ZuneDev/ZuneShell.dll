#pragma once

#include <Unknwn.h>

const GUID IID_IMCDynamicImage = { 0x102e281e, 0x28ad, 0x4688, { 0xaa, 0xff, 0xf5, 0x60, 0xf8, 0x05, 0x3d, 0x90 } };

private struct IMCDynamicImage : public IUnknown
{
public:
    STDMETHOD(Initialize(int));
};

