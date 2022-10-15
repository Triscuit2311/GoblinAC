using System;
using System.Collections.Generic;

namespace GoblinAC.Server.Modules.Crypto;

public static class NumericalProvider
{
    internal static int[] GenerateKeys(int num)
    {
        var gen = new Random();
        var keys = new List<int>();
        for (var i = 0; i < num; i++) keys.Add(gen.Next(-10000, 10000));
        return keys.ToArray();
    }
}