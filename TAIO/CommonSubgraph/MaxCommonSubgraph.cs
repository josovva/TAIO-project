using TAIO.GeneticAlg;

namespace TAIO.CommonSubgraph;

public static class MaxCommonSubgraph
{
    public static Dictionary<int, int> FindExact(Graph a, Graph b)
    {
        CreateCorrespondanceMatrix(a, b, out var L, out var c);

        var maxClique = BrutalMaxClique.FindExact(c, Math.Min(a.NumberOfVertices, b.NumberOfVertices));
        var mapping = maxClique.Select(x => L[x]);

        return mapping.ToDictionary(x => x.Item1, y => y.Item2);
    }

    public static Dictionary<int, int> FindApprox(Graph a, Graph b)
    {
        CreateCorrespondanceMatrix(a, b, out var L, out var c);

        var n = c.NumberOfVertices;

        var maxEdgeWeight = 0;
        for (var i = 0; i < n; ++i)
        {
            for (var j = 0; j < n; ++j)
            {
                if (c.GetWeight(i,j) > maxEdgeWeight)
                    maxEdgeWeight = c.GetWeight(i,j);
            }
        }

        var maxClique = MaxClique.Calculate(new(c.AdjustmentMatrix), 1, 0, (genome) => genome.NumberOfVertices + (double)genome.TotalEdgeWeight / (n * (n - 1) * maxEdgeWeight));
        var mapping = maxClique.Item2.Select(x => L[x]).ToList();

        return mapping.ToDictionary(x => x.Item1, y => y.Item2);
    }

    private static bool AreCompatible(Graph A, Graph B, (int i, int j) map1, (int k, int l) map2)
    {
        var sum = A.GetWeight(map1.i, map2.k) + B.GetWeight(map1.j, map2.l);
        var prod = A.GetWeight(map1.i, map2.k) * B.GetWeight(map1.j, map2.l);
        return sum == 0 || prod != 0;
    }

    private static void CreateCorrespondanceMatrix(Graph a, Graph b, out List<(int, int)> L, out Graph c)
    {
        L = new List<(int, int)>();
        for (var i = 0; i < a.NumberOfVertices; ++i)
        {
            for (var j = 0; j < b.NumberOfVertices; ++j)
            {
                L.Add((i, j));
            }
        }

        var count = L.Count;
        c = new Graph(new int[count, count]);
        for (var n = 1; n < count; ++n)
        {
            for (var m = 0; m < n; ++m)
            {
                (var i, var j) = L[n];
                (var k, var l) = L[m];

                if (i != k && j != l && AreCompatible(a, b, L[n], L[m]) && AreCompatible(a, b, L[m], L[n]))
                {
                    c.AdjustmentMatrix[n, m] = Math.Min(a.GetWeight(i, k), b.GetWeight(j, l)) + 1;
                    c.AdjustmentMatrix[m, n] = Math.Min(a.GetWeight(k, i), b.GetWeight(l, j)) + 1;
                }
            }
        }
    }
}
