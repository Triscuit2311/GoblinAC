using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Goblin.Shared;

public static class Utilities
{
    public static string EncodeStr(string global, string client, string input)
    {
        var b = new StringBuilder();
        for (var i = 0; i < input.Length; i++)
        {
            var n = i >= global.Length ? i - global.Length : i;
            b.Append((char)(input[i] ^ global[n] ^ client[n]));
        }

        return b.ToString();
    }

    public static int EncodeInt(IEnumerable<int> keys, int num)
    {
        return num + keys.Sum();
    }

    public static int DecodeInt(IEnumerable<int> keys, int num)
    {
        return num - keys.Sum();
    }

    public static double EncodeDouble(IEnumerable<int> keys, double num)
    {
        return num + keys.Sum();
    }

    public static double DecodeDouble(IEnumerable<int> keys, double num)
    {
        return num - keys.Sum();
    }
}