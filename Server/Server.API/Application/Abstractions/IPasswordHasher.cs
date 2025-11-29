namespace Server.API.Application.Abstractions
{
    public interface IPasswordHasher
    {
        void HashPassword(string password, out byte[] hash, out byte[] salt);

        bool VerifyPassword(string password, byte[] hash, byte[] salt);
    }
}
