using System;
using System.Runtime.InteropServices;

namespace Microsoft.Zune.Service;

public class Offer
{
	private Guid m_id;

	private string m_title;

	private string m_artist;

	private bool m_isMP3;

	private bool m_previouslyPurchased;

	private bool m_inCollection;

	private string m_serviceContext;

	private PriceInfo m_priceInfo;

	public PriceInfo PriceInfo => m_priceInfo;

	public string ServiceContext => m_serviceContext;

	public bool InCollection
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return m_inCollection;
		}
	}

	public bool PreviouslyPurchased
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return m_previouslyPurchased;
		}
	}

	public bool IsMP3
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return m_isMP3;
		}
	}

	public string Artist => m_artist;

	public string Title => m_title;

	public Guid Id => m_id;

	public Offer(Guid id, string title, string artist, PriceInfo priceInfo, [MarshalAs(UnmanagedType.U1)] bool isMP3, [MarshalAs(UnmanagedType.U1)] bool previouslyPurchased, [MarshalAs(UnmanagedType.U1)] bool inCollection, string serviceContext)
	{
		m_priceInfo = priceInfo;
		m_id = id;
		m_title = title;
		m_artist = artist;
		m_isMP3 = isMP3;
		m_previouslyPurchased = previouslyPurchased;
		m_inCollection = inCollection;
		m_serviceContext = serviceContext;
	}
}
