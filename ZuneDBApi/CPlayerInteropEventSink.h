#pragma once

private struct CPlayerInteropEventSink : public IMCPlayerEvents, public IMCTransportEvents
{
private:
    void* _alignment1;

public:
    IMCTransportEvents* transportEvents;
    IMCPlayerSetUriEvents* playerSetUriEvents;
};
