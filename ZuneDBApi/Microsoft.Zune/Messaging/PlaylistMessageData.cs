using System;
using System.Collections;

namespace Microsoft.Zune.Messaging;

// Original builds a native WMP property-set tree (IMSMediaSchemaPropertySet /
// IMSMediaSchemaPropertyList) describing a playlist and its tracks, via
// ZuneLibraryExports.CreatePropertySet/CreatePropertySetList/CreateTrackPropSet — none
// reverse engineered. See logs/Microsoft.Zune/Messaging/PlaylistMessageData.md.
public class PlaylistMessageData : IPropertySetMessageData
{
    private string m_title;
    private string m_author;
    private IList m_tracks;

    public PlaylistMessageData(string title, string author, IList tracks)
    {
        m_title = title;
        m_author = author;
        m_tracks = tracks;
    }

    public int GetPropertySet(out IntPtr ppPropSet)
    {
        ppPropSet = IntPtr.Zero;
        return unchecked((int)0x80004005); // E_FAIL
    }
}
