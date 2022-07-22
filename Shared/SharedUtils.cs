
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Goblin.Shared
{
    public static class SharedUtils
    {
        public static string RandomAscii(int len)
        {
            var crypt = new RNGCryptoServiceProvider();
            var buf = new byte[len]; 
            crypt.GetBytes(buf);
            return Convert.ToBase64String(buf);
        }
        
        public static string ComposeHash(string globalKey, string clientKey, string input)
        {
            var builder = new StringBuilder();
        
            for (var i = 0; i < input.Length; i++)
            {
                char c = (char) (input[i] ^ globalKey[i]^ clientKey[i]);
                builder.Append(c);
            }
            return builder.ToString();
        }

        
        public static int ConcealInt(IEnumerable<int> keys, int num)
        {
            return num + keys.Sum();
        }
        
        public static int RevealInt(int[] keys, int num)
        {
            return num - keys.Sum();
        }
        
        public static double ConcealDouble(IEnumerable<int> keys, double num)
        {
            return num + keys.Sum();
        }
        
        public static double RevealDouble(int[] keys, double  num)
        {
            return num - keys.Sum();
        }

    }
}