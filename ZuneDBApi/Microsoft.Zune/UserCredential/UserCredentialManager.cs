namespace Microsoft.Zune.UserCredential;

// Original wraps a native IUserCredentialManager* singleton. Not reverse engineered —
// see logs/Microsoft.Zune/User/UserManager.md.
public class UserCredentialManager
{
    private static UserCredentialManager sm_instance;

    public static UserCredentialManager Instance => sm_instance ??= new UserCredentialManager();

    private UserCredentialManager()
    {
    }

    public int SetCredentialHandler(UserCredentialHandler credentialHandler) => unchecked((int)0x80004005);
}
