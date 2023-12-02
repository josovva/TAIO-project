using TAIO.CommonSubgraph;
using TAIO.CommonSubgraph.Utils;

namespace TAIO.MaxCliqueAlg;

public class BrutalMaxClique
{
    /// <summary>
    /// Finds exact solution for the maximum clique problem using brute-force algorithm.
    /// </summary>
    /// <param name="A"></param>
    /// <param name="maxClique"></param>
    /// <returns></returns>
    public static List<int> FindExact(Graph g, int? maxClique = null)
    {
        var combinations = GetCombination(g.NumberOfVertices, maxClique).ToList();
        combinations.Sort(new ListComparison());

        var result = (Array.Empty<int>(), 0);
        foreach (var subgraph in combinations)
        {
            var isClique = true;
            var edges = 0;

            foreach (var i in subgraph)
            {
                foreach (var j in subgraph.Where(j => i != j))
                {
                    if (g.GetWeight(i, j) == 0)
                    {
                        isClique = false;
                        break;
                    }

                    edges += g.GetWeight(i, j);
                }

                if (!isClique)
                {
                    break;
                }
            }

            if (isClique && edges > result.Item2)
            {
                result = (subgraph.ToArray(), edges);
            }
        }

        return result.Item1.ToList();
    }

    /// <summary>
    /// Gets all combinations of 0..<paramref name="n"/> numbers of the maximum length equal to <paramref name="maxCount"/> (if specified)
    /// </summary>
    private static IEnumerable<List<int>> GetCombination(int n, int? maxCount = null)
    {
        var count = Math.Pow(2, n);
        for (int i = 1; i <= count - 1; i++)
        {
            var str = Convert.ToString(i, 2).PadLeft(n, '0');

            if (maxCount is not null && str.Where(x => x != '0').ToList().Count > maxCount)
            {
                continue;
            }

            var L = new List<int>();
            for (int j = 0; j < str.Length; j++)
            {
                if (str[j] == '1')
                {
                    L.Add(j);
                }
            }

            yield return L;
        }
    }

    /// <summary>
    /// Calculate maximal Q_{k,l,p} clique with given l as <paramref name="clique_order"/>.
    /// </summary>
    /// <param name="matrix"></param>
    /// <param name="clique_order"></param>
    /// <param name="porosity"></param>
    /// <returns></returns>
    public static (double, List<int>) Calcuate(int[,] matrix, int clique_order, double porosity)
    {
        var max_arch_weight = GetMaxWeightOfArch(matrix);

        var size = matrix.GetLength(0);
        var max_clique = new List<int>();
        double max_size = -1;
        double clique_porosity = 0;

        foreach (var subgraph in GetCombination(size))
        {
            double subgraph_porosity = 0;
            var subgraph_size = FintessFunctionMaxClique(subgraph, matrix, max_arch_weight);
            if (CliqueTester.IsThisCliqueCorrect(subgraph, matrix, ref subgraph_porosity, clique_order, porosity) && subgraph_size > max_size)
            {
                max_clique = subgraph;
                max_size = subgraph_size;
                clique_porosity = subgraph_porosity;
            }
        }

        return (clique_porosity, max_clique);
    }

    public static double FintessFunctionMaxClique(List<int> subgraph, int[,] matrix, int max_arch_weight)
    {
        var n = matrix.GetLength(0);

        var total_edge_weight = 0;
        foreach (var u in subgraph)
        {
            foreach (var v in subgraph)
            {
                if (u == v) continue;
                total_edge_weight += matrix[u, v];
            }
        }

        return subgraph.Count + (double)total_edge_weight / (n * (n - 1) * max_arch_weight);
    }

    public static int GetMaxWeightOfArch(int[,] matrix)
    {
        var n = matrix.GetLength(0);

        var max_arch_weight = 0;
        for (var i = 0; i < n; ++i)
        {
            for (var j = 0; j < n; ++j)
            {
                if (matrix[i, j] > max_arch_weight)
                    max_arch_weight = matrix[i, j];
            }
        }

        return max_arch_weight;
    }

}
