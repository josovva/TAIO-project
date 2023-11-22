using TAIO.Metric.Utils;
using TAIO.Metric.Utils.Extensions;

namespace TAIO.Metric;

public class ApproximateMetricCalculator : MetricCalculator
{
    protected override int GetMinimumDistance(Graph g1, Graph g2)
    {
        var n = g1.NumberOfVertices;
        var m = new double[n + 1, n + 1];

        for (var i = 0; i < n; i++)
        {
            for (var j = 0; j < n; j++)
            {
                m[i + 1, j + 1] = GetValueAtCellHeuristic(g1, g2, i, j);
            }
        }

        HungarianMethod.Optimize(m, out var mapping);

        var permutation = new int[n];
        foreach (var (from, to) in mapping)
        {
            permutation[from - 1] = to - 1;
        }

        var permMatrix = permutation.GetPermutationMatrix();
        var invPermMatrix = permMatrix.Transpose();

        var permutedG1Matrix = permMatrix.Multiply(g1.AdjustmentMatrix).Multiply(invPermMatrix);
        return CalculateDistance(permutedG1Matrix, g2.AdjustmentMatrix);
    }

    private static double GetValueAtCellHeuristic(Graph g1, Graph g2, int row, int col)
    {
        const double alpha = 0.8;
        const double edgeDeletionCost = 3;

        double VertexSubstitutionCost(int u, int v) => alpha * Math.Pow(u - v, 2);
        double EdgeSubstitutionCost((int u, int v) p, (int u, int v) q) => (1 - alpha) * Math.Abs(g1.GetWeight(p.u, p.v) - g2.GetWeight(q.u, q.v));
        double EdgeDeletionCost((int u, int v) p, Graph g) => (1 - alpha) * g.GetWeight(p.u, p.v) * edgeDeletionCost;

        var n1 = GetNeighboursMapping(g1, row, out var g1Mapping);
        var n2 = GetNeighboursMapping(g2, col, out var g2Mapping);

        var m = new double[n1 + n2 + 1, n1 + n2 + 1];

        for (var i = 0; i < n1 + n2; i++)
        {
            for (var j = 0; j < n1 + n2; j++)
            {
                if (i < n1 && j < n2)
                {
                    m[i + 1, j + 1] = EdgeSubstitutionCost((row, g1Mapping[i]), (col, g2Mapping[j]));
                }
                else if (i < n1 && j >= n2)
                {
                    m[i + 1, j + 1] = j - n2 == i
                        ? EdgeDeletionCost((row, g1Mapping[j - n2]), g1)
                        : double.MaxValue;
                }
                else if (i >= n1 && j < n2)
                {
                    m[i + 1, j + 1] = i - n1 == j
                        ? EdgeDeletionCost((col, g2Mapping[i - n1]), g2)
                        : double.MaxValue;
                }
            }
        }

        return VertexSubstitutionCost(row, col) + HungarianMethod.Optimize(m, out _) / 2;
    }

    private static int GetNeighboursMapping(Graph g, int vertex, out int[] neighboursMapping)
    {
        var neighboursCount = Enumerable.Range(0, g.NumberOfVertices).Count(i => g.AdjustmentMatrix[vertex, i] != 0);
        neighboursMapping = new int[neighboursCount];
        var index = 0;

        for (var i = 0; i < g.NumberOfVertices; i++)
        {
            var cellValue = g.AdjustmentMatrix[vertex, i];
            if (cellValue != 0)
            {
                neighboursMapping[index++] = i;
            }
        }

        return neighboursCount;
    }

    private static int GetValueAtCellSimple(Graph g1, Graph g2, int row, int col)
    {
        var permutation = Enumerable.Range(0, g1.NumberOfVertices).ToList();
        (permutation[row], permutation[col]) = (permutation[col], permutation[row]);

        var permMatrix = permutation.GetPermutationMatrix();
        var invPermMatrix = permMatrix.Transpose();

        var permutedG1Matrix = permMatrix.Multiply(g1.AdjustmentMatrix).Multiply(invPermMatrix);

        return CalculateDistance(permutedG1Matrix, g2.AdjustmentMatrix);
    }
}