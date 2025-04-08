namespace ApiCurso.Security
{
    using System;
    using System.Security.Cryptography;

    public class PasswordHasher
    {
        private const int SaltSize = 16; // 16 bytes = 128 bits
        private const int KeySize = 32;  // 32 bytes = 256 bits
        private const int Iterations = 10000; // Número de iteraciones

        public static string HashPassword(string password)
        {
            using var rng = RandomNumberGenerator.Create();
            byte[] salt = new byte[SaltSize];
            rng.GetBytes(salt);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            byte[] key = pbkdf2.GetBytes(KeySize);

            // Concatenamos salt + hash y lo convertimos en base64 para almacenar
            byte[] hashBytes = new byte[SaltSize + KeySize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(key, 0, hashBytes, SaltSize, KeySize);

            return Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyPassword(string password, string storedHash)
        {
            byte[] hashBytes = Convert.FromBase64String(storedHash);

            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            byte[] key = pbkdf2.GetBytes(KeySize);

            // Comparamos el hash generado con el almacenado
            for (int i = 0; i < KeySize; i++)
            {
                if (hashBytes[SaltSize + i] != key[i])
                    return false;
            }

            return true;
        }
    }
}