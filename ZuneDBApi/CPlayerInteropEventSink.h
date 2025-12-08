#pragma once

private struct CPlayerInteropEventSink
{
public:
    IMCPlayerEvents* playerEvents;
    IMCTransportEvents* transportEvents;
    IMCPlayerSetUriEvents* playerSetUriEvents;
};
