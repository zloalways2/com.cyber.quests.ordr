using System;
using System.Collections.Generic;

public static class NewClass
{
    private static Random rng = new Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int ncyber = list.Count;
        while (ncyber > 1)
        {
            ncyber--;
            int k = rng.Next(ncyber + 1);
            T valuecyber = list[k];
            list[k] = list[ncyber];
            list[ncyber] = valuecyber;
        }
    }
}

