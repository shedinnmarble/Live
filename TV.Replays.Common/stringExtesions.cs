using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace TV.Replays.Common
{
    public static class StringExtesions
    {
        public static string GetMD5(this string str)
        {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            StringBuilder sb = new StringBuilder();
            bytes = provider.ComputeHash(bytes);

            foreach (byte b in bytes)
                sb.Append(b.ToString("x2").ToLower());

            return sb.ToString();
        }
    }
}
