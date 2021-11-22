using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Iris;

namespace MicrosoftZuneLibrary
{
	public class FriendsOfArtistQuery : ModelItem, IDisposable
	{
		private Guid m_artistId;

		private bool m_getFriends;

		private int m_userId;

		private bool m_resolveZuneTags;

		private bool m_hasFriends = false;

		private bool m_queryComplete = false;

		private IList m_friends = null;

		private IList m_friendsWithFavs = null;

		private IList m_friendsWithMostPlayedSongs = null;

		private IList m_friendsWithMostPlayedArtists = null;

		private static FriendsOfArtistQuery m_singletonInstance = null;

		public IList FriendsWithMostPlayedArtists => m_friendsWithMostPlayedArtists;

		public IList FriendsWithMostPlayedSongs => m_friendsWithMostPlayedSongs;

		public IList FriendsWithFavs => m_friendsWithFavs;

		public IList Friends => m_friends;

		public bool HasFriends
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return m_hasFriends;
			}
		}

		public bool QueryComplete
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return m_queryComplete;
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				if (value != m_queryComplete)
				{
					m_queryComplete = value;
					FirePropertyChanged("QueryComplete");
				}
			}
		}

		public static FriendsOfArtistQuery Instance
		{
			get
			{
				if (m_singletonInstance == null)
				{
					m_singletonInstance = new FriendsOfArtistQuery();
				}
				return m_singletonInstance;
			}
		}

		public void Query(Guid artistId, [MarshalAs(UnmanagedType.U1)] bool getFriends, int userId)
		{
			Query(artistId, getFriends, userId, resolveZuneTags: true);
		}

		public void Query(Guid artistId, [MarshalAs(UnmanagedType.U1)] bool getFriends, int userId, [MarshalAs(UnmanagedType.U1)] bool resolveZuneTags)
		{
			try
			{
				Monitor.Enter(m_singletonInstance);
				QueryComplete = false;
				if (!(artistId != m_artistId) && (!getFriends || m_getFriends) && userId == m_userId)
				{
					QueryComplete = true;
					return;
				}
				m_artistId = artistId;
				m_getFriends = getFriends;
				m_userId = userId;
				m_resolveZuneTags = resolveZuneTags;
				SetResults(hasFriends: false, null, null, null, null);
				if (artistId != Guid.Empty)
				{
					AsyncQueryParams state = new AsyncQueryParams(artistId, getFriends, userId, DeferredSetResults, m_resolveZuneTags);
					ThreadPool.QueueUserWorkItem(AsyncQuery, state);
				}
				else
				{
					QueryComplete = true;
				}
			}
			finally
			{
				Monitor.Exit(m_singletonInstance);
			}
		}

		private FriendsOfArtistQuery()
		{
		}

		private void _007EFriendsOfArtistQuery()
		{
		}

		private unsafe static void AsyncQuery(object obj)
		{
			//IL_000d: Expected I, but got I8
			//IL_0011: Expected I, but got I8
			//IL_00d2: Expected I, but got I8
			//IL_00f7: Expected I, but got I8
			AsyncQueryParams asyncQueryParams = (AsyncQueryParams)obj;
			int num = 0;
			int* ptr = null;
			int* ptr2 = null;
			int num2 = ((!asyncQueryParams.GetFriends) ? 1 : 100);
			if (Module.UserCardsForMedia(asyncQueryParams.ArtistId, EMediaTypes.eMediaTypePersonArtist, asyncQueryParams.UserId, 14, num2, &num, &ptr, &ptr2) >= 0)
			{
				bool flag = num > 0;
				IList list = null;
				IList list2 = null;
				IList list3 = null;
				IList list4 = null;
				if (asyncQueryParams.GetFriends && flag)
				{
					list = new ArrayList(num);
					list2 = new ArrayList(num);
					list3 = new ArrayList(num);
					list4 = new ArrayList(num);
					int num3 = 0;
					if (0 < num)
					{
						long num4 = 0L;
						do
						{
							FriendOfArtist friendOfArtist = new FriendOfArtist();
							friendOfArtist.Id = *(int*)(num4 + (nint)ptr);
							if (asyncQueryParams.ResolveZuneTags)
							{
								_0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_0040 _0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_0040;
								Module.DBPropertyRequestStruct_002E_007Bctor_007D((DBPropertyRequestStruct*)(&_0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_0040), 187u);
								try
								{
									if (Module.GetFieldValues(*(int*)(num4 + (nint)ptr), EListType.eUserCardList, 1, (DBPropertyRequestStruct*)(&_0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_0040), null) >= 0 && Unsafe.As<_0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_0040, int>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_0040, 4)) >= 0 && Unsafe.As<_0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_0040, ushort>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_0040, 8)) == 8)
									{
										friendOfArtist.ZuneTag = new string((char*)Unsafe.As<_0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_0040, ulong>(ref Unsafe.AddByteOffset(ref _0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_0040, 16)));
									}
								}
								catch
								{
									//try-fault
									Module.___CxxCallUnwindVecDtor((delegate*<void*, ulong, int, delegate*<void*, void>, void>)(&Module.__ehvec_dtor), &_0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_0040, 32uL, 1, (delegate*<void*, void>)(delegate*<DBPropertyRequestStruct*, void>)(&Module.DBPropertyRequestStruct_002E_007Bdtor_007D));
									throw;
								}
								Module.__ehvec_dtor(&_0024ArrayType_0024_0024_0024BY00UDBPropertyRequestStruct_0040_0040, 32uL, 1, (delegate*<void*, void>)(delegate*<DBPropertyRequestStruct*, void>)(&Module.DBPropertyRequestStruct_002E_007Bdtor_007D));
							}
							list.Add(friendOfArtist);
							if (((uint)(*(int*)(num4 + (nint)ptr2)) & 2u) != 0)
							{
								list2.Add(friendOfArtist);
							}
							if (((uint)(*(int*)(num4 + (nint)ptr2)) & 4u) != 0)
							{
								list3.Add(friendOfArtist);
							}
							if (((uint)(*(int*)(num4 + (nint)ptr2)) & 8u) != 0)
							{
								list4.Add(friendOfArtist);
							}
							num3++;
							num4 += 4;
						}
						while (num3 < num);
					}
				}
				SetResultsParams args = new SetResultsParams(flag, list, list2, list3, list4);
				Application.DeferredInvoke(asyncQueryParams.SetResultsHandler, args);
			}
			if (ptr != null)
			{
				Module.delete_005B_005D(ptr);
			}
			if (ptr2 != null)
			{
				Module.delete_005B_005D(ptr2);
			}
		}

		private void SetResults([MarshalAs(UnmanagedType.U1)] bool hasFriends, IList friends, IList friendsWithFavs, IList friendsWithMostPlayedSongs, IList friendsWithMostPlayedArtists)
		{
			if (m_friends != friends)
			{
				m_friends = friends;
				FirePropertyChanged("Friends");
			}
			if (m_friendsWithFavs != friendsWithFavs)
			{
				m_friendsWithFavs = friendsWithFavs;
				FirePropertyChanged("FriendsWithFavs");
			}
			if (m_friendsWithMostPlayedSongs != friendsWithMostPlayedSongs)
			{
				m_friendsWithMostPlayedSongs = friendsWithMostPlayedSongs;
				FirePropertyChanged("FriendsWithMostPlayedSongs");
			}
			if (m_friendsWithMostPlayedArtists != friendsWithMostPlayedArtists)
			{
				m_friendsWithMostPlayedArtists = friendsWithMostPlayedArtists;
				FirePropertyChanged("FriendsWithMostPlayedArtists");
			}
			if (m_hasFriends != hasFriends)
			{
				m_hasFriends = hasFriends;
				FirePropertyChanged("HasFriends");
			}
		}

		private void DeferredSetResults(object obj)
		{
			QueryComplete = true;
			SetResultsParams setResultsParams = (SetResultsParams)obj;
			SetResults(setResultsParams.HasFriends, setResultsParams.Friends, setResultsParams.FriendsWithFavs, setResultsParams.FriendsWithMostPlayedSongs, setResultsParams.FriendsWithMostPlayedArtists);
		}

		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool P_0)
		{
			if (P_0)
			{
				try
				{
				}
				finally
				{
					base.Dispose();
				}
			}
		}

		public new void Dispose()
		{
			Dispose(true);
		}
	}
}
