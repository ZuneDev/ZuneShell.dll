// Decompiled with JetBrains decompiler
// Type: ZuneUI.PhotoDetails
// Assembly: ZuneShell, Version=4.7.0.0, Culture=neutral, PublicKeyToken=ddd0da4d3e678217
// MVID: FC8028F3-A47B-4FB4-B35B-11D1752D8264
// Assembly location: C:\Program Files\Zune\ZuneShell.dll

using Microsoft.Iris;
using MicrosoftZuneLibrary;

namespace ZuneUI
{
    public class PhotoDetails
    {
        private static int[] ColumnIndexes = new int[8]
        {
      344,
      177,
      317,
      181,
      175,
       byte.MaxValue,
      254,
      68
        };
        private static string[] DataProperties = new string[8]
        {
      "Title",
      "MediaType",
      "ImagePath",
      "FolderName",
      "FileName",
      "Width",
      "Height",
      "Copyright"
        };

        public static void Populate(object dataContainer, int libraryId)
        {
            DataProviderObject dataProviderObject = (DataProviderObject)dataContainer;
            object[] fieldValues = new object[8]
            {
         string.Empty,
         0,
         string.Empty,
         string.Empty,
         string.Empty,
         0,
         0,
         string.Empty
            };
            ZuneLibrary.GetFieldValues(libraryId, EListType.ePhotoList, ColumnIndexes.Length, ColumnIndexes, fieldValues, PlaylistManager.Instance.QueryContext);
            for (int index = 0; index < ColumnIndexes.Length; ++index)
            {
                if (ColumnIndexes[index] == 177)
                    fieldValues[index] = MediaDescriptions.Map((MediaType)fieldValues[index]);
                dataProviderObject.SetProperty(DataProperties[index], fieldValues[index]);
            }
        }
    }
}
