using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Registration_with_email_validation
{
    public static class Crypto
    {

        public static string Hash (string value) //This method coverts plain text to hash values
        {

            return Convert.ToBase64String(
                System.Security.Cryptography.SHA256.Create()
                .ComputeHash(System.Text.Encoding.UTF8.GetBytes(value))
                );


        }




    }
}