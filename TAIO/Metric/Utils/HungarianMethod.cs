namespace TAIO.Metric.Utils;

public static class HungarianMethod
{
    public static double Optimize(double[,] A, out List<(int from, int to)> mapping)
    {
        var n = A.GetLength(0) - 1;
        var m = A.GetLength(1) - 1;

        var u = new double[n + 1];
        var v = new double[m + 1];
        var p = new int[m + 1];
        var way = new int[m + 1];

        for (var i = 1; i <= n; ++i)
        {
            p[0] = i;
            var j0 = 0;
            var minV = new double[m + 1];
            var used = new bool[m + 1];

            for (var g = 0; g <= m; g++)
            {
                minV[g] = double.MaxValue;
                used[g] = false;
            }

            do
            {
                used[j0] = true;
                int i0 = p[j0], j1 = 0;
                var delta = double.MaxValue;

                for (var j = 1; j <= m; ++j)
                {
                    if (!used[j])
                    {
                        var cur = A[i0, j] - u[i0] - v[j];
                        if (cur < minV[j])
                        {
                            minV[j] = cur;
                            way[j] = j0;
                        }
                        if (minV[j] < delta)
                        {
                            delta = minV[j];
                            j1 = j;
                        }
                    }
                }

                for (var j = 0; j <= m; ++j)
                {
                    if (used[j])
                    {
                        u[p[j]] += delta;
                        v[j] -= delta;
                    }
                    else
                    {
                        minV[j] -= delta;
                    }
                }

                j0 = j1;
            } while (p[j0] != 0);
            do
            {
                var j1 = way[j0];
                p[j0] = p[j1];
                j0 = j1;
            } while (j0 != 0);
        }

        mapping = new List<(int, int)>();
        for (var i = 1; i <= m; ++i)
        {
            mapping.Add((p[i], i));
        }

        return -v[0];
    }
}