using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Business.Helpers
{
    public class PasswordHelper
    {
        private const int SaltSize = 16; // 128-bit salt
        private const int KeySize = 32; // 256-bit key
        private const int Iterations = 1000; // Recommended iterations (you may increase it for stronger security)

        public static string HashPassword(string password)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                // Generate a salt
                byte[] salt = new byte[SaltSize];
                rng.GetBytes(salt);

                // Hash the password with the salt
                var hash = GetHash(password, salt);

                // Return the salt and hash together as a single string (for example, in base64)
                return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
            }
        }

        public static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            // Split the stored hash into the salt and the actual hash
            var parts = storedHash.Split(':');
            if (parts.Length != 2)
                return false;

            var salt = Convert.FromBase64String(parts[0]);
            var hash = Convert.FromBase64String(parts[1]);

            // Hash the entered password with the extracted salt
            var hashOfEnteredPassword = GetHash(enteredPassword, salt);

            // Compare the entered password's hash with the stored hash
            return hashOfEnteredPassword.SequenceEqual(hash);
        }

        private static byte[] GetHash(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                return pbkdf2.GetBytes(KeySize);
            }
        }
    }
}
