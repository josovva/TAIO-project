namespace TAIO.Metric.Utils.Extensions;

public static class MatrixExtensions
{
    public static int[,] GetPermutationMatrix(this IEnumerable<int> permutation)
    {
        var permList = permutation.ToList();
        var n = permList.Count;

        var result = new int[n, n];

        for (var i = 0; i < n; i++)
        {
            result[i, permList[i]] = 1;
        }

        return result;
    }

    public static int[,] Transpose(this int[,] permMatrix)
    {
        var n = permMatrix.GetLength(0);
        var result = new int[n, n];

        for (var i = 0; i < n; i++)
        {
            for (var j = 0; j < n; j++)
            {
                result[j, i] = permMatrix[i, j];
            }
        }

        return result;
    }

    public static int[,] Multiply(this int[,] firstMatrix, int[,] secondMatrix)
    {
        var n = firstMatrix.GetLength(0);
        var result = new int[n, n];

        for (var i = 0; i < n; i++)
        {
            for (var j = 0; j < n; j++)
            {
                for (var k = 0; k < n; k++)
                {
                    result[i, j] += firstMatrix[i, k] * secondMatrix[k, j];
                }
            }
        }

        return result;
    }
}