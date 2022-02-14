namespace SjxLogistics.Controllers.AuthenticationComponent
{
    public interface IpasswordHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string passwordHash);
    }
}
