using System;
using System.Security.Cryptography;
using AuthMicroService.Interfaces;

namespace AuthMicroService.Services
{
    public class CryptoService : ICryptoService
    {
        private const int hashIteration = 100, hashPasswordLength = 20;
        
        public string ComputeHash(string password, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);
            using (var rfc = new Rfc2898DeriveBytes(password,saltBytes,hashIteration))
            {
                return Convert.ToBase64String(rfc.GetBytes(hashPasswordLength));
            }
        }

        public string GenerateSalt()
        {
            var salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            };
            return Convert.ToBase64String(salt);
        }
    }
}