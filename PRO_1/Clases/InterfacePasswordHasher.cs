using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRO_1.Clases
{
    internal interface InterfacePasswordHasher
    {
        string Hash(string password);

        bool Verify(string passwordHash, string inputPassword);
    }
}
