using System;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.QuickMix;

public class QuickMixItem
{
    private int m_mediaId;
    private int m_durationSeconds;
    private bool m_inCollection;
    private string m_title;
    private string m_artistName;
    private Guid m_serviceMediaId;

    public bool InCollection
    {
        [return: MarshalAs(UnmanagedType.U1)]
        get { return m_inCollection; }
    }

    public int DurationSeconds => m_durationSeconds;

    public Guid ServiceMediaId => m_serviceMediaId;

    public string ArtistName => m_artistName;

    public string Title => m_title;

    public int MediaId => m_mediaId;

    public QuickMixItem(int mediaId, string title, string artistName, int durationSeconds, [MarshalAs(UnmanagedType.U1)] bool inCollection, Guid serviceMediaId)
    {
        m_mediaId = mediaId;
        m_title = title;
        m_artistName = artistName;
        m_durationSeconds = durationSeconds;
        m_inCollection = inCollection;
        m_serviceMediaId = serviceMediaId;
    }

    public override string ToString()
    {
        return $"Title:{m_title}\n\tArtist:{m_artistName}\n\tServiceMediaId:{m_serviceMediaId}\n\tMedia Id:{m_mediaId}\tInCollection:{m_inCollection}";
    }
}
