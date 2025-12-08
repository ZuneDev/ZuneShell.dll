#pragma once

#include <unknwn.h>

const GUID IID_IMCPlayerSetUri = { 0x58864c93, 0x45f9, 0x4c6d, { 0xaa, 0x3f, 0x80, 0xf6, 0xca, 0xa0, 0x82, 0x81 } };

struct IMCPlayer : public IUnknown
{
public:
    virtual int Initialize(HWND* pHWND, unsigned int p1, IMCPlayerEvents* pEvents) = 0;
};

struct IMCPlayerSetUri : public IUnknown
{
public:
    // TODO: Is this the same method as IMCTransport.SetTransportEvents?
    virtual int SetPlayerSetUriEvents(IMCPlayerSetUriEvents* events) = 0;
};

struct IMCPlayerEvents
{
};

struct IMCPlayerSetUriEvents
{
};
