#pragma once

#include <unknwn.h>

const GUID IID_IMCPlayerSetUri = { 0x58864c93, 0x45f9, 0x4c6d, { 0xaa, 0x3f, 0x80, 0xf6, 0xca, 0xa0, 0x82, 0x81 } };

private struct IMCPlayer : public IUnknown
{
    virtual int Initialize(HWND*, unsigned int, IMCPlayerEvents*) = 0;
};

private struct IMCPlayerSetUri : public IUnknown
{
public:
    // TODO: Is this the same method as IMCTransport.SetTransportEvents?
    virtual int SetPlayerSetUriEvents(IMCPlayerSetUriEvents* events) = 0;
};

private struct IMCPlayerEvents
{
};

private struct IMCPlayerSetUriEvents
{
};
