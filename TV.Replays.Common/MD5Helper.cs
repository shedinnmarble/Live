using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace TV.Replays.Common
{
    public static class Md5Helper
    {
        public static string MD5FromString(string str)
        {
            MD5 md5 = MD5.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            bytes = md5.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("x2"));
            }
            md5.Clear();
            return sb.ToString();
        }

        public static string MD5FromFilePath(string filePath)
        {
            MD5 md5 = MD5.Create();
            StringBuilder sb = new StringBuilder();
            using (FileStream fs = File.OpenRead(filePath))
            {
                byte[] bytes = md5.ComputeHash(fs);
                for (int i = 0; i < bytes.Length; i++)
                {
                    sb.Append(bytes[i].ToString("x2"));
                }
            }
            md5.Clear();
            return sb.ToString();
        }

        public static string MD5FromFile(Stream stream)
        {
            MD5 md5 = MD5.Create();
            StringBuilder sb = new StringBuilder();
            byte[] bytes = md5.ComputeHash(stream);
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("x2"));
            }
            md5.Clear();
            return sb.ToString();
        }
    }
}
