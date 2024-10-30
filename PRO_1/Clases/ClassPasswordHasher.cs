using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PRO_1.Clases
{
    class ClassPasswordHasher : InterfacePasswordHasher
    {

        public string Hash(string password)
        {
            using (SHA256 hash = SHA256.Create())
            {
                byte[] hashedBytes = hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public bool Verify(string password, string hashedPassword)
        {
            string newHash = Hash(password);
            return newHash == hashedPassword;
        }
    }
}
