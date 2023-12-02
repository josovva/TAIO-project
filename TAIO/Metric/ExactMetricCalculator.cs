using TAIO.Metric.Utils.Extensions;

namespace TAIO.Metric;

public class ExactMetricCalculator : MetricCalculator
{
    protected override int GetMinimumDistance(Graph g1, Graph g2)
    {
        var minDist = int.MaxValue;

        foreach (var perm in Enumerable.Range(0, g1.NumberOfVertices).GetPermutations())
        {
            var permMatrix = perm.GetPermutationMatrix();
            var invPermMatrix = permMatrix.Transpose();

            var permutedMatrix = permMatrix.Multiply(g1.AdjustmentMatrix).Multiply(invPermMatrix);
            var dist = CalculateDistance(permutedMatrix, g2.AdjustmentMatrix);

            if (dist < minDist)
            {
                minDist = dist;
            }
        }

        return minDist;
    }
}