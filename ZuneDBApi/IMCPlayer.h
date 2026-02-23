#pragma once

#include <unknwn.h>

const GUID IID_IMCPlayerSetUri = { 0x58864c93, 0x45f9, 0x4c6d, { 0xaa, 0x3f, 0x80, 0xf6, 0xca, 0xa0, 0x82, 0x81 } };

struct IMCPlayer : public IUnknown
{
public:
    STDMETHOD(Initialize(HWND hWnd, unsigned int p1, IMCPlayerEvents* pEvents));

    STDMETHOD(Method32(void));
    STDMETHOD(Method40(void));
    STDMETHOD(Method64(void));
};

struct IMCPlayerSetUri : public IUnknown
{
public:
    // TODO: Is this the same method as IMCTransport.SetTransportEvents?
    STDMETHOD(SetPlayerSetUriEvents(IMCPlayerSetUriEvents* events));
};

struct IMCPlayerEvents
{
};

struct IMCPlayerSetUriEvents
{
};
