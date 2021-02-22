// Decompiled with JetBrains decompiler
// Type: ZuneUI.ApplicationDetails
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using MicrosoftZuneLibrary;

namespace ZuneUI
{
    public class ApplicationDetails
    {
        private static int[] ColumnIndices = new int[8]
        {
      344,
      181,
      317,
      175,
      176,
      68,
      24,
      376
        };
        private static string[] DataProperties = new string[8]
        {
      "Title",
      "FolderName",
      "FilePath",
      "FileName",
      "FileSize",
      "Copyright",
      "Author",
      "Version"
        };
        private static object[] DefaultFieldValues = new object[8]
        {
       string.Empty,
       string.Empty,
       string.Empty,
       string.Empty,
       0L,
       string.Empty,
       string.Empty,
       string.Empty
        };

        public static void Populate(object dataContainer, int libraryId)
        {
            DataProviderObject dataProviderObject = (DataProviderObject)dataContainer;
            object[] fieldValues = (object[])DefaultFieldValues.Clone();
            bool[] isEmptyValues = new bool[fieldValues.Length];
            ZuneLibrary.GetFieldValues(libraryId, EListType.eAppList, ColumnIndices.Length, ColumnIndices, fieldValues, isEmptyValues, PlaylistManager.Instance.QueryContext);
            for (int index = 0; index < ColumnIndices.Length; ++index)
                dataProviderObject.SetProperty(DataProperties[index], fieldValues[index]);
        }
    }
}
