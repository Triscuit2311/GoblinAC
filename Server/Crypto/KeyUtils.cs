using System;
using System.Collections.Generic;
using System.Text;

namespace Goblin.Server.Crypto;

internal class KeyUtils
{
    internal static int[] GetNumericalKeys(int num)
    {
        var gen = new Random();
        var keys = new List<int>();
        for (int i = 0; i < num; i++)
        {
            keys.Add(gen.Next(-10000,10000));
        }
        return keys.ToArray();
    }
    
    internal static string GetKey(int len)
    {
        var r = new Random();
        var sb = new StringBuilder();
        for (var i = 0; i < len; i++)
        {
            sb.Append(Charset.UnicodeChars[r.Next(0,2501)]);
        }
        return sb.ToString();
    }
}