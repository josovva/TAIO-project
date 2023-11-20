namespace TAIO;

public static class MaxCommonSubgraph
{
    public static void FindExact(int[,] A, int[,] B)
    {
        var L = new List<(int, int)>();
        for (var i = 0; i < A.GetLength(0); ++i)
        {
            for (var j = 0; j < B.GetLength(0); ++j)
            {
                L.Add((i, j));
            }
        }

        var c = L.Count;
        var C = Enumerable.Repeat(0, c * c).Chunk(c).ToArray();

        Console.WriteLine("C:");
        for (var n = 1; n < c; ++n)
        {
            Console.Write($"({L[n].Item1}, {L[n].Item2}) [ ");

            for (var m = 0; m < n; ++m)
            {
                (var i, var j) = L[n];
                (var k, var l) = L[m];

                if (i != k && j != l && AreCompatible(L[n], L[m]) && AreCompatible(L[m], L[n]))
                {
                    C[n][m] = Math.Min(A[i, k], B[j, l]) + 1;
                    C[m][n] = Math.Min(A[k, i], B[l, j]) + 1;
                }

                Console.Write($"{C[n][m]}\\{C[m][n]} ");
            }

            Console.Write("]");
            Console.WriteLine();
        }
        Console.WriteLine($"({L[0].Item1}, {L[0].Item2})");

        var maxClique = MaxClique.FindExact(C, Math.Min(A.GetLength(0), B.GetLength(0)));
        var mapping = maxClique.Select(x => L[x]);

        Console.WriteLine("A subgraph vertices:");
        foreach (var a in mapping.Select(x => x.Item1))
        {
            Console.Write(a + " ");
            Console.WriteLine();
        }

        Console.WriteLine("B subgraph vertices:");
        foreach (var b in mapping.Select(x => x.Item2))
        {
            Console.Write(b + " ");
            Console.WriteLine();
        }

        // inner utils
        bool AreCompatible((int i, int j) map1, (int k, int l) map2)
        {
            var sum = A[map1.i, map2.k] + B[map1.j, map2.l];
            var prod = A[map1.i, map2.k] * B[map1.j, map2.l];
            return sum == 0 || prod != 0;
        }
    }
}
