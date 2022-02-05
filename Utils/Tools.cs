public static class Tools
{
    public static string HashedPassword(string password)
    {
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

        return passwordHash;
    }

    public static bool VerifPassword(string password, string hash)
    {
       bool verified = BCrypt.Net.BCrypt.Verify(password, hash);

        return verified;
    }
}