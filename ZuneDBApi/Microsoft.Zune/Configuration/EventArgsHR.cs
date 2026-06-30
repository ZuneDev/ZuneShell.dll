using System;

namespace Microsoft.Zune.Configuration
{
    public class EventArgsHR : EventArgs
    {
        private int hResult;

        public int HResult
        {
            get { return hResult; }
            set { hResult = value; }
        }

        public EventArgsHR()
        {
            hResult = 0;
        }
    }
}