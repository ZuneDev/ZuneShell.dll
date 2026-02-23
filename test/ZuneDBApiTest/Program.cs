using MicrosoftZunePlayback;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZuneDBApiTest;

public static class Program
{
    public static int Main(string[] args)
    {
        var player = PlayerInterop.Instance;

        player.Initialize();

        player.PositionEventInterval = TimeSpan.FromMilliseconds(1337);

        player.Uninitialize();

        return 0;
    }
}
