#pragma once

#include <objbase.h>

const GUID IID_IMCTransport = { 0x2f33a725, 0x95cb, 0x4080, { 0xad, 0xef, 0x93, 0xa0, 0x67, 0xa7, 0x07, 0xba } };

private class IMCTransport : public IUnknown
{
public:
    // 24
    STDMETHOD(SetTransportEvents(IMCTransportEvents* events));

    // 96
    STDMETHOD(SetPositionEventInterval(unsigned int intervalMilliseconds));
};

private struct IMCTransportEvents
{
};

