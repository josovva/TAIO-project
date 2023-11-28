namespace TAIO;

public class BrutalMaxClique
{
    /// <summary>
    /// Finds exact solution for the maximum clique problem using brute-force algorithm.
    /// </summary>
    /// <param name="A"></param>
    /// <param name="maxClique"></param>
    /// <returns></returns>
    public static int[] FindExact(int[][] A, int? maxClique = null)
    {
        var combinations = GetCombination(A.Length, maxClique).ToList();
        combinations.Sort(new ListComparison());

        var result = (Array.Empty<int>(), 0);
        foreach (var subgraph in combinations)
        {
            var isClique = true;
            var edges = 0;

            foreach (var i in subgraph)
            {
                foreach (var j in subgraph.Where(j => j != i))
                {
                    if (A[i][j] == 0)
                    {
                        isClique = false;
                        break;
                    }

                    edges += A[i][j];
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

        return result.Item1;
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
}
