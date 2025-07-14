using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace ZuneDBApi.Interop;

internal static class ZuneLibraryExports
{
    private const string ZUNENATIVELIB_DLL = "ZuneNativeLib.dll";

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public static extern int AddDeviceSyncRule(EDeviceSyncRuleType P_0, [MarshalAs(UnmanagedType.U1)] bool P_1, int P_2, EMediaTypes P_3, int P_4);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public static extern int AddDeviceSyncRuleWithValue(EDeviceSyncRuleType P_0, int P_1, EMediaTypes P_2, int P_3, int P_4);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int AddGrovelerScanDirectory(ushort* P_0, EMediaTypes P_1);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int AddItemToPlaylist(int P_0, int P_1, IPlaylist* P_2);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int AddMedia(IMSMediaSchemaPropertySet* P_0, EMediaTypes P_1, int* P_2);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int AddMedia(ushort* P_0, EMediaTypes P_1, uint P_2, bool* P_3, int* P_4);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int CanAddMedia(ushort* P_0, EMediaTypes P_1);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int AddTransientMedia(ushort* P_0, EMediaTypes P_1, int* P_2);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int CanAddFromFolder(ushort* P_0);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public static extern int CleanupTransientMedia();

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int CompareWithoutArticles(ushort* P_0, ushort* P_1, int* P_2);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int CopyThumbnailBitmapData(HBITMAP__* P_0, int P_1, int P_2, int P_3, int P_4, int P_5, int P_6, HBITMAP__** P_7, void** P_8);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int CopyThumbnailBitmapData(HBITMAP__* P_0, HBITMAP__** P_1, void** P_2);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int CreateAlbumPropSet(_GUID P_0, ushort* P_1, ushort* P_2, IMSMediaSchemaPropertySet** P_3);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int CreateContentRefreshTask(IAsyncCallback* P_0, IContentRefreshTask** P_1);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int CreateDRMQuery(IDRMQuery** P_0);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int CreateDataObjectEnum(IDataObjectEnumerator** P_0);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int CreateEmptyPlaylist(IPlaylist** P_0);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int CreateMultiSortAttributes(int P_0, IMultiSortAttributes** P_1);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int CreateNativeFileAssociationHandler(void** P_0);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int CreateNativeSubscriptionViewer(void** P_0);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int CreatePropertyBag(IQueryPropertyBag** P_0);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int CreatePropertySet(_GUID* P_0, uint P_1, IMSMediaSchemaPropertySet** P_2);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int CreatePropertySetList(_GUID* P_0, uint P_1, IMSMediaSchemaPropertyList** P_2);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int CreateTrackPropSet(_GUID P_0, _GUID P_1, int P_2, ushort* P_3, int P_4, ushort* P_5, ushort* P_6, ushort* P_7, IMSMediaSchemaPropertySet** P_8);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int CreateVideoPropSet(_GUID P_0, ushort* P_1, int P_2, IMSMediaSchemaPropertySet** P_3);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int DeleteDeviceSyncRules(EDeviceSyncRuleType P_0, int P_1, EMediaTypes P_2, int* P_3, int P_4, [MarshalAs(UnmanagedType.U1)] bool P_5);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public static extern int DeleteFSFolder(int P_0, EMediaTypes P_1);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int DeleteMedia(EMediaTypes P_0, int* P_1, int P_2, int P_3, int P_4);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int DeleteRootFolder(ushort* P_0, EMediaTypes P_1);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int DestroyDataObjectEnum(IDataObjectEnumerator* P_0);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int DoesFileExist(ushort* P_0, int* P_1);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public static extern int ExportUserRatings(int P_0, EMediaTypes P_1);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int GetAlbumMetadata(int P_0, [MarshalAs(UnmanagedType.U1)] bool P_1, IAlbumInfo** P_2);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int GetAlbumMetadataForAlbumId(long P_0, int P_1, IAlbumInfo* P_2, IWMISGetAlbumForAlbumIdCallback* P_3);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int GetCDDeviceList(IWMPCDDeviceList** P_0);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int GetFieldValues(int P_0, EListType P_1, int P_2, DBPropertyRequestStruct* P_3, IQueryPropertyBag* P_4);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int GetKnownFolders(DynamicArray_003Cunsigned_0020short_0020_002A_003E* P_0, DynamicArray_003Cunsigned_0020short_0020_002A_003E* P_1, DynamicArray_003Cunsigned_0020short_0020_002A_003E* P_2, DynamicArray_003Cunsigned_0020short_0020_002A_003E* P_3, DynamicArray_003Cunsigned_0020short_0020_002A_003E* P_4, ushort** P_5, ushort** P_6, ushort** P_7, ushort** P_8, ushort** P_9);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern HINSTANCE__* GetLocResourceInstance();

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int GetLocalizedPathOfFolder(ushort* P_0, [MarshalAs(UnmanagedType.U1)] bool P_1, ushort** P_2);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int GetMappedErrorDescriptionAndUrl(int P_0, eErrorCondition P_1, int* P_2, ushort** P_3, ushort** P_4);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int GetSyncRuleForMedia(int P_0, EMediaTypes P_1, int P_2, EDeviceSyncRuleType* P_3);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int GetSyncRuleValueForMedia(EDeviceSyncRuleType P_0, int P_1, EMediaTypes P_2, int P_3, int* P_4);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int GetThumbnailBitmapData(ushort* P_0, int* P_1, int* P_2, void** P_3, HBITMAP__** P_4);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public static extern int ImportSharedRatingsForUser(int P_0, EMediaTypes P_1);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int InteropNotifyAdvise(IInteropNotify* P_0, ulong* P_1);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public static extern int InteropNotifyUnAdvise(ulong P_0);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int LocateArt(int P_0, EMediaTypes P_1, [MarshalAs(UnmanagedType.U1)] bool P_2, ushort** P_3);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public static extern int MarkAllDRMFilesAsNeedingLicenseRefresh();

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int MetadataChangeAdvise(IMetadataChangeNotify* P_0, ulong* P_1);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public static extern int MetadataChangeUnAdvise(ulong P_0);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public static extern int Phase2Initialization();

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int Phase3Initialization(IAsyncCallback* initCallback);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int QueryDatabase(EQueryType P_0, IQueryPropertyBag* P_1, IDatabaseQueryResults** P_2, ushort** P_3);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public static extern int ScanAndClearDeletedMedia();

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int SetAlbumArt(int P_0, HBITMAP__* P_1);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int SetAlbumArt(int P_0, ushort* P_1);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int SetFieldValues(int P_0, EListType P_1, int P_2, DBPropertySubmitStruct* P_3, IQueryPropertyBag* P_4);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern void ShipAssert(uint P_0, uint P_1, ushort* P_2);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public static extern int ShutdownZuneNativeLib();

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public static extern int SplitAudioTrack(int P_0);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int StartupZuneNativeLib(ushort* P_0, int* P_1);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public static extern int StopGroveler([MarshalAs(UnmanagedType.U1)] bool P_0);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int UpdateAlbumMetadata(int P_0, IAlbumInfo* P_1);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int UserCardsForMedia(_GUID P_0, EMediaTypes P_1, int P_2, int P_3, int P_4, int* P_5, int** P_6, int** P_7);

    [DllImport(ZUNENATIVELIB_DLL, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    [MethodImpl(MethodImplOptions.Unmanaged, MethodCodeType = MethodCodeType.Native)]
    [SuppressUnmanagedCodeSecurity]
    public unsafe static extern int ZunePropVariantChangeType(tagPROPVARIANT* P_0, tagPROPVARIANT* P_1, ushort P_2, ushort P_3);
}
