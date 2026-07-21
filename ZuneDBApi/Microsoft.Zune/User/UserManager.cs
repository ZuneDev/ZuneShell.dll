using System.Collections;

namespace Microsoft.Zune.User;

// Original wraps a native IUserManager* singleton. Not reverse engineered — see
// logs/Microsoft.Zune/User/UserManager.md.
public class UserManager
{
    private static UserManager sm_instance;

    public static UserManager Instance => sm_instance ??= new UserManager();

    private UserManager()
    {
    }

    public int GetUserIdList(IList userIdList) => unchecked((int)0x80004005);

    public int FindUserByPassportId(string passportId, out int userId)
    {
        userId = 0;
        return unchecked((int)0x80004005);
    }

    public int RefreshUserTile(int userId) => unchecked((int)0x80004005);

    public int CleanupUserData(int userId) => unchecked((int)0x80004005);
}
