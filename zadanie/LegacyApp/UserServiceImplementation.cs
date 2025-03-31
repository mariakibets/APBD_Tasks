using LegacyApp.interfaces;

namespace LegacyApp;

public class UserServiceImplementation : IUserService
{
    public void AddUser(User user)
    {
        UserDataAccess.AddUser(user);  
    }
    
}