using System.Security.Cryptography;

namespace DocStorage.Application.Auth
{

    public class Security
    {

        public static string HashPassword(string password)
        {
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 10000))
            {
                byte[] hash = deriveBytes.GetBytes(32);
                byte[] hashWithSalt = new byte[48];
                Array.Copy(hash, 0, hashWithSalt, 0, 32);
                Array.Copy(saltBytes, 0, hashWithSalt, 32, 16);
                return Convert.ToBase64String(hashWithSalt);
            }
        }

        public static bool Verify(string passwordHash, string password)
        {

            byte[] hashWithSalt = Convert.FromBase64String(passwordHash);
            byte[] salt = new byte[16];
            Array.Copy(hashWithSalt, 32, salt, 0, 16);

            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                byte[] hash = deriveBytes.GetBytes(32);
                for (int i = 0; i < 32; i++)
                {
                    if (hash[i] != hashWithSalt[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
