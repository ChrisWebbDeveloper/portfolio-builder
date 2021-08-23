using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioBuilder.Security
{
    public static class Encryption
    {
        
        [Authorize]
        public static string Encrypt(IDataProtector protector, string passwordToEncrypt)
        {
            if (!String.IsNullOrEmpty(passwordToEncrypt))
            {
                return protector.Protect(passwordToEncrypt);
            }
            return null;
        }

        [Authorize]
        public static string Decrypt(IDataProtector protector, string passwordHash)
        {
            if (!String.IsNullOrEmpty(passwordHash))
            {
                return protector.Unprotect(passwordHash);
            }
            return null;
        }
    }
}
