namespace TAIO.Metric;

public abstract class MetricCalculator : IMetricCalculator
{
    protected abstract int GetMinimumDistance(Graph g1, Graph g2);

    public int Calculate(Graph g1, Graph g2)
    {
        var sizeDiff = Math.Abs(g1.NumberOfVertices - g2.NumberOfVertices);

        if (g1.NumberOfVertices > g2.NumberOfVertices)
        {
            g2.AddVertices(sizeDiff);
        }
        else if (g1.NumberOfVertices < g2.NumberOfVertices)
        {
            g1.AddVertices(sizeDiff);
        }

        var minDist = GetMinimumDistance(g1, g2);

        return sizeDiff + minDist;
    }

    protected static int CalculateDistance(int[,] firstMatrix, int[,] secondMatrix)
    {
        var n = firstMatrix.GetLength(0);
        var result = 0;

        for (var i = 0; i < n; i++)
        {
            for (var j = 0; j < n; j++)
            {
                result += Math.Abs(firstMatrix[i, j] - secondMatrix[i, j]);
            }
        }

        return result;
    }
}